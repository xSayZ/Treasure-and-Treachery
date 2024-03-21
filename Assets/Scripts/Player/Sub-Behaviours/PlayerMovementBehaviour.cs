// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Script for handling player movement
// --------------------------------
// ------------------------------*/

using System.Collections;
using Game.Core;
using Game.Enemy;
using Game.Quest;
using UnityEngine;
using UnityEngine.Events;


namespace Game {
    namespace Player {
        public class PlayerMovementBehaviour : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private CapsuleCollider playerCollider;
            [SerializeField] private CapsuleCollider dashObjectCollider;
            [SerializeField] private CapsuleCollider dashKillCollider;
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
            [HideInInspector] public float MoveSpeedMultiplier = 1f;
            public float MoveSpeedItemMultiplier { private get; set; } = 1f;
            private bool isForceMoving;
            
            // Dash values
            private float currentNumberOfDashes;
            private float currentDashRechargeTime;
            public bool IsDashing { get; private set; }
            
            // Movement and rotation lock variables
            public bool CameraMoveRotateLock { private get; set; }
            public bool QuestMoveRotateLock { private get; set; }
            public bool AttackStunMoveRotateLock { private get; set; }
            public bool DeathMoveRotateLock { private get; set; }
            
            public bool AimMoveLock { private get; set; }
            
            private bool forceMoveRotateLock; // Locks rotation not movement, name is a bit misleading
            
            // Turn speed
            [HideInInspector] public float CurrentTurnSpeed;
            
            // Werewolf dash
            [HideInInspector] public bool DisableDashMove;
            
            // Events
            [HideInInspector] public UnityEvent OnDash = new UnityEvent();
            [HideInInspector] public UnityEvent<bool> OnDashKill = new UnityEvent<bool>();
            [HideInInspector] public UnityEvent<EnemyController> OnDashThroughEnemy = new UnityEvent<EnemyController>();

            public void SetupBehaviour(PlayerController _playerController)
            {
                playerController = _playerController;
                playerRigidBody = GetComponent<Rigidbody>();
                currentNumberOfDashes = numberOfDashes;
                currentMaxSpeed = movementSpeed;
                CurrentTurnSpeed = turnSpeed;
                
                dashKillCollider.enabled = false;
                
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
                if (CanRotate())
                {
                    TurnPlayer();
                }
                
                if (CanMove())
                {
                    MovePlayer();
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

            private void Update()
            {
                if (CanMove())
                {
                    ClampPlayerPosition();
                }
            }
#endregion

#region Public Functions
            public bool CanMove()
            {
                return !(CameraMoveRotateLock || QuestMoveRotateLock || AttackStunMoveRotateLock || DeathMoveRotateLock || AimMoveLock);
            }

            public bool CanRotate()
            {
                return !(CameraMoveRotateLock || QuestMoveRotateLock || AttackStunMoveRotateLock || DeathMoveRotateLock || forceMoveRotateLock);
            }

            public void UpdateMovementData(Vector3  _newMovementDirection)
            {
                if (!isForceMoving && !IsDashing)
                {
                    movementDirection = _newMovementDirection;
                }
            }

            public void Dash()
            {
                if (currentNumberOfDashes > 0 && !IsDashing && !playerController.PlayerAttackBehaviour.IsAiming && CanMove())
                {
                    // Stops werewolf from losing dash when enraged and has full health
                    if (!(DisableDashMove && playerController.PlayerData.currentHealth == playerController.PlayerData.startingHealth))
                    {
                        currentNumberOfDashes--;
                        currentDashRechargeTime = dashRechargeTime;
                        UpdateDashUI();
                    }
                    
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
                        
                        MonoBehaviour _damageableMonoBehaviour = _hit as MonoBehaviour;
                        EnemyController _enemyController = null;
                        
                        if (_damageableMonoBehaviour)
                        {
                            _damageableMonoBehaviour.TryGetComponent(out _enemyController);
                        }
                        
                        if (_killed)
                        {
                            bool _stunKill = false;
                            
                            if (_enemyController != null)
                            {
                                if (_enemyController.GetCurrentState() == _enemyController.StunnedEnemyState)
                                {
                                    _stunKill = true;
                                }
                            }
                            
                            playerController.PlayerAttackBehaviour.OnKill.Invoke(_stunKill);
                            OnDashKill.Invoke(_stunKill);
                        }
                        
                        if (_enemyController != null)
                        {
                            OnDashThroughEnemy.Invoke(_enemyController);
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
                    
                    PlayerController _otherPlayerController = _transform.GetComponent<PlayerController>();
                    _otherPlayerController.PlayerMovementBehaviour.ApplyForce(dashPushSpeed, _pushDirection, dashPushTime);
                    
                    // Make other player drop held item
                    if (_otherPlayerController.PlayerData.currentItem != null)
                    {
                        QuestManager.OnItemDropped.Invoke(_otherPlayerController.PlayerIndex, _otherPlayerController.PlayerData.currentItem, false);
                    }
                }
            }

            public void UpdateCurrentNumberOfDashes(int _amount)
            {
                currentNumberOfDashes += _amount;
                currentNumberOfDashes = Mathf.Clamp(currentNumberOfDashes, 0, numberOfDashes);
                UpdateDashUI();
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
                    var _rotation = Quaternion.Slerp(playerRigidBody.rotation, Quaternion.LookRotation(movementDirection), CurrentTurnSpeed);
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
                dashKillCollider.enabled = true;
                
                yield return new WaitForSeconds(dashTime);
                
                playerCollider.isTrigger = false;
                dashObjectCollider.enabled = false;
                dashKillCollider.enabled = false;
                
                currentMaxSpeed = movementSpeed;
                IsDashing = false;
            }

            private IEnumerator ForceMove(float _speed, Vector3 _direction, float _time, bool _keepFacingRotation)
            {
                isForceMoving = true;
                currentMaxSpeed = _speed;
                movementDirection = _direction.normalized;
                
                forceMoveRotateLock = _keepFacingRotation;
                
                yield return new WaitForSeconds(_time);
                
                forceMoveRotateLock = false;
                
                currentMaxSpeed = movementSpeed;
                isForceMoving = false;
            }

            private void ClampPlayerPosition()
            {
                UnityEngine.Camera camera = UnityEngine.Camera.main;
                
                Vector3 playerViewportPosition = camera.WorldToViewportPoint(transform.position);
                
                // Clamp the player's position to be within the camera's viewport (0 to 1)
                float clampedX = Mathf.Clamp(playerViewportPosition.x, 0.05f, 0.95f);
                float clampedY = Mathf.Clamp(playerViewportPosition.y, 0.05f, 0.90f);
                
                Vector3 newPosition = camera.ViewportToWorldPoint(new Vector3(clampedX, clampedY, playerViewportPosition.z));
                
                if (playerViewportPosition.x != clampedX || playerViewportPosition.y != clampedY)
                {
                    transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
                }
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
                //playerController.PlayerAnimationBehaviour.PlayerAnimator.SetFloat("MovementMultiplier", _movementMultiplier);
            }
#endregion
        }
    }
}