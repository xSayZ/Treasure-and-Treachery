// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using Game.Backend;
using Game.Enemy;
using Game.Quest;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace Game {
    namespace Player {
        public class PlayerUIDisplayBehaviours : MonoBehaviour {
            
            public TextMeshProUGUI currencyText, killsText;
            public GameObject playerUIElements;


            private PlayerController player;
            private int kills;
            private int currency;

            public void SetupBehaviour(PlayerController _player) {
                player = _player;
                playerUIElements.SetActive(false);
                
                QuestManager.OnGoldPickedUp.AddListener(UpdateCurrencyDisplay);
                EnemyManager.OnEnemyDeath.AddListener(UpdateKillsDisplay);
                
                
                currencyText.SetText("0");
                killsText.SetText("0");
                
            }

            private void UpdateCurrencyDisplay(int _playerID, int _currency) {
                currency = player.PlayerData.currency;
                currencyText.SetText((currency).ToString());
            }

            private void UpdateKillsDisplay(EnemyController _enemy) {
                kills++;
                killsText.SetText((kills).ToString());
            }

            public void TogglePlayerUIElements(bool _active) {
                playerUIElements.SetActive(_active);
            }
        }
    }
}
