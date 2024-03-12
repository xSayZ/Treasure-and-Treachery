using System.Collections.Generic;
using Game.Backend;
using Game.Interactable;
using UnityEngine;

namespace Game {
    namespace Scene {
        public class DynamicButtonSpawn : MonoBehaviour
        {
            public List<InteractableButton> buttons;

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
