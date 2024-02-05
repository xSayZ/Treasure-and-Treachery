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
            
#region Unity Functions
    
            // Update is called once per frame
            void Update()
            {
                if(isTimerRunning) {
                    elapsedTime += Time.deltaTime;
                } if (elapsedTime >= timerDuration) {
                    isTimerRunning = false;
                }

                Debug.Log(elapsedTime);
            }
#endregion

#region Public Functions
            public void StartTimer(float _duration){
                if(!isTimerRunning) {
                    timerDuration = _duration;
                    elapsedTime = 0f;
                    isTimerRunning = true;
                }
            }
            
            public void StopTimer(){
                isTimerRunning = false;
            }
            
            public float GetCurrentTime()
            {
                return elapsedTime;
            }
#endregion

        }
    }
}
