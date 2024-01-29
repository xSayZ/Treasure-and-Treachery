// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using Game.Backend;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game
{
    namespace Player
    {
        public class PlayerController : MonoBehaviour
        {
            [Tooltip("Set to use Keyboard debugging purposes")]
            public bool Controllers;
            [Header("SubBehaviours")] [SerializeField]
            private PlayerMovementBehaviour playerMovementBehaviour;

            [SerializeField] private PlayerInput playerInput;
            
            private int playerID;
            
            public PlayerData PlayerData;

            private GameManager gameManager;
            private string controlScheme;

            #region Unity Functions

            // Start is called before the first frame update
            void Start()
            {
                
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
            

            public void SetupPlayer(int _newPlayerId)
            {
                playerID = _newPlayerId;
                controlScheme = playerInput.defaultControlScheme;
                
                switch (playerID)
                {
                    case 0:
                        controlScheme = "Player1";
                        break;
                    case 1:
                        controlScheme = "Player2";
                        break;
                    case 2:
                        controlScheme = "Player3";
                        break;
                    case 3:
                        controlScheme = "Player4";
                        break;
                }

                // Switches the input Device Connected
                
                for (int i = 0; i < playerInput.devices.Count; i++)
                {
                    if (Controllers)
                    {
                        playerInput.SwitchCurrentControlScheme(controlScheme, playerInput.devices[i]);

                    }
                    else
                    {
                        playerInput.SwitchCurrentControlScheme(controlScheme, Keyboard.current);

                    }
                }
                
                if (!playerInput.hasMissingRequiredDevices || !playerInput.inputIsActive)
                {
                    gameObject.SetActive(false);
                }
                

            }
            
            #endregion

            #region Private Functions
            #endregion
        }
    }
}