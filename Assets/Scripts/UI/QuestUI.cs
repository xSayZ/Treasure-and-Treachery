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
            [SerializeField] private float showTime;
            [SerializeField] private string startingText;
            
            private float showTimeLeft;

#region Unity Functions
            private void Start()
            {
                DisplayText(startingText);
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
            public void DisplayText(string _text)
            {
                questText.text = _text;
                showTimeLeft = showTime;
                questText.gameObject.SetActive(true);
            }
#endregion
        }
    }
}
