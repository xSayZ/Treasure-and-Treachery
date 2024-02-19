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
            [Tooltip("Effects How smooth the movement Interpolation is. Higher value is smoother movement. Lower value is more responsive movement.")]
            [SerializeField] public float movementSmoothingSpeed = 1f;
            private Vector3 rawInputMovement;
            private Vector3 smoothInputMovement;
            
            [Header("Sub Behaviours")]
            [SerializeField] private CarriageMovementBehaviour carriageMovementBehaviour;

#region Unity Functions
            private void Start() {
                carriageMovementBehaviour.SetupBehaviour();
            }
            private void FixedUpdate() {
                CalculateMovementInputSmoothing();
                
                UpdateCarriageMovement();
                
            }
#endregion

            public void OnMovement(InputAction.CallbackContext value) {
                Vector2 input = value.ReadValue<Vector2>();
                rawInputMovement = new Vector3(input.x, 0, input.y);
            }

#region Public Functions

#endregion

#region Private Functions
            private void CalculateMovementInputSmoothing()
            {
                smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
            }
            
            private void UpdateCarriageMovement()
            {
                carriageMovementBehaviour.UpdateMovementData(smoothInputMovement);
            }
#endregion
        }
    }
}
