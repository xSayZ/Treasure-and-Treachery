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
            [SerializeField] private TextMeshProUGUI mainQuestText;
            [SerializeField] private Animator mainSpriteAnimator;
            [SerializeField] private Animator mainMaskAnimator;
            
            [Header("Settings")]
            [SerializeField] private float mainShowTime;
            [SerializeField] private string startingText;
            
            private float mainShowTimeLeft;
            private bool mainShowRunning;

#region Unity Functions
            private void Start()
            {
                DisplayMainScroll();
            }

            private void Update()
            {
                if (mainShowTimeLeft <= 0 && mainShowRunning)
                {
                    mainSpriteAnimator.SetTrigger("Close");
                    mainMaskAnimator.SetTrigger("Close");
                    mainShowRunning = false;
                }
                else if (mainShowRunning)
                {
                    mainShowTimeLeft -= Time.deltaTime;
                }
            }
#endregion

#region Public Functions
            public void UpdateSideScroll(string _text)
            {
                
            }
#endregion

#region Private Functions
            private void DisplayMainScroll()
            {
                mainQuestText.text = startingText;
                mainShowTimeLeft = mainShowTime;
                
                mainSpriteAnimator.SetTrigger("Open");
                mainMaskAnimator.SetTrigger("Open");
                
                mainShowRunning = true;
            }
#endregion
        }
    }
}