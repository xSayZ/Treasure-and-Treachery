// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-19
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
    namespace Racer {
        public class CarriageRacer : MonoBehaviour
        {
            
            [Header("Player Inputs")]
            [SerializeField] private List<Vector2> activeLeftStickValues = new List<Vector2>();
            
            [Header("Sub Behaviours")]
            [SerializeField] private CarriageMovementBehaviour carriageMovementBehaviour;
            
            [SerializeField] private Vector3 averageLeftStickValue;

            private bool submitPressed = false;

#region Unity Functions
            private void Start() {
                carriageMovementBehaviour.SetupBehaviour();
            }

            private void Update()
            {
                OnMovement();
            }

            private void FixedUpdate() {
                UpdateCarriageMovement();
            }
#endregion
            
#region Public Functions

#endregion

#region Private Functions
            private void UpdateCarriageMovement() {
                carriageMovementBehaviour.UpdateMovementData(averageLeftStickValue);
            }

            public void OnSubmit(InputAction.CallbackContext context)
            {
                if (context.performed) {
                    submitPressed = true;
                } else if (context.canceled) {
                    submitPressed = false;
                }
            }

            public bool GetSubmitPressed()
            {
                bool result = submitPressed;
                submitPressed = false;
                return result;
            }

            private void OnMovement() {
                // Clear the list of active left stick values
                activeLeftStickValues.Clear();
                
                // Iterate through all gamepads
                foreach (Gamepad gamepad in Gamepad.all) {
                    Debug.Log("Gamepad found");
                    // Check if the gamepad is actuated
                    if (gamepad.IsActuated()) {
                        
                        // Get the input from the left stick
                        var input = gamepad.leftStick.ReadValue();
                        
                        // Add the input to the list
                        activeLeftStickValues.Add(input);
                    }
                }
                
                // Calculate the average left stick value
                averageLeftStickValue = CalculateAverageLeftStickValue();
            }

            private Vector3 CalculateAverageLeftStickValue()
            {
                if (activeLeftStickValues.Count == 0)
                    return Vector2.zero;
                
                Vector2 average = Vector2.zero;
                
                foreach (var value in activeLeftStickValues) {
                    average += value;
                }

                averageLeftStickValue = new Vector3(average.x, 0, average.y);
                return averageLeftStickValue / activeLeftStickValues.Count;
            }
#endregion
        }
    }
}