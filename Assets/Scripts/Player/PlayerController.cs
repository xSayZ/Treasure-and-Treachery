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
            
            private float currentDashCooldown;

            public float angle;
            void Start()
            {
                SetupPlayer();
                EventManager.OnCurrencyPickup.AddListener(BeginCurrencyPickup);
                currentDashCooldown = 0;

            }
            private void Update()
            {
               Death();
                WaitTimeBeforeNextDash();
            }

            [field:SerializeField]public int Health { get; set; }

            public void Death()
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
                    if (CharacterType == Archetype.Melee || CharacterType == Archetype.Both)
                    {
                        Vector2 _inputValue = value.ReadValue<Vector2>();
                        Vector3 _rawInputMovement = (new Vector3(_inputValue.x, 0, _inputValue.y));
                        
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
                    
                    playerMovementBehaviour.MovementData(transform.forward*DashModifier);
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

                    }
                   
                }
            }
            
            public void OnMelee(InputAction.CallbackContext value)
            {
                playerMovementBehaviour.MovementData(Vector3.zero);
                if (value.action.triggered)
                {
                    playerAttackBehaviour.MeleeAttack();
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
                    gameObject.SetActive(false);
                    
                }
                PlayerInput.SwitchCurrentControlScheme(Keyboard.current);
            }
            
            private void BeginCurrencyPickup(int pickUpGold,int _playerId)
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
                dashing = false;
            }

            private void WaitTimeBeforeNextDash()
            {
                if (dashing ==false)
                {
                    currentDashCooldown -= Time.deltaTime;
                }
            }

            private Vector3 IsoVectorConvert(Vector3 vector)
            {
                Quaternion rotation = Quaternion.Euler(0,angle, 0);
                Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
                Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
                return result;

            }
            #endregion
        }
        

        

    }
}