// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: c21frejo
// Description: Introsekvens
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
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
            [SerializeField]private List<Sprite> introImages;

            [SerializeField,Tooltip("Set Scene that will load after load")]
            private string scene;
            //public Transform endPoint;
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
                introImages = Bank.IntroImages;
                currentIntroImage.sprite = Bank.IntroImages[0];
                currentTime = timeBeforeChange;
                //StartCoroutine(LerpPosition(endPoint.position, timeBeforeChange));
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

            #region Private Functions
            
            /*
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
            }*/
            
            private void ChangeImage()
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0 && index < introImages.Count-1 )
                {
                    currentTime = timeBeforeChange;
                    index++;
                    currentIntroImage.sprite = introImages[index];
                }

                if (index == introImages.Count - 1)
                {
                    elapsedTime += Time.deltaTime;
                    done = true;
                }
            }

            private void ChangeScene()
            {
                if (done && elapsedTime >2)
                {
                    SceneManager.LoadScene(scene);
                }
            }

           
            
            #endregion
        }
    }
}