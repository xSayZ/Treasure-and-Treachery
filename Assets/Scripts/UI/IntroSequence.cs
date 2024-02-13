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
            
            private int index;
            #region Unity Functions

            // Start is called before the first frame update
            void Start()
            {
                introImages = Bank.IntroImages;
                currentIntroImage.sprite = Bank.IntroImages[0];
                currentTime = timeBeforeChange;
            }

            // Update is called once per frame
            void Update()
            {
                //ChangeImage();
                ImageInterpolation();
            }

            #endregion

            #region Public Functions

            #endregion

            #region Private Functions

            private void ImageInterpolation()
            {
                currentIntroImage.transform.position = Vector3.LerpUnclamped(currentIntroImage.transform.position,endPoint.position,timeBeforeChange*Time.deltaTime);
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
                
            }

           
            
            #endregion
        }
    }
}