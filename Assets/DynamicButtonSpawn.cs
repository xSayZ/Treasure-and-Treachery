using System;
using System.Collections.Generic;
using Game.Backend;
using Game.Interactable;
using UnityEngine;
using UnityEngine.Events;

namespace Game {
    namespace Scene {
        public class DynamicButtonSpawn : MonoBehaviour
        {
            public List<InteractableButton> buttons;
            
            [Header("Events")]
            [Tooltip("Event that triggers when all buttons are pressed at the same time.")]
            [SerializeField] public UnityEvent allButtonsPressed = new UnityEvent();
            [SerializeField] public UnityEvent offButtonPressed = new UnityEvent();

            private void Start() {
                var _playerCount = GameManager.Instance.ActivePlayerControllers.Count;
                for (int i = 0; i < buttons.Count; i++) {
                    if (i <= _playerCount - 1)
                        continue;
                    
                    buttons[i].gameObject.SetActive(false);
                    foreach (InteractableButton _button in buttons) {
                        _button.RemoveFromList(i - 1);
                    }
                }
            }
        }
    }
}
