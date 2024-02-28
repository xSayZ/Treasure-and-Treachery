// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-28
// Author: b22alesj
// Description: Controls player overhead UI
// --------------------------------
// ------------------------------*/

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
            [SerializeField] private TextMeshProUGUI popupText;
            [SerializeField] private Image popupIcon;
            
            [Header("Held Item")]
            [SerializeField] private GameObject heldItemCanvas;
            [SerializeField] private Image heldItemImage;
            
            private PlayerController playerController;

            public void SetupBehaviour(PlayerController _playerController)
            {
                playerController = _playerController;
                personalObjectiveCanvas.SetActive(false);
                popupCanvas.SetActive(false);
                heldItemCanvas.SetActive(false);
                
                // Set personal objective text (or maybe do it in ability instead)
            }

            public void UpdatePersonalObjective(string _text)
            {
                personalObjectiveText.text = _text;
            }

            public void Popup(string _text, Sprite _sprite)
            {
                popupText.text = _text;
                popupIcon.sprite = _sprite;
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
        }
    }
}