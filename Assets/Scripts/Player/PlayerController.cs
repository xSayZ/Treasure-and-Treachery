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
            
            public PlayerData PlayerData;
            public int PlayerID { get; private set; }
            
            [Header("SubBehaviours")] 
            [SerializeField]
            private PlayerMovementBehaviour playerMovementBehaviour;
            [SerializeField] private AttackBehaviour playerAttackBehaviour;
            
            [Header("InputSettings")]
            [SerializeField] private PlayerInput playerInput;
            
            #region Unity Functions
  
            private void Awake()
            {
                PlayerData.playerIndex.Clear();
                PlayerData.currency.Clear();
                
                EventManager.OnHealthChange.AddListener(BeginHealthChange);
                
            }
            
            void Start()
            {
                
                SetupPlayer();
                EventManager.OnCurrencyPickup.AddListener(BeginCurrencyPickup);
               

                
            }

            
            private void BeginHealthChange(int Health, int _playerID)
            {
                if (Health <= 0 && _playerID == PlayerID)
                {
                    gameObject.SetActive(true);
                }
            }
            
            
            // Update is called once per frame
            void Update()
            {
                
            }

            private void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.layer == 8)
                {
                    PlayerData.CurrentHealth--;
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
                
                PlayerData.playerIndex.Add(PlayerID);
                PlayerData.currency.Add(0);

            }
            
            private void BeginCurrencyPickup(int pickUpGold,int _playerId)
            {
                for (int i = 0; i < PlayerData.playerIndex.Count; i++)
                {
                    if (i== PlayerID && PlayerID == _playerId)
                    {
                        PlayerData.currency[_playerId] += pickUpGold;
                    }
                }
                
            }
            
            #endregion
        }

        

    }
}