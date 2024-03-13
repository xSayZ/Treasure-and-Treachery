// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-12
// Author: alexa
// Description: Handles the character select
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Backend;
using Game.Managers;
using Game.WorldMap;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace Game {
    namespace CharacterSelection {
        public class CharacterSelect : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private GameObject startGameIcon;
            [SerializeField] private LevelDataSO levelToLoad;
            
            [SerializeField] public List<Transform> PlayerImageTransforms;
            
            [HideInInspector] public UnityEvent UpdateImageTint;
            
            public static Dictionary<InputDevice, PlayerData> selectedCharacters { get; private set; } = new Dictionary<InputDevice, PlayerData>(); // Input device, selected character player data
            
            private int joinedPlayerCount;

#region Unity Functions
            private void Awake()
            {
                selectedCharacters = new Dictionary<InputDevice, PlayerData>();
            }
#endregion

#region Input Functions
            public void PlayerJoined(PlayerInput _playerInput)
            {
                joinedPlayerCount++;
                UpdateStartUI();
                Debug.Log($"Input device {_playerInput.devices[0].deviceId} joined ({_playerInput.devices[0].name})");
            }

            public void PlayerLeft(PlayerInput _playerInput)
            {
                joinedPlayerCount--;
                UpdateStartUI();
                Debug.Log($"Input device {_playerInput.devices[0].deviceId} left ({_playerInput.devices[0].name})");
            }
#endregion

#region Public Functions
            public bool SelectCharacter(InputDevice _inputDevice, PlayerData _playerData)
            {
                // Character already selected by someone else
                if (selectedCharacters.ContainsValue(_playerData))
                {
                    return false;
                }
                
                Debug.Log($"Input device {_inputDevice.deviceId} selected {_playerData.name}");
                
                //Select character
                selectedCharacters[_inputDevice] = _playerData;
                
                UpdateStartUI();
                
                UpdateImageTint.Invoke();
                
                return true;
            }

            public void DeselectCharacter(InputDevice _inputDevice)
            {
                // Remove entry from selected characters dictionary
                if (selectedCharacters.ContainsKey(_inputDevice))
                {
                    Debug.Log($"Input device {_inputDevice.deviceId} deselected {selectedCharacters[_inputDevice].name}");
                    selectedCharacters.Remove(_inputDevice);
                    
                    UpdateStartUI();
                    
                    UpdateImageTint.Invoke();
                }
            }

            public void StartGame()
            {
                if (selectedCharacters.Count == joinedPlayerCount && joinedPlayerCount > 0)
                {
                    Debug.Log("Starting game");
                    LevelManager.Instance.LoadLevel(levelToLoad);
                }
            }
#endregion

#region Private Functions
            private void UpdateStartUI()
            {
                startGameIcon.SetActive(selectedCharacters.Count == joinedPlayerCount && joinedPlayerCount > 0);
            }
#endregion
        }
    }
}