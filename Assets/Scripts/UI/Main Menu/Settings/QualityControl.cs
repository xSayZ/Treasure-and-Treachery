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

            [SerializeField] private Button[] _switchQualityButtons;
            [SerializeField] private Button[] _qualityButtons;
            [SerializeField] private Button[] _fullscreenButtons;
            
            [SerializeField] private Color unselectedColor;
            [SerializeField] private Color selectedColor;
            
            
            
            public void ToggleFullscreen(bool _fullscreen) {
                Screen.fullScreen = _fullscreen;
            }
            
            public void SwitchQualityLevel(int _qualityLevel) {
                QualitySettings.SetQualityLevel(_qualityLevel);
            }
            
            public void SwitchQualityDynamicallyLevel(bool useDynamicQuality) {
                if (useDynamicQuality) {
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
            }
        }
    }
}
