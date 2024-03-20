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
            private float time;
            private float roundTime;
            private bool setupComplete = false;
            private EventsToBePlayed eventsToBePlayed;
            
            private GameManager gameManager;
            
#region Unity Functions
            
            public void Setup(GameManager _gameManager)
            {
                gameManager = _gameManager;
                roundTime = gameManager.roundTime;
                setupComplete = true;
            }
            
            void Update()
            {
                if (useTimer && setupComplete) {
                    SetParamByTime();
                }

            }
#endregion

#region Public Functions

#endregion

#region Private Functions

        private void SetParamByTime()
        {
            time = GameManager.Instance.timer.GetCurrentTime();
                
            // Set the parameter of the music event based on the timer
            int[] _thresholds = { (int)(roundTime / 2.5), (int)(roundTime / 2), (int)(roundTime / 1.5), (int)(roundTime / 1.2)};
            float[] _musicParameters = { 2f, 3f, 4f, 5f };
            
            // Set the parameter of the music event based on the timer
            for (int i = 0; i < _thresholds.Length; i++)
            {
                if (time >= _thresholds[i])
                {
                    //Debug.Log("Setting parameter to: " + _musicParameters[i]);
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
