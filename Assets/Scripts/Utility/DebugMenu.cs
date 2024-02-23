// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-22
// Author: b22alesj
// Description: Debug menu
// --------------------------------
// ------------------------------*/

using TMPro;
using UnityEngine;


namespace Utility {
    namespace Debugging {
        public class DebugMenu : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private GameObject debugPanel;
            [SerializeField] private TextMeshProUGUI fpsText;
            
            private bool isActive;
            private float[] frameTimes;
            private int frameIndex;

            private void Awake()
            {
                if (FindObjectsByType<DebugMenu>(FindObjectsSortMode.None).Length > 1)
                {
                    Destroy(gameObject);
                }
                
                DontDestroyOnLoad(gameObject);
                debugPanel.SetActive(false);
                frameTimes = new float[250];
            }

            private void Update()
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    isActive = !isActive;
                    debugPanel.SetActive(isActive);
                }
                
                frameTimes[frameIndex] = Time.deltaTime;
                frameIndex = (frameIndex + 1) % frameTimes.Length;
                
                if (isActive)
                {
                    float totalFrameTime = 0f;
                    foreach (float frameTime in frameTimes)
                    {
                        totalFrameTime += frameTime;
                    }
                    
                    fpsText.text = (frameTimes.Length / totalFrameTime).ToString().Split(",")[0];
                }
            }
        }
    }
}