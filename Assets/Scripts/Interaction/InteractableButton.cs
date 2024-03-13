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
using UnityEngine.Serialization;

namespace Game {
    namespace Interactable {
        public class InteractableButton : MonoBehaviour { 
            
            [Header("References")]
            [Tooltip("Add all buttons to work together for the event to trigger. Do not add this one to the list as it is added automatically.")]
            [SerializeField] private List<InteractableButton> buttons;
            [SerializeField] private DynamicButtonSpawn buttonSpawn;

            [Header("Settings")]
            [SerializeField] private bool isToggle;
            [SerializeField] private bool isTimed;
            [SerializeField] private Color pressedColor;
            [SerializeField] private float timeForButtonToReset;
            
            [Header("Debug")]
            [SerializeField] public bool isPressed;
            
            // Private Variables
            private Color originalColor;
            private new Renderer renderer;
            private bool coroutineRunning = false;
            
            // Private Arrays
            private Material[] materials;

#region Unity Functions
            private void Start() {
                if (buttonSpawn == null) {
                    Setup();               
                }
            }
#endregion

#region Private Functions

            public void Setup() {
                SetMaterial();

                for (int i = buttons.Count - 1; i >= 0; i--) {
                    buttons[i].isPressed = false;
                    if (!buttons[i].gameObject.activeSelf) {
                        buttons.RemoveAt(i);
                    }
                }
                buttons.Add(this);
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
                    if (buttonSpawn != null)
                        buttonSpawn.allButtonsPressed.Invoke();
                }
            }
            
            private void OnTriggerExit(Collider other) {
                if (!other.CompareTag("Player") || isToggle)
                    return;

                if (isTimed && !coroutineRunning) {
                    StartCoroutine(ResetButton());
                } else if (!isTimed) {
                    isPressed = false;
                    renderer.materials[1].color = originalColor;
                    if(buttonSpawn != null)
                        buttonSpawn.offButtonPressed.Invoke();
                }
            }
            
            private bool CheckAllButtonsPressed() {
                foreach (InteractableButton _button in buttons) {
                    if (!_button.isPressed) return false;
                }

                return true;
            }

            private IEnumerator ResetButton() {
                coroutineRunning = true;
                yield return new WaitForSeconds(timeForButtonToReset);
                isPressed = false;
                renderer.materials[1].color = originalColor;
                coroutineRunning = false;
                if(buttonSpawn != null)
                    buttonSpawn.offButtonPressed.Invoke();
            }

#endregion
        }
    }
}
