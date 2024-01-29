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


namespace Game {
    namespace Player {
        public class PlayerController : MonoBehaviour
        {
            [Header("SubBehaviours")] [SerializeField]
            private PlayerMovementBehaviour playerMovementBehaviour;

            [SerializeField] private PlayerInput playerInput;

            private string controlScheme;
            private int playerID;
            
            
            
            public PlayerData PlayerData;
#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
                SetUpPlayer(PlayerData);
            }
#endregion

#region Public Functions

        public void OnMovement(InputAction.CallbackContext value)
        {
            Vector2 inputValue = value.ReadValue<Vector2>();
            
            playerMovementBehaviour.MovementData(new Vector3(inputValue.x,0,inputValue.y));
        }


         void SetUpPlayer(PlayerData _data)
        {
            playerID = _data.playerIndex;
            controlScheme = playerInput.currentControlScheme;
        }
#endregion

#region Private Functions

        
        private void SmoothInput()
        {
            
        }
        
        
#endregion
        }
    }
}
