// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-18
// Author: b22feldy
// Description: Treasure and Treachery
// --------------------------------
// ------------------------------*/

using UnityEngine;
using UnityEngine.UI;


namespace Game {
    namespace UI {
        public class QualityControl : MonoBehaviour {

            private bool dynamicQualityUsed;
            public void ToggleFullscreen(bool _fullscreen) {
                Screen.fullScreen = _fullscreen;
            }
            
            public void SwitchQualityLevel(int _qualityLevel) {
                if (_qualityLevel == QualitySettings.GetQualityLevel()) return;
                if (dynamicQualityUsed) return;
                
                QualitySettings.SetQualityLevel(_qualityLevel);
                Debug.Log("Quality Level: " + _qualityLevel);
            }
            
            public void SwitchQualityDynamicallyLevel(bool _useDynamicQuality) {
                dynamicQualityUsed = _useDynamicQuality;
                if (_useDynamicQuality) {
                    switch (SystemInfo.graphicsMemorySize) {
                        case <= 1024:
                            QualitySettings.SetQualityLevel(0);
                            break;
                        case <= 2048:
                            QualitySettings.SetQualityLevel(1); ;
                            break;
                        case <= 2560:
                            QualitySettings.SetQualityLevel(2);
                            break;
                        case <= 3072:
                            QualitySettings.SetQualityLevel(3);
                            break;
                        case <= 4096:
                            QualitySettings.SetQualityLevel(4);
                            break;
                        default:
                            QualitySettings.SetQualityLevel(3);
                            break;
                    }
                }
                else {
                    QualitySettings.SetQualityLevel(3);
                }
                
                Debug.Log("Dynamic Quality Level: " + QualitySettings.GetQualityLevel());
            }
        }
    }
}
