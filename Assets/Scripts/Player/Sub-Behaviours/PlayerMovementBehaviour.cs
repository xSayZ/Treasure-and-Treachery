// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Script for handling player movement
// --------------------------------
// ------------------------------*/

using System.Collections;
using Game.Core;
using UnityEngine;


namespace Game {
    namespace Player {
        public class PlayerMovementBehaviour : MonoBehaviour
        {
            [Header("Settings")]
            [SerializeField] private CapsuleCollider playerCollider;
            [SerializeField] private CapsuleCollider dashCollider;
            
            [Header("Movement Settings")]
            [Tooltip("Base Movement Speed of the player. This is the speed the player moves at when not dashing.")]
            [SerializeField] private float movementSpeed;
            [Tooltip("How fast the player turns.")]
            [SerializeField] private float turnSpeed;
            
            [Header("Dash Settings")]
            [Tooltip("Addition modifier adds modified speed to the dash speed.")]
            [SerializeField] private float dashSpeedModifier;
            [Tooltip("How long should you be able to dash.")]
            [Range(0, 3)]
            [SerializeField] private float dashTime;
            [Tooltip("How long should the cooldown be for the dash.")]
            [Range(0, 30)]
            [SerializeField] private float dashRechargeTime;
            [Tooltip("Number of dashes the player has.")]
            [SerializeField] private int numberOfDashes;
            [Tooltip("How much damage the dash deals to enemies.")]
            [SerializeField] private int dashDamage;
            
            // References
            private Rigidbody playerRigidBody;
            private PlayerController playerController;
            
            // Movement values
            private Vector3 movementDirection;
            private float currentMaxSpeed;
            private bool isForceMoving;
            
            // Dash values
            private float currentNumberOfDashes;
            private float currentDashRechargeTime;
            
            public bool canMove { get; private set; } = true;
            private bool canRotate = true;
            
            public void SetupBehaviour(PlayerController _playerController)
            {
                playerController = _playerController;
                playerRigidBody = GetComponent<Rigidbody>();
                currentNumberOfDashes = numberOfDashes;
                currentMaxSpeed = movementSpeed;
            }

#region Validation
            private void OnValidate()
            {
                if(movementSpeed < 0)
                {
                    Debug.LogWarning("Movement Speed needs to be higher than 0");
                    movementSpeed = 0;
                }
                if (dashTime < 0)
                {
                    Debug.LogWarning("Dash Time needs to be higher than 0");
                    dashTime = 0;
                }
            }
#endregion
            
#region Unity Functions
            private void FixedUpdate()
            {
                if (canRotate)
                {
                    TurnPlayer();
                }
                
                if (canMove)
                {
                    MovePlayer();
                    ClampPlayerPosition();
                }

                if (currentDashRechargeTime <= 0 && currentNumberOfDashes < numberOfDashes)
                {
                    currentNumberOfDashes++;
                    
                    if (currentNumberOfDashes < numberOfDashes)
                    {
                        currentDashRechargeTime = dashRechargeTime;
                    }
                }
                else if (currentDashRechargeTime > 0)
                {
                    currentDashRechargeTime -= Time.deltaTime;
                }
            }

            // Damage enemies when dashing through them
            private void OnTriggerEnter(Collider other)
            {
                if (!other.isTrigger && other.CompareTag("Enemy"))
                {
                    if (other.TryGetComponent(out IDamageable _hit))
                    {
                        _hit.Damage(dashDamage);
                    }
                }
            }
#endregion

#region Public Functions
            public void UpdateMovementData(Vector3  _newMovementDirection)
            {
                if (!isForceMoving)
                {
                    movementDirection = _newMovementDirection;
                }
            }

            public void SetMovementActiveState(bool _movement, bool _rotate)
            {
                canMove = _movement;
                canRotate = _rotate;
            }

            public void Dash()
            {
                if (currentNumberOfDashes > 0)
                {
                    currentNumberOfDashes--;
                    currentDashRechargeTime = dashRechargeTime;
                    
                    StartCoroutine(DashMove());
                }
            }

            public void ApplyForce(float _speed, Vector3 _direction, float _time, bool _keepFacingRotation = false)
            {
                StartCoroutine(ForceMove(_speed, _direction, _time, _keepFacingRotation));
            }

            public float TurnSpeed {
                get {
                    return turnSpeed;
                } set {
                    turnSpeed = value;
                }
            }
#endregion

#region Private Functions
            private void MovePlayer()
            {
                Vector3 _movement = Time.deltaTime * currentMaxSpeed * movementDirection;
                playerRigidBody.AddForce(_movement,ForceMode.VelocityChange);
            }

            private void TurnPlayer()
            {
                if (movementDirection.sqrMagnitude > 0.01f && movementDirection != Vector3.zero)
                {
                    var _rotation = Quaternion.Slerp(playerRigidBody.rotation, Quaternion.LookRotation(movementDirection), turnSpeed);
                    playerRigidBody.rotation = _rotation;
                }
            }

            private IEnumerator DashMove()
            {
                currentMaxSpeed = movementSpeed + dashSpeedModifier;
                playerController.SetInvincibility(dashTime);
                
                playerCollider.isTrigger = true;
                dashCollider.enabled = true;
                
                yield return new WaitForSeconds(dashTime);
                
                playerCollider.isTrigger = false;
                dashCollider.enabled = false;
                
                currentMaxSpeed = movementSpeed;
            }

            private IEnumerator ForceMove(float _speed, Vector3 _direction, float _time, bool _keepFacingRotation)
            {
                isForceMoving = true;
                currentMaxSpeed = _speed;
                movementDirection = _direction.normalized;
                
                if (_keepFacingRotation)
                {
                    SetMovementActiveState(true, false);
                }
                
                yield return new WaitForSeconds(_time);
                
                SetMovementActiveState(true, true);
                currentMaxSpeed = movementSpeed;
                isForceMoving = false;
            }

            private void ClampPlayerPosition()
            {
                UnityEngine.Camera camera = UnityEngine.Camera.main;

                Vector3 playerPosition =
                    camera.WorldToViewportPoint(playerRigidBody.transform.position);
                    
                // Clamp the player's position to be within the camera's viewport (0 to 1)

                float clampedX = Mathf.Clamp01(playerPosition.x);
                float clampedY = Mathf.Clamp01(playerPosition.y);

                if (clampedY > 0.9f) clampedY = 0.9f;

                if (clampedX > 0.95f) clampedX = 0.95f;
                if (clampedX < 0.05f) clampedX = 0.05f;
                
                Vector3 newPosition = camera.ViewportToWorldPoint(new Vector3(clampedX, clampedY, playerPosition.z));
                playerRigidBody.position = newPosition;
            }
#endregion
        }
    }
}
