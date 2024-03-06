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
        public class TimeSetParam : MonoBehaviour {
            [SerializeField] private bool useTimer = true;
            private float timer;
            private EventsToBePlayed eventsToBePlayed;
            
#region Unity Functions
            void Update()
            {
                if (useTimer) {
                    SetParamByTime();
                }

            }
#endregion

#region Public Functions

#endregion

#region Private Functions

        private void SetParamByTime()
        {
            timer = GameManager.Instance.timer.GetCurrentTime();
                
            // Set the parameter of the music event based on the timer
            int[] _thresholds = { 60, 120, 180, 240 };
            float[] _musicParameters = { 2f, 3f, 4f, 5f };

            // Set the parameter of the music event based on the timer
            for (int i = 0; i < _thresholds.Length; i++)
            {
                if (timer >= _thresholds[i])
                {
                    // Set the parameter of the music event
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic, "MusicProg", _musicParameters[i], false, false);
                }
            }

            if (GameManager.Instance.ActivePlayerControllers.Count == 0)
            {
                AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic, "MusicProg", 1f, false, false);
            }
        }
#endregion
        }
    }
}
