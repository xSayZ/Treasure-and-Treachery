// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using Cinemachine;
using Game.Backend;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game
{
    namespace Player
    {
        public class PlayerController : MonoBehaviour
        {
            public PlayerData Data;
            private int playerID;
            public CinemachineTargetGroup targetGroup;
            
            [Header("SubBehaviours")] [SerializeField]
            private PlayerMovementBehaviour playerMovementBehaviour;
            [SerializeField] private PlayerInput playerInput;
            
            #region Unity Functions

            // Start is called before the first frame update
            void Start()
            {
                SetupPlayer();
                targetGroup = FindObjectOfType<CinemachineTargetGroup>();
            }

            // Update is called once per frame
            void Update()
            {
            }
            
            #endregion

            #region Public Functions

            public void OnMovement(InputAction.CallbackContext value)
            {
                Vector2 inputValue = value.ReadValue<Vector2>();

                playerMovementBehaviour.MovementData(new Vector3(inputValue.x, 0, inputValue.y));
            }

            public void OnAttack(InputAction.CallbackContext value)
            {
                if (value.started)
                {
                    //TODO;; PlayAttackAnimation 
                }
            }
            
            #endregion

            #region Private Functions
            private void SetupPlayer()
            {
                playerID = playerInput.playerIndex;
                Data.playerIndex = playerID;

                if (playerInput.playerIndex !=0 && playerInput.currentControlScheme !="Player1")
                {
                    gameObject.SetActive(false);
                    
                }
                playerInput.SwitchCurrentControlScheme(Keyboard.current);
            }


           
            #endregion
        }
        

    }
}