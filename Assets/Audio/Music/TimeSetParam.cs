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
            private bool setupComplete;
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
            int currentThreshold = CalculateCurrentThreshold(time);
            float[] _musicParameters = { 2f, 3f, 4f, 5f };
            
            switch (currentThreshold)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic, "MusicProg", _musicParameters[currentThreshold], false, false);
                    break;
                default:
                    break;
            }

            if (GameManager.Instance.ActivePlayerControllers.Count == 0)
            {
                AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic, "MusicProg", 1f, false, false);
            }
        }
        
        private int CalculateCurrentThreshold(float time)
        {
            int[] _thresholds = { Mathf.RoundToInt(roundTime * 0.25f), Mathf.RoundToInt(roundTime * 0.5f), Mathf.RoundToInt(roundTime * 0.75f), Mathf.RoundToInt(roundTime)};
            for (int i = 0; i < _thresholds.Length; i++)
            {
                if (time >= _thresholds[i])
                {
                    return i;
                }
            }
            return -1;
        }
#endregion
        }
    }
}
