// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-06
// Author: b22alesj
// Description: Updates progress bar
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace UI {
        public class ProgressBar : MonoBehaviour
        {
            [SerializeField] private RectTransform progressTransform;
            [SerializeField] private float progressWidth;
            
            public void SetProgress(float _progress)
            {
                progressTransform.sizeDelta = new Vector2(_progress * progressWidth, progressTransform.sizeDelta.y);
            }
        }
    }
}
