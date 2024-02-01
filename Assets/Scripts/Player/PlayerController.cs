// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/


using System;
using System.Collections;
using System.Diagnostics;
using Game.Backend;
using Game.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;


namespace Game
{
    namespace Player
    {
        using Events;
        using Scenes;
        public class PlayerController : MonoBehaviour,IDamageable
        {
            
            public PlayerData PlayerData;
            public int PlayerID { get; private set; }

            public static int CurrentAmountOfControllers;
            
            //temp Health Solution
            [Header("SubBehaviours")] 
            [SerializeField]
            private PlayerMovementBehaviour playerMovementBehaviour;
            [SerializeField] private AttackBehaviour playerAttackBehaviour;
            
            [Header("InputSettings")]
            [SerializeField] private PlayerInput PlayerInput;
            #region Unity Functions
            

            [Header("Dash Data")]
            [Tooltip("How much current displacement should increase with")]
            public float DashModifier;
            public float dashTime;
            public float waitTimeUntilNextDash;
            private bool dashing;
            
            private bool waitUntilNextDash;
            void Start()
            {
                CurrentAmountOfControllers = Gamepad.all.Count;
                SetupPlayer();
                EventManager.OnCurrencyPickup.AddListener(BeginCurrencyPickup);
                
            }
            private void Update()
            {
               Death();
                WaitTimeBeforeNextDash();
            }

            [field:SerializeField]public int Health { get; set; }

            private void Death()
            {
                if (Health <=0)
                {
                    gameObject.SetActive(false);
                }
            }

            #endregion

            #region Public Functions

            public void OnMovement(InputAction.CallbackContext value)
            {
                // Dashing will lock character from moving direction during the duration 
                if (!dashing)
                {
                    Vector2 _inputValue = value.ReadValue<Vector2>();
                    Vector3 _rawInputMovement = (new Vector3(_inputValue.x, 0, _inputValue.y));
                    
                    playerMovementBehaviour.MovementData(_rawInputMovement);
                }
            }
            
            public void OnRanged(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    //TODO: ADD MovementData = 0,0,0
                    //TODO;; PlayAttackAnimation
                    playerAttackBehaviour.RangedAttack();
                   
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
                    playerAttackBehaviour.MeleeAttack();
                }  
            }

            public void OnDash(InputAction.CallbackContext value)
            {
                
                if (value.started && dashing == false  && waitTimeUntilNextDash >=0)
                {
                    //Add Dash dust cloud if wanted
                    playerMovementBehaviour.MovementData(transform.forward*DashModifier);
                    StartCoroutine(WaitUntilDashComplete());
                    dashing = true;
                    

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


            
            #region Private Functions
            private void SetupPlayer()
            {
                
                PlayerID = PlayerInput.playerIndex;
                
                Health = PlayerData.playerHealth;
                
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
            
            
            
            //TODO: Move Dash to playerMovement
            private IEnumerator WaitUntilDashComplete()
            {
                yield return new WaitForSeconds(dashTime);
                dashing = false;
            }

            private void WaitTimeBeforeNextDash()
            {
                if (waitUntilNextDash)
                {
                    waitTimeUntilNextDash -= Time.deltaTime;
                }
            }


            #endregion
        }
        

        

    }
}