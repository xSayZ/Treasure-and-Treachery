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

            private int kills;

            public void SetupBehaviour() {
                playerUIElements.SetActive(false);
                
                QuestManager.OnGoldPickedUp.AddListener(UpdateCurrencyDisplay);
                EnemyManager.OnEnemyDeath.AddListener(UpdateKillsDisplay);
                
                
                currencyText.SetText("0");
                killsText.SetText("0");
                
            }

            private void UpdateCurrencyDisplay(int _playerID, int _currency) {
                currencyText.SetText((_currency).ToString());
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
