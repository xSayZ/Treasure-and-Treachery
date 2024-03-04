// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Script for handling player movement
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Enemy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Game {
    namespace Player {
        public class PlayerMovementBehaviour : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private CapsuleCollider playerCollider;
            [SerializeField] private CapsuleCollider dashObjectCollider;
            [SerializeField] private GameObject fullDashImage;
            [SerializeField] private GameObject halfDashImage;
            
            [Header("Movement Settings")]
            [Tooltip("Base Movement Speed of the player.")]
            [SerializeField] private float movementSpeed;
            [Tooltip("How fast the player turns.")]
            [SerializeField] private float turnSpeed;
            
            [Header("Dash Settings")]
            [Tooltip("Speed of the dash.")]
            [SerializeField] private float dashSpeed;
            [Tooltip("How long time the dash speed is applied for.")]
            [Range(0, 3)]
            [SerializeField] private float dashTime;
            [Tooltip("How long the dash cooldown is.")]
            [Range(0, 30)]
            [SerializeField] private float dashRechargeTime;
            [Tooltip("Number of dashes the player has.")]
            [Range(0, 2)]
            [SerializeField] private int numberOfDashes;
            [Tooltip("How much damage the dash deals to enemies.")]
            [SerializeField] private int dashDamage;
            [Tooltip("Speed of the push.")]
            [SerializeField] private float dashPushSpeed;
            [Tooltip("How long time the push speed is applied for.")]
            [Range(0, 3)]
            [SerializeField] private float dashPushTime;
            
            // References
            private Rigidbody playerRigidBody;
            private PlayerController playerController;
            
            // Movement values
            private Vector3 movementDirection;
            private float currentMaxSpeed;
            private bool isForceMoving;
            [HideInInspector] public float MoveSpeedMultiplier = 1f;
            public float MoveSpeedItemMultiplier { private get; set; } = 1f;
            
            // Dash values
            private float currentNumberOfDashes;
            private float currentDashRechargeTime;
            public bool IsDashing { get; private set; }
            
            // Werewolf dash
            public bool DisableDashMove;
            
            public bool canMove { get; private set; } = true;
            private bool canRotate = true;
            
            // Events
            [HideInInspector] public UnityEvent OnDash = new UnityEvent();
            [HideInInspector] public UnityEvent<bool> OnDashKill = new UnityEvent<bool>();
            
            public void SetupBehaviour(PlayerController _playerController)
            {
                playerController = _playerController;
                playerRigidBody = GetComponent<Rigidbody>();
                currentNumberOfDashes = numberOfDashes;
                currentMaxSpeed = movementSpeed;
                
                UpdateDashUI();
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
                
                UpdateMovementAnimationSpeed();
                
                if (currentDashRechargeTime <= 0 && currentNumberOfDashes < numberOfDashes)
                {
                    currentNumberOfDashes++;
                    UpdateDashUI();
                    
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
#endregion

#region Public Functions
            public void UpdateMovementData(Vector3  _newMovementDirection)
            {
                if (!isForceMoving && !IsDashing)
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
                if (currentNumberOfDashes > 0 && !IsDashing && !playerController.PlayerAttackBehaviour.IsAiming && canMove)
                {
                    currentNumberOfDashes--;
                    currentDashRechargeTime = dashRechargeTime;
                    UpdateDashUI();
                    
                    if (!DisableDashMove)
                    {
                        StartCoroutine(DashMove());
                    }
                    
                    OnDash.Invoke();
                }
            }

            public void ApplyForce(float _speed, Vector3 _direction, float _time, bool _keepFacingRotation = false)
            {
                StartCoroutine(ForceMove(_speed, _direction, _time, _keepFacingRotation));
            }

            // Damage enemies when dashing through them
            public void DashKillRangeEntered(Transform _transform)
            {
                if (!IsDashing)
                {
                    return;
                }
                
                if (_transform.CompareTag("Enemy"))
                {
                    if (_transform.TryGetComponent(out IDamageable _hit))
                    {
                        bool _killed = _hit.Damage(dashDamage, transform.position, 0);
                        if (_killed)
                        {
                            bool _stunKill = false;
                            
                            MonoBehaviour _damageableMonoBehaviour = _hit as MonoBehaviour;
                            if (!_damageableMonoBehaviour)
                            {
                                return;
                            }
                            
                            if (_damageableMonoBehaviour.TryGetComponent(out EnemyController _enemyController))
                            {
                                if (_enemyController.GetCurrentState() == _enemyController.StunnedEnemyState)
                                {
                                    _stunKill = true;
                                }
                            }
                            
                            OnDashKill.Invoke(_stunKill);
                        }
                    }
                }
            }
            
            public void DashPushEntered(Transform _transform)
            {
                if (!IsDashing)
                {
                    return;
                }
                
                if (_transform.CompareTag("Player"))
                {
                    Vector3 _pushDirection = transform.right;
                    
                    if (IsLeftOfLine(transform.position, transform.position + transform.forward, _transform.position))
                    {
                        _pushDirection = -transform.right;
                    }
                    
                    _transform.GetComponent<PlayerController>().PlayerMovementBehaviour.ApplyForce(dashPushSpeed, _pushDirection, dashPushTime);
                }
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
                Vector3 _movement;
                
                if (isForceMoving)
                {
                    _movement = Time.deltaTime * currentMaxSpeed * movementDirection;
                }
                else
                {
                    _movement = Time.deltaTime * currentMaxSpeed * MoveSpeedMultiplier * MoveSpeedItemMultiplier * movementDirection;
                }
                
                playerRigidBody.AddForce(_movement, ForceMode.VelocityChange);
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
                IsDashing = true;
                currentMaxSpeed = dashSpeed;
                movementDirection = transform.forward;
                playerController.SetInvincibility(dashTime);
                
                playerCollider.isTrigger = true;
                dashObjectCollider.enabled = true;
                
                yield return new WaitForSeconds(dashTime);
                
                playerCollider.isTrigger = false;
                dashObjectCollider.enabled = false;
                
                currentMaxSpeed = movementSpeed;
                IsDashing = false;
            }

            private IEnumerator ForceMove(float _speed, Vector3 _direction, float _time, bool _keepFacingRotation)
            {
                isForceMoving = true;
                currentMaxSpeed = _speed;
                movementDirection = _direction.normalized;
                
                bool _prevoiusCanMove = canMove;
                bool _prevoiusCanRotate = canRotate;
                
                if (_keepFacingRotation)
                {
                    SetMovementActiveState(true, false);
                }
                
                yield return new WaitForSeconds(_time);
                
                if (_keepFacingRotation)
                {
                    if (canMove == _prevoiusCanMove && canRotate == _prevoiusCanRotate)
                    {
                        SetMovementActiveState(_prevoiusCanMove, _prevoiusCanRotate);
                    }
                    else
                    {
                        SetMovementActiveState(true, true);
                    }
                }
                
                currentMaxSpeed = movementSpeed;
                isForceMoving = false;
            }

            private void ClampPlayerPosition()
            {
                UnityEngine.Camera camera = UnityEngine.Camera.main;
                
                Vector3 playerPosition = camera.WorldToViewportPoint(playerRigidBody.transform.position);
                
                // Clamp the player's position to be within the camera's viewport (0 to 1)
                
                float clampedX = Mathf.Clamp01(playerPosition.x);
                float clampedY = Mathf.Clamp01(playerPosition.y);

                if (clampedY > 0.9f) clampedY = 0.9f;
                
                if (clampedX > 0.95f) clampedX = 0.95f;
                if (clampedX < 0.05f) clampedX = 0.05f;
                
                Vector3 newPosition = camera.ViewportToWorldPoint(new Vector3(clampedX, clampedY, playerPosition.z));
                playerRigidBody.position = new Vector3(newPosition.x, playerRigidBody.position.y, newPosition.z);
            }

            private void UpdateDashUI()
            {
                halfDashImage.SetActive(currentNumberOfDashes >= 1);
                fullDashImage.SetActive(currentNumberOfDashes >= 2);
            }

            private bool IsLeftOfLine(Vector3 _lineStart, Vector3 _lineEnd, Vector3 _point)
            {
                return (_lineEnd.x - _lineStart.x) * (_point.z - _lineStart.z) - (_lineEnd.z - _lineStart.z) * (_point.x - _lineStart.x) > 0;
            }

            private void UpdateMovementAnimationSpeed()
            {
                float _movementMultiplier = (MoveSpeedMultiplier * MoveSpeedItemMultiplier * movementDirection).magnitude;
                playerController.PlayerAnimationBehaviour.PlayerAnimator.SetFloat("MovementMultiplier", _movementMultiplier);
            }
#endregion
        }
    }
}
