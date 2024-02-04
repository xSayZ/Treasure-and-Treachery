// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-02
// Author: Felix
// Description: Timer Script
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Backend {
        public class Timer : MonoBehaviour
        {
            private float timerDuration;
            private float elapsedTime;
            
            private bool isTimerRunning;

            // Constructor to set the timer duration
            public Timer (float _duration) {
                timerDuration = _duration;
            }
            
        

#region Unity Functions
    
            // Update is called once per frame
            void Update()
            {
                if(isTimerRunning) {
                    elapsedTime += Time.deltaTime;
                } if (elapsedTime >= timerDuration) {
                    isTimerRunning = false;
                }

                Debug.Log(timerDuration);
            }
#endregion

#region Public Functions
            public void StartTimer(){
                if(!isTimerRunning) {
                    elapsedTime = 0f;
                    isTimerRunning = true;
                }
            }
#endregion

        }
    }
}
