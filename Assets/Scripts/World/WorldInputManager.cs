// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-27
// Author: Felix
// Description: Handles Inputs from players 
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using Game.UI;
using Game.Voting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


namespace Game {
    namespace World {
        public class WorldInputManager : MonoBehaviour {
            public GameObject menuItem;
            public Canvas canvas;

            [SerializeField] private List<GameObject> buttons = new List<GameObject>();
            [SerializeField] private List<VotingButton> votingButtons = new List<VotingButton>();
            
            public Dictionary<PlayerInput, int> playerInputs = new Dictionary<PlayerInput, int>();
            
            public PlayerInputManager playerInputManager;
            
            private bool test = true;
            private void Start() {
                var _players = Input.GetJoystickNames();
                for (int i = 0; i < _players.Length; i++) {
                    var _player = playerInputManager.JoinPlayer(i, -1, null);
                    playerInputs.Add(_player, i);
                }
            }

            public void JoinNewPlayer(PlayerInput _playerInput) {
                
                _playerInput.uiInputModule = _playerInput.gameObject.GetComponent<InputSystemUIInputModule>();
                var _multiplayerEventSystem = _playerInput.gameObject.GetComponent<MultiplayerEventSystem>();
                _multiplayerEventSystem.playerRoot = canvas.gameObject;
                _multiplayerEventSystem.firstSelectedGameObject = menuItem;
                
                StartCoroutine(SelectFirstChoice(_playerInput.uiInputModule));
            }


            private IEnumerator SelectFirstChoice(InputSystemUIInputModule _inputModule) 
            {
                // Event System requires we clear it first, then wait
                // for at least one frame before we set the current selected object.
                _inputModule.gameObject.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(null);
                yield return new WaitForEndOfFrame();
                _inputModule.gameObject.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(menuItem);
            }
        }
    }
}
