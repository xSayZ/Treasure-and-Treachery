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
using UnityEngine.SceneManagement;


namespace Game {
    namespace UI {
        public class ScoreScreenUI : MonoBehaviour
        {
            [SerializeField] private GameObject playerScoreCanvasPrefab;
            [SerializeField] private List<RenderTexture> renderTextures;
            [SerializeField] private List<PlayerData> playerData; // Temporary

            private int playersDoneCountingUp;
            
            private void Start()
            {
                for (int i = 0; i < Input.GetJoystickNames().Length; i++)
                {
                    PlayerScoreUI playerScoreUI = Instantiate(playerScoreCanvasPrefab, transform).GetComponent<PlayerScoreUI>();
                    playerScoreUI.SetupUI(renderTextures[i], playerData[i]);
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
                    SceneManager.LoadScene(sceneBuildIndex: GameManager.Instance.nextSceneBuildIndex);
                }
            }
        }
    }
}
