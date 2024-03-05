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
            
            
#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
               
            }
    
            // Update is called once per frame
            void Update()
            {
                timer = GameManager.Instance.timer.GetCurrentTime();
                // Debug.Log("time is" + timer);

                
                if (timer >= 5)
                {
                   AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic,"MusicProg", 2f, false, false);
                }
                
                if (timer >= 10)
                {
                    
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic,"MusicProg", 3f, false, false);
                    
                }
                

                if (timer >= 15)
                {
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic,"MusicProg", 4f, false, false);
                }
                

                if (timer >= 20)
                {
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic,"MusicProg", 5f, false, false);
                }
                
                
                if (GameManager.Instance.ActivePlayerControllers.Count == 0)
                {
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic, "MusicProg", 1f,false,false);
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
