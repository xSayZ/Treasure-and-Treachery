// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections;
using System.Collections.Generic;
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
            public ImageBank Bank;
            public Image currentIntroImage;
            public List<Sprite> introImages;

            public Transform endPoint;
            public float timeBeforeChange;
            private float currentTime;

            private float elapsedTime;
            
            private int index;
            #region Unity Functions

            // Start is called before the first frame update
            void Start()
            {
                introImages = Bank.IntroImages;
                currentIntroImage.sprite = Bank.IntroImages[0];
                currentTime = timeBeforeChange;
                StartCoroutine(LerpPosition(endPoint.position, timeBeforeChange));
            }

            // Update is called once per frame
            void Update()
            {
                
                ChangeScene();
            }

            #endregion

            #region Public Functions

            #endregion

            #region Private Functions
            
            
            private IEnumerator LerpPosition(Vector3 targetPosition, float duration)
            {
                float time = 0;
                Vector3 startPosition = currentIntroImage.transform.position;

                while (time < duration)
                {
                    currentIntroImage.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
                    time += Time.deltaTime;
                    yield return null;
                }
                currentIntroImage.transform.position = targetPosition;
            }
            
            /*private void ChangeImage()
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0 && index < introImages.Count-1 )
                {
                    currentTime = timeBeforeChange;
                    index++;
                    currentIntroImage.sprite = introImages[index];
                }
            }*/

            private void ChangeScene()
            {
                if (currentIntroImage.transform.position == endPoint.position)
                {
                    // hårdKodad scene
                    SceneManager.LoadScene("Official Test Scene 1");
                }
            }

           
            
            #endregion
        }
    }
}