// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: b22alesj
// Description: Controls the player score canvas
// --------------------------------
// ------------------------------*/

using Game.Backend;
using TMPro;
using UnityEngine;


namespace Game {
    namespace UI {
        public class PlayerScoreUI : MonoBehaviour
        {
            [SerializeField] private TextMeshProUGUI pointsText;
            [SerializeField] private TextMeshProUGUI coinsText;
            [SerializeField] private TextMeshProUGUI killsText;

            public void SetupUI(PlayerData _playerData)
            {
                coinsText.text = _playerData.currency.ToString();
                killsText.text = _playerData.killsThisLevel.ToString();
            }
        }
    }
}
