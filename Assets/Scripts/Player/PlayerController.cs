// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Script for handling the player
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Backend;
using Game.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Audio;
using Game.Quest;
using UnityEngine.UI;


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
            [SerializeField] public PlayerData playerData;
            private int playerID;

            
            [Header("Sub Behaviours")]
            [Tooltip("Assign sub behaviours for player")]
            [SerializeField] private PlayerMovementBehaviour playerMovementBehaviour;
            [SerializeField] private AttackBehaviour playerAttackBehaviour;
            // [SerializeField] private PlayerAnimationBehaviour playerAnimationBehaviour;
            // [SerializeField] private PlayerInteractionBehaviour playerInteractionBehaviour;
            
            [Header("Input Settings")]
            [SerializeField] private PlayerInput playerInput;
            
            [SerializeField] private Archetype characterType;

            // Interactables
            private List<IInteractable> inInteractRange;
            // Movement
            private Vector3 _rawInputMovement;

            [Header("UI")]
            [SerializeField] private GameObject itemImage;
            
            [Header("Audio")]
            [SerializeField] private GameObject playerObj;
            [SerializeField] private PlayerAudio playerAudio;
            
            [field: SerializeField] public int Health { get; set; }

            [Header("Test Stuff")]
            public Material _material;
            public bool WalkOnGraves;
            
            [Space]
            [Header("Debug")]
            [SerializeField] private bool debug;
            
            #region Unity Functions

            private void OnEnable()
            {
                // Subscribe to events
                QuestManager.OnItemPickedUp.AddListener(PickUpItem);
                QuestManager.OnItemDropped.AddListener(DropItem);
                QuestManager.OnGoldPickedUp.AddListener(PickUpGold);
            }
            
            private void OnDisable()
            {
                // Unsubscribe from events
                QuestManager.OnItemPickedUp.AddListener(PickUpItem);
                QuestManager.OnItemDropped.AddListener(DropItem);
                QuestManager.OnGoldPickedUp.AddListener(PickUpGold);
            }
            
            void Start()
            {
                SetupPlayer();
                inInteractRange = new List<IInteractable>();
            }
            
            public void Death()
            {
                if (playerData.currentItem != null)
                {
                    DropItem(playerID, playerData.currentItem, false);
                }
                
                for (int i = 0; i < inInteractRange.Count; i++) // Dose not remove interact Ui for item dropped on death for some reason
                {
                    Debug.Log(i);
                    inInteractRange[i].InInteractionRange(playerID, false);
                }
                
                Destroy(gameObject);
            }

            //Temp animation
            private async void FlashRed()
            {
                _material.color = Color.red;
                await Task.Delay(1000);

                _material.color = Color.white;
            }

            public void DamageTaken()
            {
                FlashRed();
                Log("Player " + playerID + " took damage");
                playerData.currentHealth = Health;
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
                
                _rawInputMovement = (new Vector3(_inputValue.x, 0, _inputValue.y));

                playerMovementBehaviour.MovementData(IsoVectorConvert(_rawInputMovement));
            }
            public void OnDash(InputAction.CallbackContext value)
            {
                if (value.action.WasPressedThisFrame() && playerMovementBehaviour.dashCooldown <= 0)
                {
                    //Todo: PlayDustCloud Particle if needed
                    playerMovementBehaviour.Dash(value.action.WasPressedThisFrame());
                }
            }

            public void OnRanged(InputAction.CallbackContext value)
            {
                //TODO: make Character chargeUp
                if (value.action.triggered)
                {
                    //TODO: PlayAttackAnimation
                    if (characterType == Archetype.Ranged || characterType == Archetype.Both)
                    {
                        if (playerData.currentItem != null)
                        {
                            return;
                        }
                        
                        playerAttackBehaviour.RangedAttack();
                        //playerAudio.PlayerRangedAudio(playerObj);
                    }
                }
            }

            public void OnMelee(InputAction.CallbackContext value)
            {
                if (value.action.triggered)
                {
                    if (playerData.currentItem != null)
                        return;
                    
                    playerMovementBehaviour.TurnPlayer();
                    playerAttackBehaviour.MeleeAttack();
                    playerAudio.MeleeAudioPlay(playerObj);
                }
            }
            public void OnInteract(InputAction.CallbackContext value)
            {
                if (value is { started: true, performed: false }) // Needed to stop interaction form triggering twice when pressing button
                    return;
                
                // Null check
                for (int i = inInteractRange.Count - 1; i >= 0; i--)
                {
                    if (!(inInteractRange[i] as Object)) // Fancy null check because a normal null check dose not work for some reason
                    {
                        inInteractRange.Remove(inInteractRange[i]);
                    }
                }
                
                // Drop item
                if (inInteractRange.Count <= 0 && playerData.currentItem != null && value.performed)
                {
                    Log("Plyer" + playerID + "Dropped item");
                    DropItem(playerID, playerData.currentItem, false);
                }
                
                // Interact with stuff
                for (int i = 0; i < inInteractRange.Count; i++)
                {
                    inInteractRange[i].Interact(playerID, value.performed);
                }
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
                    GameManager.Instance.TogglePauseState(this);
                }
            }
            
            public void SetInputActiveState(bool gameIsPaused)
            {
                switch (gameIsPaused)
                {
                    case true:
                        playerInput.DeactivateInput();
                        break;
                    case false:
                        playerInput.ActivateInput();
                        break;
                }
            }
            public void InteractRangeEntered(Transform _transform) {
                if (!_transform.TryGetComponent(out IInteractable _interactable))
                    return;
                
                inInteractRange.Add(_interactable);
                _interactable.InInteractionRange(playerID, true);
            }
            public void InteractRangeExited(Transform _transform)
            {
                if (_transform.TryGetComponent(out IInteractable _interactable))
                {
                    inInteractRange.Remove(_interactable);
                    _interactable.InInteractionRange(playerID, false);
                }
            }
            #endregion

            #region Private Functions
            
            private void SetupPlayer()
            {
                playerID = playerInput.playerIndex;
                Health = playerData.startingHealth;

                // * Marked for delete
                if (playerInput.playerIndex != 0 && playerInput.currentControlScheme != "Player1")
                {
                    Destroy(gameObject);
                }

                playerInput.SwitchCurrentControlScheme(Keyboard.current);
            }
            
            private void PickUpItem(int _playerId, Item _item) {
                if (playerID != _playerId)
                    return;
                
                if (playerData.currentItem != null)
                {
                    DropItem(_playerId, playerData.currentItem, false);
                }
                    
                _item.Pickup.SetActive(false);
                InteractRangeExited(_item.Pickup.transform);
                playerData.currentItem = _item;

                itemImage.GetComponent<Image>().sprite = _item.Sprite;
                itemImage.SetActive(true);
            }
            
            private void DropItem(int _playerId, Item _item, bool _destroy) {
                if (playerID != _playerId)
                    return;
                
                if (playerData.currentItem == _item)
                {
                    playerData.currentItem = null;
                    itemImage.SetActive(false);

                    if (_destroy)
                        return;
                    
                    _item.Pickup.SetActive(true);
                    _item.Pickup.transform.position = transform.position;
                }
                else
                {
                    LogWarning("Cant remove item from player inventory since player does not have that item in their inventory"); 
                }
            }
            
            private void PickUpGold(int _playerId, int pickUpGold)
            {
                if (playerID == _playerId)
                {
                    playerData.currency += pickUpGold;
                }
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
                Debug.Log("[GameManager]: "+msg);
            }

            private void LogWarning(string msg) {
                if (!debug) return;
                Debug.Log("[GameManager]: "+msg);
            }
        }
    }
}