// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: b22alesj
// Description: UI for quests
// --------------------------------
// ------------------------------*/

using TMPro;
using UnityEngine;


namespace Game {
    namespace UI {
        public class QuestUI : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private TextMeshProUGUI questText;

            [Header("Settings")]
            [SerializeField] private string startingText;
            [SerializeField] private float startingShowTime;
            
            private float showTimeLeft;
            
#region Unity Functions
            private void Start()
            {
                SetText(startingText, startingShowTime);
            }
            
            private void Update()
            {
                if (showTimeLeft <= 0)
                {
                    questText.gameObject.SetActive(false);
                }
                else
                {
                    showTimeLeft -= Time.deltaTime;
                }
            }
#endregion

#region Public Functions
            public void SetText(string _text, float _showTime)
            {
                questText.text = _text;
                showTimeLeft = _showTime;
                questText.gameObject.SetActive(true);
            }
#endregion
        }
    }
}
