// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
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
            private int playerID;

            [Header("SubBehaviours")] [SerializeField]
            private PlayerMovementBehaviour playerMovementBehaviour;

            [SerializeField] private AttackBehaviour playerAttackBehaviour;

            [Header("InputSettings"), Range(0.1f, 10)]
            [Tooltip("Effects How effective turning is and inertial movement")]
            [SerializeField] private PlayerInput playerInput;
            public float MovementSmoothing;

            
            [Header("Other Settings")]
            [Tooltip("How long before able to Moveagain after shooting")]
            public float BaseLockoutTimer;
            private float currentLockoutTimer= 0;
            private Vector3 rawInputMovement;
            private Vector3 smoothInputMovement;
            private Vector3 velocity;
            private bool lockout;

            public float Currency;


            #region Unity Functions
       
            // Start is called before the first frame update

            private void Awake()
            {
                EventManager.OnCurrencyPickup.AddListener(BeginCurrencyPickup);
            }
            
            void Start()
            {
                SetupPlayer();
                
            }


            private void BeginCurrencyPickup(int pickUpGold)
            {
                PlayerData.currency += pickUpGold;
                Currency = PlayerData.currency;
                Debug.Log(PlayerData.currency);
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
                PlayerData.playerIndex = playerID;

                if (playerInput.playerIndex !=0 && playerInput.currentControlScheme !="Player1")
                {
                    gameObject.SetActive(false);
                    
                }
                playerInput.SwitchCurrentControlScheme(Keyboard.current);

                PlayerData.playerIndex = playerID;

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