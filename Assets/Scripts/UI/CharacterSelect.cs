
// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/05
// Author: Fredrik
// Description: Character Selection for players
// --------------------------------
// ------------------------------*/


using System;
using System.Collections.Generic;
using System.Linq;
using Game.Backend;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game
{
    namespace UI
    {
        public class CharacterSelect : MonoBehaviour
        {
            public Image Image;
            public bool PlayersIsReady;
            public bool BeginGame;
            [HideInInspector] public PlayerInput playerInputs;
            private int id;
            
            
            private Sprite cachedSprite;
            private int cachedId = 10;
            private float inputDelay;
            private PlayerData data;
            // Start is called before the first frame update

            private CharacterSelectHandler characterSelectHandler;
            #region Unity functions

            public void Start()
            {
                characterSelectHandler = FindObjectOfType<CharacterSelectHandler>();
                
                playerInputs = GetComponent<PlayerInput>();
                Image.sprite = characterSelectHandler.Images[0];
                for (int i = 0; i <  characterSelectHandler.Datas.Count-1; i++)
                {
                    if (playerInputs.user.index == i)
                    {
                        data =  characterSelectHandler.Datas[i];
                    }
                }
                SetPlayerImagePosition();
                
                inputDelay = 0.01f;
                cachedId = id;

            }

            #endregion

            public void Update()    
            {

               PlayerBlurOut();
            }
            
            

            #region Public

            public void OnNavigation(InputAction.CallbackContext context)
            {
                int amountOfImages =  characterSelectHandler.ImagesBackup.Count;
                Debug.Log(amountOfImages);
                if (PlayersIsReady) return;
                Vector2 value = context.ReadValue<Vector2>();
                switch (value.x)
                {
                    case > 0:
                    {
                        inputDelay -= Time.deltaTime;
                        if (inputDelay <0)
                        {
                            id++;
                            inputDelay = 0.01f;
                        }
                        break;
                    }
                    case < 0:
                    {
                        inputDelay -= Time.deltaTime;
                        if(inputDelay <0)
                        {
                            id--;
                            inputDelay = 0.01f; 
                        }

                        break;
                    }
                }

                id = Wrap(id, 0, 4);
                if (id < 0) id += amountOfImages;
                if (id > 3) id -= amountOfImages;

                if ( characterSelectHandler.ImagesBackup.TryGetValue(id,out Sprite sprite))
                {
                    Image.sprite = sprite;
                    cachedId = id;
                }
                

            }

            public void OnConfirm(InputAction.CallbackContext context)
            {
                if ((!context.performed || PlayersIsReady)) return;
                if (characterSelectHandler.Images.TryGetValue(id, out Sprite sprite))
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                        data.CharacterID= id;
                        cachedId = id;
                        cachedSprite = sprite;
                        characterSelectHandler.Images.Remove(id);
                    }
                    PlayersIsReady = true;
                }
                else
                {
                    
                    Debug.Log("Player is already Taken");
                }
                
            }

            #endregion

            public void OnCancel(InputAction.CallbackContext context)
            {
                if (!context.performed || !PlayersIsReady) return;
                
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                    characterSelectHandler.Images.Add(cachedId,cachedSprite);
                }
                
                PlayersIsReady = false;

            }
            
            #region Private

            private int Wrap(int Target, int LowerBounds, int UpperBounds)
            {
                int dif = UpperBounds - LowerBounds;

                if (Target > LowerBounds)
                {
                    return LowerBounds + (Target - LowerBounds) % dif;
                }

                Target = LowerBounds + (LowerBounds - Target);
                int tempVal = LowerBounds + (Target - LowerBounds) % dif;
                return UpperBounds - tempVal;
            }

            private void SetPlayerImagePosition()
            {
                int userIndex = playerInputs.user.index;
                Transform targetTransform = characterSelectHandler.imagePosition[userIndex];

                transform.SetParent(targetTransform);
                transform.position = targetTransform.position;

                int childCount = transform.parent.childCount;

                for (int i = 0; i < childCount; i++)
                {
                    Transform childTransform = transform.parent.GetChild(0);
                    childTransform.gameObject.SetActive(false);
                }
            }

            private void PlayerBlurOut()
            { 
                if (!characterSelectHandler.Images.TryGetValue(cachedId, out Sprite sprite))
                {
                  
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
                else
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            
            
            #endregion
            
        }
    }
}
