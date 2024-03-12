// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-10
// Author: Felix
// Description: Treasure and Treachery
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game {
    namespace Interactable {
        public class InteractableButton : MonoBehaviour { 
            
            [Header("References")]
            [Tooltip("Add all buttons to work together for the event to trigger. Do not add this one to the list as it is added automatically.")]
            [SerializeField] private List<InteractableButton> buttons;

            [Header("Settings")]
            [SerializeField] private bool toggle;
            [SerializeField] private Color pressedColor;
            
            [Header("Events")]
            [Tooltip("Event that triggers when all buttons are pressed at the same time.")]
            [SerializeField] private UnityEvent allButtonsPressed = new UnityEvent();
            [Tooltip("Event that triggers when this button is released.")]
            [SerializeField] private UnityEvent offButtonPressed = new UnityEvent();
            
            [Header("Debug")]
            [SerializeField] private bool isPressed;
            
            // Private Variables
            private Color originalColor;
            private new Renderer renderer;
            
            // Private Arrays
            private Material[] materials;

#region Unity Functions
            private void Start() {
                Setup();
            }
#endregion

#region Private Functions

            private void Setup() {
                SetMaterial();
                buttons.Add(this);
                foreach (InteractableButton _button in buttons) {
                    _button.isPressed = false;
                }
            }
            private void SetMaterial() {
                renderer = GetComponentInChildren<Renderer>();
                materials = renderer.materials;
                originalColor = materials[1].color;
            }

            private void OnTriggerEnter(Collider other) {
                if (!other.CompareTag("Player"))
                    return;
                
                isPressed = true;
                renderer.materials[1].color = pressedColor;
                if (CheckAllButtonsPressed()) {
                    allButtonsPressed.Invoke();
                }
            }
            
            private void OnTriggerExit(Collider other) {
                if (!other.CompareTag("Player") || toggle)
                    return;
                
                isPressed = false;
                renderer.materials[1].color = originalColor;
                offButtonPressed.Invoke();
            }
            
            private bool CheckAllButtonsPressed() {
                foreach (InteractableButton _button in buttons) {
                    if (!_button.isPressed) return false;
                }

                return true;
            }
            
#endregion
        }
    }
}
