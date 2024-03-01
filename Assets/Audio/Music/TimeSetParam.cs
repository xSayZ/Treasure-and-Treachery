// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-12
// Author: johan
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using FMOD.Studio;
using FMODUnity;
using Game.Backend;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System;


namespace Game {
    namespace Audio {
        public class TimeSetParam : MonoBehaviour
        {

            private float timer;
            private EventsToBePlayed eventsToBePlayed;
            private bool isPlaying = false;
            
#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                isPlaying = false;
            }
    
            // Update is called once per frame
            void Update()
            {
                timer = GameManager.Instance.timer.GetCurrentTime();
                // Debug.Log("time is" + timer);

                
                if (timer >= 60)
                {
                    if (!isPlaying)
                    {
                       // AudioMananger.Instance.WaveStinger(EventsToBePlayed.TimedStinger);
                        isPlaying = true;
                    }
                   // Debug.Log("played once");
                   // AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic,"MusicProg", 2f, false, false);
                    
                    
                }
                
                if (timer >= 120)
                {
                    
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic,"MusicProg", 3f, false, false);
                    
                }
                

                if (timer >= 180)
                {
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic,"MusicProg", 4f, false, false);
                }
                

                if (timer >= 240)
                {
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic,"MusicProg", 5f, false, false);
                }
                
                
                
            }
#endregion

#region Public Functions

#endregion

#region Private Functions

        private void SetParamByTime()
        {
            


        }

#endregion
        }
    }
}
