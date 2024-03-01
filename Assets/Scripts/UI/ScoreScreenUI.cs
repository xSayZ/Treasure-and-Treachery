// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: b22alesj
// Description: Controls the score screen
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Backend;
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
            
            [Header("Debug")]
            [SerializeField] private List<PlayerData> playerDatas;
            
            private int playersDoneCountingUp;

            private void Start()
            {
                if (CharacterSelectHandler.staticData.Count > 0)
                {
                    foreach (CharacterSelect _characterSelect in CharacterSelectHandler.staticData)
                    {
                        PlayerScoreUI playerScoreUI = Instantiate(playerScoreCanvasPrefab, transform).GetComponent<PlayerScoreUI>();
                        playerScoreUI.SetupUI(_characterSelect.data, playerImages[_characterSelect.data.playerIndex], personalObjectiveImages[_characterSelect.data.playerIndex]);
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
                if (playersDoneCountingUp == Input.GetJoystickNames().Length)
                {
                    // Load map
                }
            }
        }
    }
}