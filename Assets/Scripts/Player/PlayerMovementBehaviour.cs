// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Script for handling player movement
// --------------------------------
// ------------------------------*/

using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


namespace Game {
    namespace Player
    {
        public class PlayerMovementBehaviour : MonoBehaviour
        {

            [Header("MovementSettings")]
            [Tooltip("Effects How smooth the movement Interpolation is. Higher value is smoother movement. Lower value is more responsive movement.")]
            [SerializeField] public float movementSmoothing;

            [Tooltip("Base Movement Speed of the player. This is the speed the player moves at when not dashing.")]
            [SerializeField] private float baseMoveSpeed;
            
            [Tooltip("How fast the player turns")]
            [SerializeField] private float turnSpeed;
            private float currentSpeed;
            
            [Header("Dash Settings")] 
            [Tooltip("Addition modifier adds modified speed to the dash speed.")]
            [SerializeField] private float dashSpeedModifier;
            
            [Tooltip("How long should you be able to dash.")]
            [Range(0, 3)]
            [SerializeField] private float dashTime = 2f;
            
            [Tooltip("How long should the cooldown be for the dash.")]
            [Range(0, 5)]
            [SerializeField] public float dashCooldown = 2f;
            private bool lockout;
            
            // References
            private Rigidbody playerRigidBody;
            
            // Internal Variables
            private Vector3 movement;
            private Vector3 rawInputDirection = Vector3.zero;
            private Vector3 smoothMovementDirection;

#region Validation
            private void OnValidate() {
                if (movementSmoothing < 0) {
                    Debug.LogWarning("movementSmoothing needs to be higher than 0");
                    movementSmoothing = 0;
                }
                if(baseMoveSpeed < 0) {
                    Debug.LogWarning("baseMoveSpeed needs to be higher than 0");
                    baseMoveSpeed = 0;
                }
                if (dashTime < 0) {
                    Debug.LogWarning("lockout period needs to be higher than 0");
                    dashTime = 0;   
                }
            }
#endregion
            
#region Unity Functions
            private void Start()
            {
                currentSpeed = baseMoveSpeed;
                playerRigidBody = GetComponent<Rigidbody>();
                dashCooldown = 0;
            }

            private void Update()
            {
                SmoothInputMovement();
                TurnPlayer();
                DashCompletion();
                MovePlayer();
            }
#endregion

#region Public Functions
            public void MovementData(Vector3 _directionVector)
            {
                rawInputDirection = _directionVector;
            }
#endregion

#region Private Functions
            private void MovePlayer()
            {
                if (rawInputDirection == Vector3.zero)
                {
                    playerRigidBody.velocity = Vector3.zero;
                }
                
                movement = Time.deltaTime * currentSpeed * rawInputDirection;
                playerRigidBody.AddForce(movement,ForceMode.VelocityChange);
            }
            public void TurnPlayer()
            {
                if(smoothMovementDirection.sqrMagnitude > 0.01f)
                {
                    var _rotation = Quaternion.Slerp(playerRigidBody.rotation,
                        Quaternion.LookRotation(smoothMovementDirection), turnSpeed);
    
                    playerRigidBody.rotation = _rotation;
                }
                
            }
    
            public void Dash(bool _dash) {
                if (!_dash)
                    return;
                StartCoroutine(IsDashing());
            }
    
            private IEnumerator IsDashing()
            {
                currentSpeed = baseMoveSpeed + dashSpeedModifier;
                yield return new WaitForSeconds(dashTime);
                currentSpeed = baseMoveSpeed;
                lockout = true;
            }
            private void DashCompletion()
            {
                if (lockout)
                {
                    dashCooldown -=Time.deltaTime;
                }
    
                if (dashCooldown <= 0)
                {
                    lockout = false;
                }
            }
            private void SmoothInputMovement()
            {
                smoothMovementDirection = Vector3.Lerp(smoothMovementDirection, rawInputDirection,
                    Time.deltaTime * movementSmoothing);
            }
#endregion

        }
    }
}
