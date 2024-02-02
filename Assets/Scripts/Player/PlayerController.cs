// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/


using System;
using System.Collections;
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
            
            [field:SerializeField]public int Health { get; set; }

        
            private Vector3 _rawInputMovement;

            public bool WalkOnGraves;

            public bool dashAttack;

            [Header("Test Stuff")] 
            public Material _material;
            void Start()
            {
                SetupPlayer();
                EventManager.OnCurrencyPickup.AddListener(BeginCurrencyPickup);
            }
            private void Update()
            {
               Death();
               if (WalkOnGraves)
               {
                   
               }
               OnRayHit();
            }

            private void FixedUpdate()
            {
                if (WalkOnGraves)
                {
                    Ray();

                }
            }


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
                Vector2 _inputValue = value.ReadValue<Vector2>();

                // Dashing will lock character from moving direction during the duration 
               
                    if (CharacterType == Archetype.Melee || CharacterType == Archetype.Both)
                    {
                        _rawInputMovement = (new Vector3(_inputValue.x, 0, _inputValue.y));
                        
                        playerMovementBehaviour.MovementData(IsoVectorConvert(_rawInputMovement));
                    }
            }


            
            public void OnDash(InputAction.CallbackContext value)
            {
                if (value.action.WasPressedThisFrame()  && playerMovementBehaviour.currentLockoutTime <=0)
                {
                    //Todo:: PlayDustCloud Particle if needed
                    //playerMovementBehaviour.MovementData(IsoVectorConvert(_rawInputMovement*DashModifier));
                    playerMovementBehaviour.Dash(value.action.WasPressedThisFrame());
                }
            }
            
            public void OnRanged(InputAction.CallbackContext value)
            {
                //TODO:: Make Character Charge up and able to
                if (value.action.triggered)
                {
                    playerMovementBehaviour.MovementData(Vector3.zero);
                    if (CharacterType == Archetype.Ranged || CharacterType == Archetype.Both)
                    {
                        playerMovementBehaviour.TurnPlayer();
                        playerAttackBehaviour.RangedAttack();

                    }
                   
                }
            }
            
            public void OnMelee(InputAction.CallbackContext value)
            {
                if (value.action.triggered)
                {
                    playerMovementBehaviour.TurnPlayer();
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
                    Destroy(gameObject);
                    
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
            
            private Vector3 IsoVectorConvert(Vector3 vector)
            {
                Vector3 cameraRot = UnityEngine.Camera.main.transform.rotation.eulerAngles;
                Quaternion rotation = Quaternion.Euler(0,cameraRot.y, 0);
                Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
                Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
                return result;

            }
            #endregion
            
            #region Experimental code

            public Vector3 DownDir;
            public float RideSpringDamper;
            public float RideSpringStrength;
            public float RideHeight;

            private RaycastHit _rayHit;
        
            public bool _rayDidHit;
        
            public float SphereCheckNumber;
            public float raycastDistance;
        

            #endregion

            void OnRayHit()
            {
                DownDir = IsoVectorConvert(-transform.up);
                if (Physics.Raycast(transform.position,-transform.up,out _rayHit,raycastDistance))
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
                    Debug.DrawLine(transform.position,transform.position+(rayDir*springForce),Color.blue);
                    GetComponent<Rigidbody>().AddForce(IsoVectorConvert(rayDir*springForce));
                    if (hitBody != null)
                    {
                        hitBody.AddForceAtPosition(rayDir*-springForce,IsoVectorConvert(_rayHit.point));
                    }
                }
            }
        }

    

    }
}