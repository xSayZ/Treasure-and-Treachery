// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/


using Game.Backend;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game
{
    namespace Player
    {
        using Events;
        using Scenes;
        public class PlayerController : MonoBehaviour
        {
            
            public PlayerData PlayerData;
            public int PlayerID { get; private set; }

            public static int CurrentAmountOfControllers;

            public bool isEvent;
            //temp Health Solution
            public int Health;

            public SceneData SceneData;
            [Header("SubBehaviours")] 
            [SerializeField]
            private PlayerMovementBehaviour playerMovementBehaviour;
            [SerializeField] private AttackBehaviour playerAttackBehaviour;
            
            [Header("InputSettings")]
            [SerializeField] private PlayerInput PlayerInput;
            //ActionMaps
            private string MenuActions = "Events";
            private string PlayerAction = "Player";
            
            #region Unity Functions
            
            
            void Start()
            {
                CurrentAmountOfControllers = Gamepad.all.Count;
                SetupPlayer();
                SetStartHealth();
                EventManager.OnCurrencyPickup.AddListener(BeginCurrencyPickup);
                
            }
            
            void SetStartHealth()
            {

                Health = PlayerData.playerHealth;

            }
            
            // Update is called once per frame
       
            #endregion

            #region Public Functions

            public void OnMovement(InputAction.CallbackContext value)
            {
                
                    Vector2 _inputValue = value.ReadValue<Vector2>();
                    Vector3 _rawInputMovement = (new Vector3(_inputValue.x, 0, _inputValue.y));
                    playerMovementBehaviour.MovementData(_rawInputMovement);
                
               
            }

            public void OnRanged(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    //TODO: ADD MovementData = 0,0,0
                    //TODO;; PlayAttackAnimation
                    playerAttackBehaviour.RangedAttack(playerMovementBehaviour.SmoothMovementDirection);
                   
                }
            }

            public void OnSubmit(InputAction.CallbackContext value)
            {
                Debug.Log(value.ReadValueAsButton());
            }
            
            public void OnMelee(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    //TODO:: AttackAnimation
                    Debug.Log(value);
                }  
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

            private void Update()
            {
               
                Death();
            }

            
            #region Private Functions
            public void SetupPlayer()
            {
                
                PlayerID = PlayerInput.playerIndex;

                if (PlayerInput.playerIndex !=0 && PlayerInput.currentControlScheme !="Player1")
                {
                    gameObject.SetActive(false);
                    
                }
                PlayerInput.SwitchCurrentControlScheme(Keyboard.current);
            }
            
            private void BeginCurrencyPickup(int pickUpGold,int _playerId)
            {
                    if (PlayerID == _playerId)
                    {
                        PlayerData.currency += pickUpGold;
                    }
                
            }

            private void Death()
            {
                if (Health <= 0)
                {
                    // TODO: Forward to animationBehaviour
                    gameObject.SetActive(false);
                }
            }

            #endregion
        }
        

        

    }
}