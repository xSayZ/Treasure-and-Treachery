// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-06
// Author: b22alesj
// Description: Updates progress bar
// --------------------------------
// ------------------------------*/

using UnityEngine;
using UnityEngine.UI;


namespace Game {
    namespace UI {
        public class ProgressBar : MonoBehaviour
        {
            [SerializeField] private Slider slider;
            
            public void SetProgress(float _progress)
            {
                slider.value = _progress;
            }
        }
    }
}
