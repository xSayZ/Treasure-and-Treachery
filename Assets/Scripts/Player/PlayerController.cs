// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Script for handling the player
// --------------------------------
// ------------------------------*/

using System.Threading.Tasks;
using Game.Backend;
using Game.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Audio;


namespace Game
{
    namespace Player
    {
        public enum Archetype
        {
            Melee,
            Ranged,
            Both,
        }

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
            // [SerializeField] private PlayerAnimationBehaviour playerAnimationBehaviour;

            [Header("UI")]
            [SerializeField] private PlayerHealthBar playerHealthBar;
            
            [Header("Input Settings")]
            [SerializeField] private PlayerInput playerInput;
            
            [SerializeField] private Archetype characterType;
            
            // Movement
            private Vector3 rawInputMovement;
            
            [Header("Audio")]
            [SerializeField] private GameObject playerObj;
            [SerializeField] private PlayerAudio playerAudio;
            
            [field: SerializeField] public int Health { get; set; }

            [Header("Temporary damage animation")]
            [SerializeField] private MeshRenderer meshRenderer;
            [SerializeField] private Material defaultMaterial;
            [SerializeField] private Material damagedMaterial;
            // public bool WalkOnGraves;
            
            [Space]
            [Header("Debug")]
            [SerializeField] private bool debug;
            
#region Unity Functions
            private void OnEnable()
            {
                playerInput.deviceRegainedEvent.Invoke(playerInput);
            }
            
            private void OnDisable()
            {
                playerInput.deviceLostEvent.Invoke(playerInput);
            }
            
            void Start()
            {
                playerHealthBar.SetupHealthBar(PlayerData.startingHealth);
                SetupPlayer();
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
            
            public void Death()
            {
                playerInteractionBehaviour.OnDeath();
                
                playerHealthBar.UpdateHealthBar(Health);
                
                GameManager.OnPlayerDeath.Invoke(PlayerIndex);
                
                Destroy(gameObject);
            }
            
            public void DamageTaken()
            {
                FlashRed();
                Log("Player " + PlayerIndex + " took damage");
                PlayerData.currentHealth = Health;
                playerHealthBar.UpdateHealthBar(Health);
            }
#endregion

#region Public Functions
            
            public void OnMovement(InputAction.CallbackContext value)
            { 
                // TODO: PlayFootStepAudio
                // TODO: PlayFootStepParticle
                // TODO: PlayFootStepAnimation
                
                // Get current input value
                Vector2 _inputValue = value.ReadValue<Vector2>();
                
                rawInputMovement = (new Vector3(_inputValue.x, 0, _inputValue.y));

                playerMovementBehaviour.MovementData(IsoVectorConvert(rawInputMovement));
            }
            
            public void OnDash(InputAction.CallbackContext value)
            {
                if (value.performed && playerMovementBehaviour.currentDashCooldown <= 0)
                {
                    //Todo: PlayDustCloud Particle if needed
                    playerMovementBehaviour.Dash(value.performed);
                }
            }

            public void OnRanged(InputAction.CallbackContext value)
            {
                if (PlayerData.currentItem != null)
                    return;
                
                if (playerAttackBehaviour.currentFireRate >= 0)
                    return;
                
                //TODO: make Character chargeUp
                if(value.started)
                {
                    // Aiming
                    playerMovementBehaviour.SetMovementActiveState(false, true);
                    playerMovementBehaviour.TurnSpeed /= 2;
                    // TODO: Aim UI
                    // TODO: Aim Sound

                } else if (value.canceled) {
                    // Shooting
                    playerAttackBehaviour.RangedAttack();
                    playerMovementBehaviour.TurnSpeed *= 2;
                    //playerAudio.RangedAudioPlay(playerObj);
                }
                    
                
                
                /*
                    //TODO: PlayAttackAnimation
                    if (characterType == Archetype.Ranged || characterType == Archetype.Both)
                    {
                        if (PlayerData.currentItem != null)
                        {
                            return;
                        }
                        
                        playerAttackBehaviour.RangedAttack();
                        //playerAudio.PlayerRangedAudio(playerObj);
                    }*/
            }

            public void OnMelee(InputAction.CallbackContext value)
            {
                if (value.action.triggered)
                {
                    if (PlayerData.currentItem != null)
                        return;
                    
                    playerMovementBehaviour.TurnPlayer();
                    playerAttackBehaviour.MeleeAttack();
                    playerAudio.MeleeAudioPlay(playerObj);
                }
            }
            
            public void OnInteract(InputAction.CallbackContext value)
            {
                if (value.started && !value.performed) // Needed to stop interaction form triggering twice when pressing button
                {
                    return;
                }
                
                playerInteractionBehaviour.OnInteract(value.performed);
            }
            
            public void EnableEventControls()
            {
                playerInput.SwitchCurrentActionMap("Events");
            }

            public void EnableGamePlayControls()
            {
                playerInput.SwitchCurrentActionMap("Players");
            }

            public void OnTogglePause(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    // Remove after pause has been implemented
                    return;
                    GameManager.Instance.TogglePauseState(this);
                }
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

#region Private Functions
            private void SetupPlayer()
            {
                PlayerIndex = PlayerData.SetPlayerData(Health);
                
                playerInput.SwitchCurrentControlScheme(Keyboard.current);
            }
            
            private static Vector3 IsoVectorConvert(Vector3 vector) {

                if (UnityEngine.Camera.main == null)
                    return vector;
                
                Vector3 _cameraRot = UnityEngine.Camera.main.transform.rotation.eulerAngles;
                Quaternion _rotation = Quaternion.Euler(0, _cameraRot.y, 0);
                Matrix4x4 _isoMatrix = Matrix4x4.Rotate(_rotation);
                Vector3 _result = _isoMatrix.MultiplyPoint3x4(vector);
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