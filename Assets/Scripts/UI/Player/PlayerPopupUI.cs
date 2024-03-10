// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-28
// Author: b22alesj
// Description: Player popup UI
// --------------------------------
// ------------------------------*/

using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Game {
    namespace Player {
        public class PlayerPopupUI : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private TextMeshProUGUI popupText;
            [SerializeField] private Image popupIcon;
            
            [Header("Settings")]
            [SerializeField] private float showTime;
            [SerializeField] private float fadeTime;
            
            private int currentValue;
            private float currentShowTime;
            private float currentFadeFloat;

            public void SetUp(int _amount, Sprite _sprite)
            {
                currentValue += _amount;
                currentShowTime = 0;
                
                popupText.text = "+" + currentValue;
                popupIcon.sprite = _sprite;
            }

            private void Awake()
            {
                popupText.color = new Color(popupText.color.r, popupText.color.g, popupText.color.b, 0);
                popupIcon.color = new Color(popupIcon.color.r, popupIcon.color.g, popupIcon.color.b, 0);
            }

            private void Update()
            {
                // Fade in
                if (currentFadeFloat < 1 && currentShowTime < showTime)
                {
                    currentFadeFloat += Time.deltaTime / fadeTime;
                    popupText.color = new Color(popupText.color.r, popupText.color.g, popupText.color.b, currentFadeFloat);
                    popupIcon.color = new Color(popupIcon.color.r, popupIcon.color.g, popupIcon.color.b, currentFadeFloat);
                }
                // Fade out
                else if (currentFadeFloat > 0 && currentShowTime >= showTime)
                {
                    currentFadeFloat -= Time.deltaTime / fadeTime;
                    popupText.color = new Color(popupText.color.r, popupText.color.g, popupText.color.b, currentFadeFloat);
                    popupIcon.color = new Color(popupIcon.color.r, popupIcon.color.g, popupIcon.color.b, currentFadeFloat);
                    
                    if (currentFadeFloat <= 0)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    currentShowTime += Time.deltaTime;
                }
            }
        }
    }
}