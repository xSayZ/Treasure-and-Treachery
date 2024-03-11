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
            public List<InteractableButton> buttons;
            public bool isPressed;
            
            public Color pressedColor;
            
            public UnityEvent allButtonsPressed = new UnityEvent();
            public UnityEvent OffButtonPressed = new UnityEvent();
            
            private Color originalColor;
            private Renderer _renderer;

            private void Start()
            {
                _renderer = GetComponentInChildren<Renderer>();
                originalColor = _renderer.material.color;
                buttons.Add(this);
                foreach (var _button in buttons) {
                    _button.isPressed = false;
                }
            }

            private void OnTriggerEnter(Collider other)
            {
                if (other.CompareTag("Player")) {
                    isPressed = true;
                    _renderer.material.color = pressedColor;
                    if (CheckAllButtonsPressed()) {
                        allButtonsPressed.Invoke();
                    }
                }
            }
            
            private void OnTriggerExit(Collider other)
            {
                if (other.CompareTag("Player")) {
                    isPressed = false;
                    _renderer.material.color = originalColor;
                    OffButtonPressed.Invoke();
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
