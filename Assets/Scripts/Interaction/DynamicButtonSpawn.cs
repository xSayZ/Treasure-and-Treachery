using System;
using System.Collections.Generic;
using System.Linq;
using Game.Backend;
using Game.Interactable;
using UnityEngine;
using UnityEngine.Events;

namespace Game {
    namespace Scene {
        public class DynamicButtonSpawn : MonoBehaviour {
            public List<InteractableButton> buttons;

            [Header("Events")]
            [Tooltip("Event that triggers when all buttons are pressed at the same time.")]
            [SerializeField] public UnityEvent allButtonsPressed = new UnityEvent();
            [SerializeField] public UnityEvent offButtonPressed = new UnityEvent();

            private void Start() {
                int _playerCount = GameManager.Instance.ActivePlayerControllers.Count;
                SinglePlayerToggle();
                GameManager.OnPlayerDeath.AddListener(ToggleButtons);

                for (int i = 0; i < buttons.Count; i++) {
                    if (i <= _playerCount - 1)
                        continue;
                    
                    buttons[i].gameObject.SetActive(false);
                    
                }
                foreach (InteractableButton button in buttons.Where(button => button.gameObject.activeSelf)) {
                    button.Setup();
                }
            }
            
            private void SinglePlayerToggle() {
                if (GameManager.Instance.ActivePlayerControllers.Count == 1) {
                    foreach (var button in buttons) {
                        button.isToggle = true;
                    }
                }
            }

            private void ToggleButtons(int _playerIndex) {
                SinglePlayerToggle();
                buttons[_playerIndex].isPressed = true;
                buttons[_playerIndex].isToggle = true;
            }

            private void OnDisable() {
                GameManager.OnPlayerDeath.RemoveListener(ToggleButtons);
            }
        }
    }
}
