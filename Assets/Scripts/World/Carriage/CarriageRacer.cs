// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-19
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game {
    namespace Racer {
        public class CarriageRacer : MonoBehaviour
        {
            [Header("Input Settings")]
            [SerializeField] private PlayerInput playerInput;
            
            [Header("Sub Behaviours")]
            [SerializeField] private CarriageMovementBehaviour carriageMovementBehaviour;

            private Vector2 movementInput;
            private bool breakingInput;

#region Unity Functions
            private void Start() {
                carriageMovementBehaviour.SetupBehaviour();
            }

            private void FixedUpdate() {
                UpdateCarriageMovement();
            }
#endregion

            public void OnMovement(InputAction.CallbackContext value) {
                movementInput = value.ReadValue<Vector2>();
            }


#region Public Functions

#endregion

#region Private Functions

            
            private void UpdateCarriageMovement() {
                carriageMovementBehaviour.UpdateMovementData(movementInput);
            }
#endregion
        }
    }
}
