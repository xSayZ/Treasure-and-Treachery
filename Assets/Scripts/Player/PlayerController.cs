// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using Cinemachine;
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
            public int playerID { get; private set; }
            
            [Header("SubBehaviours")] [SerializeField]
            private PlayerMovementBehaviour playerMovementBehaviour;

            [SerializeField] private AttackBehaviour playerAttackBehaviour;
            
            [Header("InputSettings")]
         
            [SerializeField] private PlayerInput playerInput;
            [Tooltip("Effects How effective turning is and inertial movement"),Range(0.1f, 10)]
            public float MovementSmoothing;

            
            [Header("Other Settings")]
            [Tooltip("How long before able to Moveagain after shooting")]
            public float BaseLockoutTimer;
            private float currentLockoutTimer= 0;
            private Vector3 rawInputMovement;
            private Vector3 smoothInputMovement;
            private Vector3 velocity;
            private bool lockout;

            [field:SerializeField] public float Currency { get; private set; }


            #region Unity Functions
       
            // Start is called before the first frame update

            
          
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
                if (Health <= 0 && _playerID == playerID)
                {
                    gameObject.SetActive(true);
                }
            }


            private void BeginCurrencyPickup(int pickUpGold,int _playerId)
            {
                for (int i = 0; i < PlayerData.playerIndex.Count; i++)
                {
                    if (i== playerID && playerID == _playerId)
                    {
                        PlayerData.currency[_playerId] += pickUpGold;
                    }
                }
                
            }

            private void OnValidate()
            {
                if (MovementSmoothing == 0)
                {
                    MovementSmoothing = 0.1f;
                    Debug.LogWarning("Smoothing Movement Must be higher than 0.1f");
                }
            }
            
            // Update is called once per frame
            void Update()
            {
                SmoothInputMovement();
                UpdatePlayerMovement();
                
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
                
                    Vector2 inputValue = value.ReadValue<Vector2>();
                    rawInputMovement = (new Vector3(inputValue.x, 0, inputValue.y));
                
               
            }
            
            
            

            public void OnRanged(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    //TODO: ADD MovementData = 0,0,0
                    //TODO;; PlayAttackAnimation
                    
                    playerAttackBehaviour.RangedAttack(playerMovementBehaviour.direction.normalized);
                    Debug.Log(playerMovementBehaviour.direction.normalized);
                    Lockout();
                }
            }
            
            public void OnMelee(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    rawInputMovement = Vector3.zero;
                    //TODO:: AttackAnimation
                    Debug.Log(value);
                }   
                
            }

            private void Lockout()
            {
                lockout = true;
                currentLockoutTimer = BaseLockoutTimer;
                if (lockout)
                {
                    currentLockoutTimer -= Time.deltaTime;
                }

                if (currentLockoutTimer <= 0)
                {
                    lockout = false;
                }
            }
            
            
            #endregion

            #region Private Functions
            private void SetupPlayer()
            {
                
                playerID = playerInput.playerIndex;

                if (playerInput.playerIndex !=0 && playerInput.currentControlScheme !="Player1")
                {
                    gameObject.SetActive(false);
                    
                }
                playerInput.SwitchCurrentControlScheme(Keyboard.current);
                
                PlayerData.playerIndex.Add(playerID);
                PlayerData.currency.Add(0);

            }

            private void SmoothInputMovement()
            {
                smoothInputMovement = Vector3.LerpUnclamped(smoothInputMovement, rawInputMovement,
                Time.deltaTime * MovementSmoothing);
            }

            private void UpdatePlayerMovement()
            {
                playerMovementBehaviour.MovementData(smoothInputMovement);
            }
            
            
            #endregion
        }

        

    }
}