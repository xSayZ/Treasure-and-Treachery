// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: c21frejo
// Description: Introsekvens
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Audio;
using Game.Backend;
using Game.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Game
{
    namespace UI
    {
        public class IntroSequence : MonoBehaviour
        {
            [SerializeField] private ImageBank Bank;
            [SerializeField] private Image currentIntroImage;

            [SerializeField] private PlayerInput Input;

            //public Transform endPoint;
            [Header("Timer Before next intro slide")]
            public float timeBeforeChange;

            private float currentTime;
            private float elapsedTime;
            private int index;
            private bool done;
            private int stopUpdating;

            private bool skiped;

            #region Unity Functions

            // Start is called before the first frame update
            void Start()
            {
                stopUpdating = 0;
                elapsedTime = 0;
                currentIntroImage.sprite = Bank.IntroImages[0];
                currentTime = timeBeforeChange;
            }

            // Update is called once per frame
            void Update()
            {
                ChangeImage();
                ChangeScene();

                if (skiped)
                {
                    skiped = false;
                }
            }

            #endregion

            public void OnSkip(InputAction.CallbackContext context)
            {
                if (context.action.WasPerformedThisFrame())
                {
                    skiped = true;
                }

                if (context.action.WasReleasedThisFrame())
                    skiped = false;
            }
            
            #region Private Functionn

            private void ChangeImage()
            {
                currentTime -= Time.deltaTime;
                if (((currentTime <= 0) || skiped) && index < Bank.IntroImages.Count - 1)
                {
                    currentTime = timeBeforeChange;
                    index++;
                    currentIntroImage.sprite = Bank.IntroImages[index];
                }

                if (index == Bank.IntroImages.Count - 1 && !done)
                {
                    elapsedTime += Time.deltaTime;
                    done = true;
                    skiped = false;
                }
            }

            private void ChangeScene()
            {
                if (done && elapsedTime > timeBeforeChange && stopUpdating < 1 || (done && skiped))
                {
                    stopUpdating++;
                    LevelManager.Instance.LoadScene(1);
                }
            }

            #endregion
        }
    }
}