// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-22
// Author: b22alesj
// Description: Debug menu
// --------------------------------
// ------------------------------*/

using System.Linq;
using TMPro;
using UnityEngine;


namespace Utility {
    namespace Debugging {
        public class DebugMenu : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private GameObject debugPanel;
            [SerializeField] private TextMeshProUGUI MinFPSText;
            [SerializeField] private TextMeshProUGUI AvgFPSText;
            [SerializeField] private TextMeshProUGUI MaxFPSText;
            [SerializeField] private TextMeshProUGUI MinFrameTimeText;
            [SerializeField] private TextMeshProUGUI AvgFrameTimeText;
            [SerializeField] private TextMeshProUGUI MaxFrameTimeText;
            
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
                frameTimes = new float[500];
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

                    MinFPSText.text = (1f / frameTimes.Max()).ToString("000");
                    AvgFPSText.text = (frameTimes.Length / totalFrameTime).ToString("000");
                    MaxFPSText.text = (1f / frameTimes.Min()).ToString("000");
                    
                    MinFrameTimeText.text = (frameTimes.Min() * 1000).ToString("G3");
                    AvgFrameTimeText.text = (totalFrameTime / frameTimes.Length * 1000).ToString("G3");
                    MaxFrameTimeText.text = (frameTimes.Max() * 1000).ToString("G3");
                    
                }
            }
        }
    }
}