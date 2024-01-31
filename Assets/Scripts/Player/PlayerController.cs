// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using Game.Backend;
using Game.Events;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game
{
    namespace Player
    {
        public class PlayerController : MonoBehaviour
        {
            [Header("PlayerInfo")]
            public PlayerData PlayerData;
            [field:SerializeField]public int PlayerID { get; private set; }
            //temp Health Solution
            public int Health;
            public int Currency;
            [Header("SubBehaviours")] 
            [SerializeField]
            private PlayerMovementBehaviour playerMovementBehaviour;
            [SerializeField] private AttackBehaviour playerAttackBehaviour;
            
            [Header("InputSettings")]
            [SerializeField] private PlayerInput playerInput;
            
            #region Unity Functions
  
            private void Awake()
            {
                
                
            }
            
            void Start()
            {
                
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
                Currency = PlayerData.currency;
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
                
                PlayerID = playerInput.playerIndex;

                if (playerInput.playerIndex !=0 && playerInput.currentControlScheme !="Player1")
                {
                    gameObject.SetActive(false);
                    
                }
                playerInput.SwitchCurrentControlScheme(Keyboard.current);

                PlayerData.playerIndex = PlayerID;
                


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