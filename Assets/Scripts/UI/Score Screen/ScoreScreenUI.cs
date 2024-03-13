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
            [SerializeField] private List<Sprite> playerImages;
            [SerializeField] private List<Sprite> personalObjectiveImages;
            [SerializeField] private LevelDataSO worldMap;
            
            [Header("Debug")]
            [SerializeField] private List<PlayerData> playerDatas;

            private int playersDoneCountingUp;

            private void Start()
            {
                if (CharacterSelect.selectedCharacters.Count > 0)
                {
                    foreach (KeyValuePair<InputDevice, PlayerData> _kvp in CharacterSelect.selectedCharacters)
                    {
                        PlayerScoreUI playerScoreUI = Instantiate(playerScoreCanvasPrefab, transform).GetComponent<PlayerScoreUI>();
                        playerScoreUI.SetupUI(_kvp.Value, playerImages[_kvp.Value.playerIndex], personalObjectiveImages[_kvp.Value.playerIndex]);
                    }
                }
                else
                {
                    for (int i = 0; i < Input.GetJoystickNames().Length; i++)
                    {
                        PlayerScoreUI playerScoreUI = Instantiate(playerScoreCanvasPrefab, transform).GetComponent<PlayerScoreUI>();
                        playerScoreUI.SetupUI(playerDatas[i], playerImages[i], personalObjectiveImages[i]);
                    }
                }
            }

            public void DoneCountingUp()
            {
                playersDoneCountingUp++;
            }

            public void OnSubmitPressed(InputAction.CallbackContext _value)
            {
                if (playersDoneCountingUp == CharacterSelect.selectedCharacters.Count)
                {
                    LevelManager.Instance.LoadLevel(worldMap);
                }
            }
        }
    }
}