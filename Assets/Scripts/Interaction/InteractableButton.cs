// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-10
// Author: Felix
// Description: Treasure and Treachery
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game {
    namespace Interactable {
        public class InteractableButton : MonoBehaviour
        {
            [SerializeField] public List<InteractableButton> buttons;
            [SerializeField] public bool isPressed;
            
            [SerializeField] public Color pressedColor;
            
            [SerializeField] public UnityEvent allButtonsPressed = new UnityEvent();
            [SerializeField] public UnityEvent offButtonPressed = new UnityEvent();
            
            private Color originalColor;
            private Renderer renderer;
 
            private Material[] materials;

            private void Start()
            {
                renderer = GetComponentInChildren<Renderer>();
                materials = renderer.materials;
                originalColor = materials[1].color;
                buttons.Add(this);
                foreach (var _button in buttons) {
                    _button.isPressed = false;
                }
            }

            private void OnTriggerEnter(Collider other)
            {
                if (other.CompareTag("Player")) {
                    isPressed = true;
                    materials[1].color = pressedColor;
                    if (CheckAllButtonsPressed()) {
                        allButtonsPressed.Invoke();
                    }
                }
            }
            
            private void OnTriggerExit(Collider other)
            {
                if (other.CompareTag("Player")) {
                    isPressed = false;
                    materials[1].color = originalColor;
                    offButtonPressed.Invoke();
                }
            }
            
            private bool CheckAllButtonsPressed()
            {
                foreach (var _button in buttons) {
                    if (!_button.isPressed) return false;
                }

                return true;
            }
        }
    }
}
