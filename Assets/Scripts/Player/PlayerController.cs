// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Script for handling the player
// --------------------------------
// ------------------------------*/

using System;
using System.Security;
using System.Threading.Tasks;
using Game.Backend;
using Game.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Audio;
using Game.NAME;
using UnityEngine.InputSystem.Users;


namespace Game
{
    namespace Player
    {
        public class PlayerController : MonoBehaviour, IDamageable
        {
            [Header("Player Data")]
            [Tooltip("Player Data Scriptable Object")]
            [SerializeField] public PlayerData PlayerData;
            [HideInInspector] public int PlayerIndex;

            
            [Header("Sub Behaviours")]
            [Tooltip("Assign sub behaviours for player")]
            [SerializeField] private PlayerMovementBehaviour playerMovementBehaviour;
            [SerializeField] private PlayerAttackBehaviour playerAttackBehaviour;
            [SerializeField] private PlayerInteractionBehaviour playerInteractionBehaviour;
            [SerializeField] public PlayerAnimationBehaviour playerAnimationBehaviour;
            [SerializeField] private PlayerVisualBehaviour playerVisualBehaviour;
            [SerializeField] private PlayerUIDisplayBehaviours playerUIDisplayBehaviours;

            [Header("UI")]
            [SerializeField] private PlayerHealthBar playerHealthBar;
            
            [Header("Input Settings")]
            [SerializeField] private PlayerInput playerInput;
            [Tooltip("Effects How smooth the movement Interpolation is. Higher value is smoother movement. Lower value is more responsive movement.")]
            [SerializeField] public float movementSmoothingSpeed = 1f;
            private Vector3 rawInputMovement;
            private Vector3 smoothInputMovement;
            
            [Header("Audio")]
            [SerializeField] private GameObject playerObj;
            [SerializeField] private PlayerAudio playerAudio;
            [SerializeField] private DialogueAudio dialogueAudio;

                [field: HideInInspector] public int Health { get; set; }

            [Header("Temporary damage animation")]
            [SerializeField] private MeshRenderer meshRenderer;
            [SerializeField] private Material defaultMaterial;
            [SerializeField] private Material damagedMaterial;

            [Header("Rumble Settings")]
            [SerializeField,Range(0,1)] private float lowFrequency;
            [SerializeField,Range(0,1)] private float highFrequency;
            [SerializeField] private float duration;

            [Space]
            [Header("Debug")]
            [SerializeField] private bool debug;
            
            public void SetupPlayer(int _newPlayerID)
            {
                PlayerData.playerIndex = _newPlayerID;
                PlayerIndex = _newPlayerID;
               
                Health = PlayerData.currentHealth;
                
                PlayerData.NewScene();
                
                playerInput.SwitchCurrentControlScheme(Keyboard.current);
                
                playerMovementBehaviour.SetupBehaviour();
                playerAnimationBehaviour.SetupBehaviour();
                playerVisualBehaviour.SetupBehaviour(PlayerData);
                playerUIDisplayBehaviours.SetupBehaviour(this);
                
                playerHealthBar.SetupHealthBar(PlayerData.startingHealth, PlayerData.currentHealth);

                
                var player = PlayerInput.all[_newPlayerID];
                InputUser.PerformPairingWithDevice(Gamepad.all[PlayerIndex],user:player.user);
                
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

            void FixedUpdate()
            {
                CalculateMovementInputSmoothing();
                if (smoothInputMovement.magnitude < 0.01f) {
                    smoothInputMovement = Vector3.zero;
                }
                
                UpdatePlayerMovement();
                UpdatePlayerAnimationMovement();
            }
            #endregion

            public void OnPlayerJoined()
            {
                
            }
#region Input System Actions // INPUT SYSTEM ACTION METHODS
            
            /// <summary>
            /// This is called from PlayerInput; when a joystick or arrow keys has been pushed.
            /// It stores the input Vector as a Vector3 to then be used by the smoothing function.
            /// </summary>
            /// <param name="value"></param>
            public void OnMovement(InputAction.CallbackContext value)
            { 
                Vector2 _inputValue = value.ReadValue<Vector2>();
                rawInputMovement = (new Vector3(_inputValue.x, 0, _inputValue.y));
            }
            /// <summary>
            /// This is called from PlayerInput, when a button has been pushed, that is corresponds with the 'Dash' action.
            /// </summary>
            /// <param name="value"></param>
            public void OnDash(InputAction.CallbackContext value)
            {
                if (value.performed && playerMovementBehaviour.currentDashCooldown <= 0)
                {
                    playerMovementBehaviour.Dash(value.performed);
                }
            }
            /// <summary>
            /// This is called from PlayerInput, when a button has been pushed, that is corresponds with the 'Ranged' action.
            /// </summary>
            /// <param name="value"></param>
            public void OnRanged(InputAction.CallbackContext value)
            {
                if (PlayerHasNoCurrentItem() && playerAttackBehaviour.currentFireRate <= 0) {
                    //TODO: make Character chargeUp
                    if(value.started)
                    {
                        // Aiming
                        playerMovementBehaviour.SetMovementActiveState(false, true);
                        playerMovementBehaviour.TurnSpeed /= 2;

                    } else if (value.canceled) {
                        // Shooting
                        playerAttackBehaviour.RangedAttack();
                        playerMovementBehaviour.TurnSpeed *= 2;
                        //playerAudio.RangedAudioPlay(playerObj);
                    }
                }
            }
            /// <summary>
            /// This is called from PlayerInput, when a button has been pushed, that is corresponds with the 'Melee' action.
            /// </summary>
            /// <param name="value"></param>
            public void OnMelee(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    if (PlayerHasNoCurrentItem()) {
                        
                        if (playerAttackBehaviour.MeleeAttack()) {
                            playerAnimationBehaviour.PlayMeleeAttackAnimation();
                        }
                        try
                        {
                            playerAudio.MeleeAudioPlay(playerObj);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("[{PlayerController}]: Error Exception " + e);
                        }
                        
                    }
                }
            }
            /// <summary>
            /// This is called from PlayerInput, when a button has been pushed, that is corresponds with the 'Interact' action.
            /// </summary>
            /// <param name="value"></param>
            public void OnInteract(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    playerInteractionBehaviour.OnInteract(true);
                }
                else if (value.canceled)
                {
                    playerInteractionBehaviour.OnInteract(false);
                }
            }
            
            /// <summary>
            /// This is called from PlayerInput, when a button has been pushed, that is corresponds with the 'TogglePause' action.
            /// </summary>
            /// <param name="value"></param>
            public void OnTogglePause(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    // Remove after pause has been implemented
                    return;
                    // GameManager.Instance.TogglePauseState(this);
                }
            }
            
            public void OnTogglePlayerUI(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    playerUIDisplayBehaviours.TogglePlayerUIElements(true);
                }
                else if (value.canceled) {
                    playerUIDisplayBehaviours.TogglePlayerUIElements(false);
                }
            }
            
            // SWITCHING INPUT ACTION MAPS
            public void EnableEventControls()
            {
                playerInput.SwitchCurrentActionMap("Events");
            }

            public void EnableGamePlayControls()
            {
                playerInput.SwitchCurrentActionMap("Players");
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
            
            public void Death()
            {
                playerInteractionBehaviour.OnDeath();
                playerHealthBar.UpdateHealthBar(Health);
                GameManager.OnPlayerDeath.Invoke(PlayerIndex);
                Destroy(gameObject);
            }
            
            public void DamageTaken()
            {
                // FlashRed();
                //Log("Player " + PlayerIndex + " took damage");
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
 #endregion

#region Private Functions
            private void CalculateMovementInputSmoothing()
            {
                smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
            }

            private void UpdatePlayerMovement()
            {
                playerMovementBehaviour.UpdateMovementData(IsoVectorConvert(smoothInputMovement));
            }

            private void UpdatePlayerAnimationMovement()
            {
                playerAnimationBehaviour.UpdateMovementAnimation(smoothInputMovement.magnitude);
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
            
            // Temporary damage animation
            private async void FlashRed()
            {
                meshRenderer.material = damagedMaterial;
                await Task.Delay(100);
                meshRenderer.material = defaultMaterial;
                await Task.Delay(100);
                meshRenderer.material = damagedMaterial;
                await Task.Delay(100);
                meshRenderer.material = defaultMaterial;
                await Task.Delay(100);
                meshRenderer.material = damagedMaterial;
                await Task.Delay(100);
                meshRenderer.material = defaultMaterial;
            }

            private bool PlayerHasNoCurrentItem() {
                return PlayerData.currentItem == null;
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