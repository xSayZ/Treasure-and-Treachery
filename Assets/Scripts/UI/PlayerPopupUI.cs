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
            
            private int currentValue;
            private float currentShowTime;

            public void SetUp(int _amount, Sprite _sprite)
            {
                currentValue += _amount;
                currentShowTime = 0;
                
                popupText.text = "+" + currentValue;
                popupIcon.sprite = _sprite;
            }

            private void Update()
            {
                if (currentShowTime >= showTime)
                {
                    Destroy(gameObject);
                }
                else
                {
                    currentShowTime += Time.deltaTime;
                }
            }
        }
    }
}