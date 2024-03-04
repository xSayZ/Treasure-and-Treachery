// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-19
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
    namespace Racer {
        public class CarriageRacer : MonoBehaviour
        {
            [Header("Sub Behaviours")]
            [SerializeField] private CarriageMovementBehaviour carriageMovementBehaviour;
            [SerializeField] private CarriageAnimationBehaviour carriageAnimationBehaviour;
            
            [Header("Player Inputs")]
            [SerializeField] private List<Vector2> activeLeftStickValues = new List<Vector2>();
            
            private Vector3 averageLeftStickValue;

            private bool submitPressed = false;

#region Unity Functions
            private void Start() {
                carriageMovementBehaviour.SetupBehaviour();
                carriageAnimationBehaviour.SetupBehaviour();
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

            private void OnMovement() {
                // Clear the list of active left stick values
                activeLeftStickValues.Clear();
                
                // Iterate through all gamepads
                foreach (Gamepad gamepad in Gamepad.all) {
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
                carriageAnimationBehaviour.UpdateMovementAnimation(averageLeftStickValue.magnitude);
            }

            private Vector3 CalculateAverageLeftStickValue()
            {
                if (activeLeftStickValues.Count == 0)
                    return Vector2.zero;
                
                Vector2 average = Vector2.zero;

                int notZeroCount = 0;
                
                foreach (var value in activeLeftStickValues)
                {
                    average += value;
                    if (value != Vector2.zero)
                    {
                        notZeroCount++;
                    }
                }

                if (notZeroCount > 0)
                {
                    average /= notZeroCount;
                }
                
                averageLeftStickValue = new Vector3(average.x, 0, average.y);
                return averageLeftStickValue / activeLeftStickValues.Count;
            }
#endregion
        }
    }
}
