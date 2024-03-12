// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-10
// Author: Felix
// Description: Treasure and Treachery
// --------------------------------
// ------------------------------*/

using System.Collections;
using System.Collections.Generic;
using Game.Scene;
using UnityEngine;
using UnityEngine.Events;

namespace Game {
    namespace Interactable {
        public class InteractableButton : MonoBehaviour { 
            
            [Header("References")]
            [Tooltip("Add all buttons to work together for the event to trigger. Do not add this one to the list as it is added automatically.")]
            [SerializeField] private List<InteractableButton> buttons;
            [SerializeField] private DynamicButtonSpawn buttonSpawn;

            [Header("Settings")]
            [SerializeField] private bool toggle;
            [SerializeField] private Color pressedColor;
            
            [Header("Debug")]
            [SerializeField] public bool isPressed;
            
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
                buttonSpawn = GetComponentInParent<DynamicButtonSpawn>();

                for (int i = buttons.Count - 1; i >= 0; i--) {
                    buttons[i].isPressed = false;
                    if (!buttons[i].isActiveAndEnabled) {
                        buttons.Remove(buttons[i]);
                    }
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
                    buttonSpawn.allButtonsPressed.Invoke();
                }
            }
            
            private void OnTriggerExit(Collider other) {
                if (!other.CompareTag("Player") || toggle)
                    return;
                
                isPressed = false;
                renderer.materials[1].color = originalColor;
                buttonSpawn.offButtonPressed.Invoke();
            }

            public void RemoveFromList(int button) {
                buttons.RemoveAt(button);
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
