// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: b22alesj
// Description: Controls the score screen
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Backend;
using Game.CharacterSelection;
using Game.Managers;
using Game.WorldMap;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game {
    namespace UI {
        public class ScoreScreenUI : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private GameObject playerScoreCanvasPrefab;
            [SerializeField] private GameObject playerScoreParent;
            [SerializeField] private List<Sprite> playerImages;
            [SerializeField] private List<Sprite> personalObjectiveImages;
            [SerializeField] private LevelDataSO worldMap;
            
            [Header("Debug")]
            [SerializeField] private List<PlayerData> playerDatas;
            
            private int playersInScoreScreen;
            private int playersDoneCountingUp;

            private void Start()
            {
                playersInScoreScreen = 0;
                
                if (CharacterSelect.selectedCharacters.Count > 0)
                {
                    foreach (KeyValuePair<InputDevice, PlayerData> _kvp in CharacterSelect.selectedCharacters)
                    {
                        PlayerScoreUI playerScoreUI = Instantiate(playerScoreCanvasPrefab, playerScoreParent.transform).GetComponent<PlayerScoreUI>();
                        playerScoreUI.SetupUI(_kvp.Key, this, _kvp.Value, playerImages[_kvp.Value.playerIndex], personalObjectiveImages[_kvp.Value.playerIndex]);
                        playersInScoreScreen++;
                    }
                }
                else
                {
                    int _inputIndex = 0;
                    foreach (InputDevice _inputDevice in InputSystem.devices)
                    {
                        if (_inputDevice is Keyboard or Gamepad)
                        {
                            PlayerScoreUI playerScoreUI = Instantiate(playerScoreCanvasPrefab, playerScoreParent.transform).GetComponent<PlayerScoreUI>();
                            playerScoreUI.SetupUI(_inputDevice, this, playerDatas[_inputIndex], playerImages[_inputIndex], personalObjectiveImages[_inputIndex]);
                            playersInScoreScreen++;
                            
                            _inputIndex++;
                            if (_inputIndex >= 4)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            public void DoneCountingUp()
            {
                playersDoneCountingUp++;
            }

            public void OnSubmitPressed(InputAction.CallbackContext _value)
            {
                Debug.Log("Submit");
                
                if (playersDoneCountingUp == playersInScoreScreen)
                {
                    LevelManager.Instance.LoadLevel(worldMap);
                }
            }
        }
    }
}