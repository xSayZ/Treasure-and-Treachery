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


namespace Game {
    namespace UI {
        public class ScoreScreenUI : MonoBehaviour
        {
            [SerializeField] private GameObject playerScoreCanvasPrefab;
            [SerializeField] private List<RenderTexture> renderTextures;
            [SerializeField] private List<PlayerData> playerData; // Temporary
            
            private void Start()
            {
                for (int i = 0; i < Input.GetJoystickNames().Length; i++)
                {
                    PlayerScoreUI playerScoreUI = Instantiate(playerScoreCanvasPrefab, transform).GetComponent<PlayerScoreUI>();
                    playerScoreUI.SetupUI(renderTextures[i], playerData[i]);
                }
            }
        }
    }
}
