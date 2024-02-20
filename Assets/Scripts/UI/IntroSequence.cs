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

            //public Transform endPoint;
            [Header("Timer Before next intro slide")] 
            public float timeBeforeChange;
            private float currentTime;
            private float elapsedTime;
            private int index;
            private bool done;

            #region Unity Functions

            // Start is called before the first frame update
            void Start()
            {
                elapsedTime = 0;
                currentIntroImage.sprite = Bank.IntroImages[0];
                currentTime = timeBeforeChange;
            }

            // Update is called once per frame
            void Update()
            {
                ChangeImage();
                ChangeScene();
            }

            #endregion

            #region Public Functions

            #endregion

            #region Private Functionn
            
            private void ChangeImage()
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0 && index < Bank.IntroImages.Count-1 )
                {
                    currentTime = timeBeforeChange;
                    index++;
                    currentIntroImage.sprite = Bank.IntroImages[index];
                }

                if (index == Bank.IntroImages.Count - 1)
                {
                    elapsedTime += Time.deltaTime;
                    done = true;
                }
            }

            private void ChangeScene()
            {
                if (done && elapsedTime >timeBeforeChange)
                {
                    CustomSceneManager.Instance.ChangeScene();
                }
            }

           
            
            #endregion
        }
    }
}