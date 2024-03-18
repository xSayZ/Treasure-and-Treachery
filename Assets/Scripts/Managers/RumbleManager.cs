// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-09
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;


namespace Game
{
    namespace Backend
    {
        public class RumbleManager : Singleton<RumbleManager>
        {
            #region Public Functions

            public void RumblePulse(float _lowFrequency, float _highFrequency, float _duration, InputDevice _device)
            {
                if (_device is Gamepad _gamepad)
                {
                    _gamepad.SetMotorSpeeds(_lowFrequency, _highFrequency);
                    StartCoroutine(StopRumble(_duration, _gamepad));
                }
            }
            #endregion

            #region Private Functions

            private static IEnumerator StopRumble(float _duration, IDualMotorRumble _gamepad)
            {
                float _elapsedTime = 0f;

                while (_elapsedTime < _duration)
                {
                    _elapsedTime += Time.deltaTime;
                    yield return null;
                }
                
                _gamepad.SetMotorSpeeds(0f, 0f);
            }

            #endregion
        }
    }
}