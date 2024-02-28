// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-28
// Author: b22alesj
// Description: Controls player overhead UI
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Quest;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace Game {
    namespace Player {
        public class PlayerOverheadUIBehaviour : MonoBehaviour
        {
            [Header("Personal Objective")]
            [SerializeField] private GameObject personalObjectiveCanvas;
            [SerializeField] private TextMeshProUGUI personalObjectiveText;
            
            [Header("Popup")]
            [SerializeField] private GameObject popupCanvas;
            [SerializeField] private GameObject popupPrefab;
            [SerializeField] private Sprite popupGoldSprite;
            [SerializeField] private Sprite popupKillSprite;
            [SerializeField] private Sprite popupPersonalObjectiveSprite;
            
            [Header("Held Item")]
            [SerializeField] private GameObject heldItemCanvas;
            [SerializeField] private Image heldItemImage;
            
            private PlayerController playerController;
            private Dictionary<Sprite, PlayerPopupUI> activePopups;

#region Unity Functions
            private void OnEnable()
            {
                QuestManager.OnGoldPickedUp.AddListener(GoldPopup);
            }

            private void OnDisable()
            {
                QuestManager.OnGoldPickedUp.RemoveListener(GoldPopup);

                if (playerController)
                {
                    playerController.PlayerAttackBehaviour.OnKill.RemoveListener(KillPopup);
                    playerController.PlayerAttackBehaviour.OnWaveKill.RemoveListener(KillPopup);
                }
            }
#endregion

#region Public Functions
            public void SetupBehaviour(PlayerController _playerController)
            {
                playerController = _playerController;
                
                playerController.PlayerAttackBehaviour.OnKill.AddListener(KillPopup);
                playerController.PlayerAttackBehaviour.OnWaveKill.AddListener(KillPopup);
                
                activePopups = new Dictionary<Sprite, PlayerPopupUI>();
                
                personalObjectiveCanvas.SetActive(false);
                heldItemCanvas.SetActive(false);
            }

            public void UpdatePersonalObjective(int _totalAmount, int _addedAmount)
            {
                personalObjectiveText.text = _totalAmount.ToString();
                
                if (_addedAmount > 0)
                {
                    PersonalObjectivePopup(_addedAmount);
                }
            }

            public void SetHeldItemSprite(Sprite _sprite)
            {
                heldItemImage.sprite = _sprite;
            }

            public void ToggleHeldItemUI(bool _active)
            {
                heldItemCanvas.SetActive(_active);
            }

            public void ToggleOverheadStatsUI(bool _active)
            {
                personalObjectiveCanvas.SetActive(_active);
            }
#endregion

#region Private Functions
            private void Popup(int _amount, Sprite _sprite)
            {
                if (activePopups.ContainsKey(_sprite))
                {
                    if (activePopups[_sprite] != null)
                    {
                        activePopups[_sprite].SetUp(_amount, _sprite);
                        return;
                    }
                }
                
                PlayerPopupUI _playerPopupUI = Instantiate(popupPrefab, popupCanvas.transform).GetComponent<PlayerPopupUI>();
                _playerPopupUI.SetUp(_amount, _sprite);
                LayoutRebuilder.ForceRebuildLayoutImmediate(popupCanvas.GetComponent<RectTransform>());
                activePopups[_sprite] = _playerPopupUI;
            }

            private void KillPopup()
            {
                Popup(1, popupKillSprite);
            }

            private void GoldPopup(int _playerIndex, int _amount)
            {
                if (_playerIndex == playerController.PlayerIndex)
                {
                    Popup(_amount, popupGoldSprite);
                }
            }

            private void PersonalObjectivePopup(int _amount)
            {
                Popup(_amount, popupPersonalObjectiveSprite);
            }
#endregion
        }
    }
}