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


namespace Game
{
    namespace NAME
    {
        public class RumbleManager : Singleton<RumbleManager>

        {
            private Gamepad gamePad;

            private Coroutine stopRumbleAfterTimeCoroutine;

            #region Unity Functions

            // Start is called before the first frame update
            void Start()
            {
            }

            // Update is called once per frame
            void Update()
            {
            }

            #endregion

            #region Public Functions

            public void RumblePulse(float lowFrequency, float highFrequency, float duration)
            {
                gamePad = Gamepad.current;
                if (gamePad == null) return;

                gamePad.SetMotorSpeeds(lowFrequency, highFrequency);

                StartCoroutine(StopRumble(duration, gamePad));
            }

            #endregion

            #region Private Functions

            private IEnumerator StopRumble(float duration, Gamepad pad)
            {
                float elapsedTime = 0f;

                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                pad.SetMotorSpeeds(0f, 0f);
            }

            #endregion
        }
    }
}