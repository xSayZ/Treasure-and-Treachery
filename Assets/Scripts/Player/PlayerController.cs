// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/



using System.Collections;
using Game.Backend;
using Game.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using Game.Audio;


namespace Game
{
    namespace Player
    {
        using Quest;
        using Scenes;

        public enum Archetype
        {
            Melee,
            Ranged,
            Both,
        }

        public class PlayerController : MonoBehaviour,IDamageable
        {
            
            public PlayerData PlayerData;
            private int playerID;
            
            //temp Health Solution
            [Header("SubBehaviours")] 
            [SerializeField]
            private PlayerMovementBehaviour playerMovementBehaviour;
            [SerializeField] private AttackBehaviour playerAttackBehaviour;
            
            [Header("InputSettings")]
            [SerializeField] private PlayerInput PlayerInput;
            
            // stashed values will lose on death
            private int currency;
            private int questValue;

            public Archetype CharacterType;
            #region Unity Functions
            

            [Header("Dash Data")]
            [Tooltip("How much current displacement should increase with")]
            public float DashModifier;
            public float  dashTime;
            public float BaseDashCoolDown;

            private bool dashing;
            private Vector3 _rawInputMovement;
            private float currentDashCooldown;

            [Header("Audio")] 
            [SerializeField] private GameObject playerObj;
            [SerializeField] private PlayerAudio playerAudio;
            
            
            [Header("Test Stuff")] 
            public Material _material;
            public float angle;
            void Start()
            {
                SetupPlayer();
                QuestManager.OnGoldPickedUp.AddListener(BeginCurrencyPickup);
                currentDashCooldown = 0;

            }
            private void Update()
            {
               Death();
            }

            [field:SerializeField]public int Health { get; set; }

            public void Death()
            {
                if (Health <=0)
                {
                    
                    Destroy(gameObject);
                }
            }
            
            //Temp animation
            IEnumerator FlashRed()
            {
                _material.color = Color.red;
                yield return new WaitForSeconds(1f);

                _material.color = Color.white;
                
            }
            
            public void DamageTaken()
            {
                StartCoroutine(FlashRed());
            }

            

            #endregion

            #region Public Functions

            public void OnMovement(InputAction.CallbackContext value)
            {
                // Dashing will lock character from moving direction during the duration 
                if (!dashing)
                {
                    if (CharacterType == Archetype.Melee || CharacterType == Archetype.Both)
                    {
                        Vector2 _inputValue = value.ReadValue<Vector2>();
                        _rawInputMovement = (new Vector3(_inputValue.x, 0, _inputValue.y));
                        
                        playerMovementBehaviour.MovementData(IsoVectorConvert(_rawInputMovement));
                    }
                }

                if (playerAttackBehaviour.isAttacking)
                {
                    Debug.Log("test");
                }
            }


            
            public void OnDash(InputAction.CallbackContext value)
            {
                
                if (value.started && dashing == false  && BaseDashCoolDown <=0)
                {
                    //Todo:: PlayDustCloud Particle if needed
                    
                    playerMovementBehaviour.MovementData(IsoVectorConvert(_rawInputMovement*DashModifier));
                    StartCoroutine(WaitUntilDashComplete());
                    dashing = true;
                    currentDashCooldown = BaseDashCoolDown;
                }
            }
            
            public void OnRanged(InputAction.CallbackContext value)
            {
                if (value.action.triggered)
                {
                    //TODO: ADD MovementData = 0,0,0
                    playerMovementBehaviour.MovementData(Vector3.zero);
                    //TODO;; PlayAttackAnimation
                    if (CharacterType == Archetype.Ranged || CharacterType == Archetype.Both)
                    {
                        playerAttackBehaviour.RangedAttack();
                        //playerAudio.PlayerRangedAudio(playerObj);
                    }
                   
                }
            }
            
            public void OnMelee(InputAction.CallbackContext value)
            {
                playerMovementBehaviour.MovementData(Vector3.zero);
                if (value.action.triggered)
                {
                    playerAttackBehaviour.MeleeAttack();
                    playerAudio.MeleeAudioPlay(playerObj);
                }  
            }


            public void OnSubmit(InputAction.CallbackContext value)
            {
                Debug.Log(value.ReadValueAsButton());
            }
            
     
          
            public void EnableEventControls()
            {
                PlayerInput.SwitchCurrentActionMap("Events");
            }

            public void EnableGamePlayControls()
            {
                PlayerInput.SwitchCurrentActionMap("Players");
            }


            public void OnTogglePause(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    GameManager.Instance.TogglePauseState(this);
                    
                }
                
            }
            
                 
            public void SetInputActiveState(bool gameIsPaused) {
                switch (gameIsPaused)
                {
                    case true:
                        PlayerInput.DeactivateInput();
                        break;

                    case false:
                        PlayerInput.ActivateInput();
                        break;
                }
            }

            
            #endregion


            
            #region Private Functions
            private void SetupPlayer()
            {
                
                playerID = PlayerInput.playerIndex;
                
                Health = PlayerData.playerHealth;
                
                if (PlayerInput.playerIndex !=0 && PlayerInput.currentControlScheme !="Player1")
                {
                    Destroy(gameObject);
                    
                }
                PlayerInput.SwitchCurrentControlScheme(Keyboard.current);
            }
            
            private void BeginCurrencyPickup(int _playerId,int pickUpGold)
            {
                    if (playerID == _playerId)
                    {
                        PlayerData.currency += pickUpGold;
                    }
                
            }
            
            
            
            //TODO: Move Dash to playerMovement
            private IEnumerator WaitUntilDashComplete()
            {
                yield return new WaitForSeconds(dashTime);
                playerMovementBehaviour.MovementData(IsoVectorConvert(_rawInputMovement));
                dashing = false;
            }
            
            private Vector3 IsoVectorConvert(Vector3 vector)
            {
                Vector3 cameraRot = UnityEngine.Camera.main.transform.rotation.eulerAngles;
                Quaternion rotation = Quaternion.Euler(0,cameraRot.y, 0);
                Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
                Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
                return result;

            }
            #endregion
        }
        

        

    }
}