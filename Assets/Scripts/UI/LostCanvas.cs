// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-04
// Author: b22alesj
// Description: Script for lose UI
// --------------------------------
// ------------------------------*/

using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Game {
    namespace Scenes {
        public class LostCanvas : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private TextMeshProUGUI lostReasonText;
            [SerializeField] private Button retryButton;
            
            private bool hasRetried;

            public void Setup(string _reason)
            {
                lostReasonText.text = _reason.Trim();
            }

            public void RetryLevel()
            {
                if (!hasRetried)
                {
                    hasRetried = true;
                    
                    // Triggers button pressed color
                    ExecuteEvents.Execute(retryButton.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
                    
                    LevelManager.Instance.ReloadLevel();
                }
            }
        }
    }
}