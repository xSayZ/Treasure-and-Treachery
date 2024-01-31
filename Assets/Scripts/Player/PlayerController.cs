// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/


using System;
using System.Linq;
using Game.Backend;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


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
  
            private void Awake()
            {
                
            }

            private void OnEnable()
            {
                PlayerInput.actions["SwitchMap"].performed += SwitchActionmap;
            }
            
            private void OnDisable()
            {
                
                PlayerInput.actions["SwitchMap"].performed += SwitchActionmap;
            }

            private void SwitchActionmap(InputAction.CallbackContext context)
            {
                PlayerInput.actions.FindAction("Events").Enable();
            }

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
            void Update()
            {
                
                   
            }

          

            private void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.layer == 8)
                {
                }
            }

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
                    
                    playerAttackBehaviour.RangedAttack(playerMovementBehaviour.SmoothMovementDirection.normalized);
                    Debug.Log(playerMovementBehaviour.SmoothMovementDirection.normalized);
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
            
            #endregion

            #region Private Functions
            private void SetupPlayer()
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
            

            #endregion
        }
        

        

    }
}