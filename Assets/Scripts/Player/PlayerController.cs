// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Script for handling the player
// --------------------------------
// ------------------------------*/

using System;
using Game.Backend;
using Game.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Audio;
using Game.NAME;
using Game.UI;


namespace Game
{
    namespace Player
    {
        public class PlayerController : MonoBehaviour, IDamageable
        {
            [Header("Player Data")]
            [Tooltip("Player Data Scriptable Object")]
            [SerializeField] public PlayerData PlayerData;
            public int PlayerIndex;
            
            [field:Header("Sub Behaviours")]
            [Tooltip("Assign sub behaviours for player")]
            [field:SerializeField] public PlayerMovementBehaviour PlayerMovementBehaviour { get; private set; }
            [field:SerializeField] public PlayerAttackBehaviour PlayerAttackBehaviour { get; private set; }
            [field:SerializeField] public PlayerInteractionBehaviour PlayerInteractionBehaviour { get; private set; }
            [field:SerializeField] public PlayerAnimationBehaviour PlayerAnimationBehaviour { get; private set; }
            [field:SerializeField] public PlayerVisualBehaviour PlayerVisualBehaviour { get; private set; }
            [field:SerializeField] public PlayerOverheadUIBehaviour PlayerOverheadUIBehaviour { get; private set; }
            [field:SerializeField] public PlayerAbilityBehaviour PlayerAbilityBehaviour { get; private set; }
            
            [Header("UI")]
            [SerializeField] private PlayerHealthBar playerHealthBar;
            [SerializeField] private PauseMenu pauseMenu;
            
            [Header("Input Settings")]
            [SerializeField] private PlayerInput playerInput;
            [Tooltip("Effects How smooth the movement Interpolation is. Higher value is smoother movement. Lower value is more responsive movement.")]
            [SerializeField] private float movementSmoothingSpeed = 1f;
            private Vector3 rawInputMovement;
            private Vector3 smoothInputMovement;
            
            [Header("Invincibility")]
            [SerializeField] private float invincibilityTime;
            private float currentInvincibilityTime;
            
            // Health variables
            public int Health { get; set; }
            public bool Invincible { get; set; }
            
            [Header("Temporary damage animation")]
            [SerializeField] private MeshRenderer meshRenderer;
            [SerializeField] private Material defaultMaterial;
            [SerializeField] private Material damagedMaterial;
            
            [Header("Rumble Settings")]
            [SerializeField,Range(0,1)] private float lowFrequency;
            [SerializeField,Range(0,1)] private float highFrequency;
            [SerializeField] private float duration;
            
            [Header("Audio")] 
            [SerializeField] private DialogueAudio dialogueAudio;
            
            [Space]
            [Header("Debug")]
            [SerializeField] private bool debug;
            
            private Rigidbody rigidbody;

            public void SetupPlayer(InputDevice _inputDevice)
            {
                if (_inputDevice != null)
                {
                    playerInput.SwitchCurrentControlScheme(_inputDevice);
                }
                
                PlayerData.NewScene();
                pauseMenu = FindObjectOfType<PauseMenu>(true);
                PlayerIndex = PlayerData.playerIndex;
               
                Health = PlayerData.currentHealth;

                rigidbody = GetComponent<Rigidbody>();
                
                PlayerMovementBehaviour.SetupBehaviour(this);
                PlayerAnimationBehaviour.SetupBehaviour();
                //PlayerVisualBehaviour.SetupBehaviour(PlayerData);
                PlayerOverheadUIBehaviour.SetupBehaviour(this);
                if (PlayerAbilityBehaviour)
                {
                    PlayerAbilityBehaviour.SetupBehaviour(this);
                }
                
                playerHealthBar.SetupHealthBar(PlayerData.startingHealth, PlayerData.currentHealth);

            }

#region Unity Functions
            private void OnEnable()
            {
                playerInput.deviceRegainedEvent.Invoke(playerInput);
            }

            private void OnDisable()
            {
                playerInput.deviceLostEvent.Invoke(playerInput);
            }

            private void FixedUpdate()
            {
                CalculateMovementInputSmoothing();
                if (smoothInputMovement.magnitude < 0.01f)
                {
                    smoothInputMovement = Vector3.zero;
                }
                
                UpdatePlayerMovement();
                UpdatePlayerAnimationMovement();
            }

            private void Update()
            {
                if (currentInvincibilityTime > 0)
                {
                    currentInvincibilityTime -= Time.deltaTime;
                }
                else
                {
                    Invincible = false;
                }
            }
#endregion

#region Input System Actions // INPUT SYSTEM ACTION METHODS
            /// <summary>
            /// This is called from PlayerInput, corresponds with player movement.
            /// </summary>
            public void OnMovement(InputAction.CallbackContext value)
            { 
                Vector2 _inputValue = value.ReadValue<Vector2>();
                rawInputMovement = (new Vector3(_inputValue.x, 0, _inputValue.y));
            }

            /// <summary>
            /// This is called from PlayerInput, corresponds with player dash.
            /// </summary>
            public void OnDash(InputAction.CallbackContext value)
            {
                if (value.performed)
                {
                    PlayerMovementBehaviour.Dash();
                }
            }

            /// <summary>
            /// This is called from PlayerInput, corresponds with player attack.
            /// </summary>
            public void OnAttack(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    PlayerAttackBehaviour.Attack(true);
                }
                else if (value.canceled) 
                {
                    PlayerAttackBehaviour.Attack(false);
                }
            }

            /// <summary>
            /// This is called from PlayerInput, corresponds with player interact.
            /// </summary>
            public void OnInteract(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    PlayerInteractionBehaviour.OnInteract(true);
                }
                else if (value.canceled)
                {
                    PlayerInteractionBehaviour.OnInteract(false);
                }
            }

            /// <summary>
            /// This is called from PlayerInput, corresponds with player UI.
            /// </summary>
            public void OnPlayerUI(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    PlayerOverheadUIBehaviour.ToggleOverheadStatsUI(true);
                }
                else if (value.canceled)
                {
                    PlayerOverheadUIBehaviour.ToggleOverheadStatsUI(false);
                }
            }

            public void OnPause(InputAction.CallbackContext value)
            {
                pauseMenu.StartPauseGameplay(value.started,this);
            }

            public void OnSubmit(InputAction.CallbackContext value)
            {
                pauseMenu.UnPauseGameplay(value.started,this);
            }

            // Switching input action maps
            public void EnableEventControls()
            {
                playerInput.SwitchCurrentActionMap("Menu");
            }

            public void EnableGamePlayControls()
            {
                playerInput.SwitchCurrentActionMap("Player");
            }

            public void SetInputPausedState(bool _paused)
            {
                switch (_paused)
                {
                    case true:
                        playerInput.DeactivateInput();
                        break;
                    case false:
                        playerInput.ActivateInput();
                        break;
                }
            }
#endregion

#region Public Functions
            public void SetInvincibility(float _time)
            {
                Invincible = true;
                currentInvincibilityTime = _time;
            }
            
            public void Death()
            {
                playerHealthBar.UpdateHealthBar(0);
                PlayerInteractionBehaviour.OnDeath();
                GameManager.OnPlayerDeath.Invoke(PlayerIndex);
                
                Destroy(gameObject);
            }
            
            public void DamageTaken(Vector3 _damagePosition, float _knockbackForce)
            {
                // Knockback
                Vector3 _knockbackDirection = transform.position - _damagePosition;
                _knockbackDirection = new Vector3(_knockbackDirection.x, 0, _knockbackDirection.z).normalized;
                _knockbackDirection *= _knockbackForce;
                rigidbody.AddForce(_knockbackDirection);
                
                Invincible = true;
                currentInvincibilityTime = invincibilityTime;
                
                RumbleManager.Instance.RumblePulse(lowFrequency,highFrequency,duration);
                PlayerData.currentHealth = Health;
                playerHealthBar.UpdateHealthBar(Health);
                
                try
                {
                    dialogueAudio.PlayerDamageAudio(PlayerIndex);
                }
                catch (Exception e)
                {
                    Debug.LogError("[{PlayerController}]: Error Exception " + e);
                }
            }

            public bool Heal(int _amount)
            {
                if (Health == PlayerData.startingHealth)
                {
                    return false;
                }
                else
                {
                    Health += _amount;
                    Health = Mathf.Clamp(Health, 0, PlayerData.startingHealth);
                    PlayerData.currentHealth = Health;
                    playerHealthBar.UpdateHealthBar(Health);
                    return true;
                }
            }
#endregion

#region Private Functions
            private void CalculateMovementInputSmoothing()
            {
                smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
            }

            private void UpdatePlayerMovement()
            {
                PlayerMovementBehaviour.UpdateMovementData(IsoVectorConvert(smoothInputMovement));
            }

            private void UpdatePlayerAnimationMovement()
            {
                if (!PlayerMovementBehaviour.CanMove())
                {
                    PlayerAnimationBehaviour.UpdateMovementAnimation(0);
                }
                else
                {
                    PlayerAnimationBehaviour.UpdateMovementAnimation(smoothInputMovement.magnitude);
                }
            }

            private static Vector3 IsoVectorConvert(Vector3 _vector) {

                if (UnityEngine.Camera.main == null)
                    return _vector;
                
                Vector3 _cameraRot = UnityEngine.Camera.main.transform.rotation.eulerAngles;
                Quaternion _rotation = Quaternion.Euler(0, _cameraRot.y, 0);
                Matrix4x4 _isoMatrix = Matrix4x4.Rotate(_rotation);
                Vector3 _result = _isoMatrix.MultiplyPoint3x4(_vector);
                return _result;
            }
#endregion

#region Experimental code
            /*
            public Vector3 DownDir;
            public float RideSpringDamper;
            public float RideSpringStrength;
            public float RideHeight;

            private RaycastHit _rayHit;

            public bool _rayDidHit;

            public float SphereCheckNumber;
            public float raycastDistance;

            

            void OnRayHit()
            {
                DownDir = IsoVectorConvert(-transform.up);
                if (Physics.Raycast(transform.position, -transform.up, out _rayHit, raycastDistance))
                {
                    _rayDidHit = true;
                }
                else
                {
                    _rayDidHit = false;
                }
            }

            void Ray()
            {
                if (_rayDidHit)
                {
                    Vector3 vel = IsoVectorConvert(GetComponent<Rigidbody>().velocity);
                    Vector3 rayDir = IsoVectorConvert(transform.TransformDirection(DownDir));
                    Vector3 otherVel = Vector3.zero;
                    Rigidbody hitBody = _rayHit.rigidbody;
                    if (hitBody != null)
                    {
                        otherVel = hitBody.velocity;
                    }

                    float rayDirVel = Vector3.Dot(rayDir, vel);
                    float otherDirVel = Vector3.Dot(rayDir, otherVel);
                    float x = _rayHit.distance - RideHeight;
                    float relVel = rayDirVel - otherDirVel;
                    float springForce = (x * RideSpringStrength) - (relVel * RideSpringDamper);
                    Debug.DrawLine(transform.position, transform.position + (rayDir * springForce), Color.blue);
                    GetComponent<Rigidbody>().AddForce(IsoVectorConvert(rayDir * springForce));
                    if (hitBody != null)
                    {
                        hitBody.AddForceAtPosition(rayDir * -springForce, IsoVectorConvert(_rayHit.point));
                    }
                }
            }*/
#endregion

            private void Log(string msg) {
                if (!debug) return;
                Debug.Log("[GameManager]: " + msg);
            }

            private void LogWarning(string msg) {
                if (!debug) return;
                Debug.Log("[GameManager]: " + msg);
            }
        }
    }
}