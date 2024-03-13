using Game.Backend;
using UnityEngine;

namespace Game {
    namespace UI {
        public class LevelTimer : MonoBehaviour {
            public TMPro.TextMeshProUGUI text;
            private Utility.Timer timer;

            private void Update() {
                timer = GameManager.Instance.timer;
                
                if (timer != null && timer.GetCurrentTime() < GameManager.Instance.roundTime) {
                    float remainingTime = GameManager.Instance.roundTime - timer.GetCurrentTime();
                    int minutes = Mathf.FloorToInt(remainingTime / 60F);
                    int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);
                    string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
                    
                    text.text = niceTime;
                } else {
                    text.text = "0:00";
                }
            }
        }
    }
}
