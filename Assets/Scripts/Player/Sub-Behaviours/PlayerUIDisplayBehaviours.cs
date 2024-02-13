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
            
            [SerializeField] private TextMeshProUGUI currencyText, killsText;
            [SerializeField] private GameObject playerUIElements;
            
            private PlayerController playerController;

            public void SetupBehaviour(PlayerController _playerController)
            {
                playerController = _playerController;
                playerUIElements.SetActive(false);
                
                currencyText.SetText("0");
                killsText.SetText("0");
            }

            private void OnEnable()
            {
                QuestManager.OnGoldPickedUp.AddListener(UpdateCurrencyDisplay);
                EnemyManager.OnEnemyDeathUI.AddListener(UpdateKillsDisplay);
            }

            private void OnDisable()
            {
                QuestManager.OnGoldPickedUp.RemoveListener(UpdateCurrencyDisplay);
                EnemyManager.OnEnemyDeathUI.RemoveListener(UpdateKillsDisplay);
            }

            private void UpdateCurrencyDisplay(int _playerIndex, int _amount)
            {
                currencyText.SetText(playerController.PlayerData.currencyThisLevel.ToString());
            }

            private void UpdateKillsDisplay() 
            {
                killsText.SetText(playerController.PlayerData.killsThisLevel.ToString());
            }

            public void TogglePlayerUIElements(bool _active)
            {
                playerUIElements.SetActive(_active);
            }
        }
    }
}
