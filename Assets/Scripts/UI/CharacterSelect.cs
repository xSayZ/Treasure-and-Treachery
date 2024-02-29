
// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/05
// Author: Fredrik
// Description: Character Selection for players
// --------------------------------
// ------------------------------*/

using System;
using Game.Backend;
using Game.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Game
{
    namespace UI
    {
        public class CharacterSelect : MonoBehaviour
        {
            [SerializeField] private Image Image;
            [SerializeField] private Image arrowImages;
            [SerializeField] private float inputDelay;
            
            public bool PlayersIsReady { get; private set; }
            [HideInInspector] public PlayerInput playerInputs;
            private int id;
            
            private Sprite cachedSprite;
            private int cachedId = 10;
            public float currentDelay;
            public  static PlayerData data;
            
            private CharacterSelectHandler characterSelectHandler;
            #region Unity functions

            private void Awake()
            {
                characterSelectHandler = FindObjectOfType<CharacterSelectHandler>();

            }

            private void Start()
            {
                currentDelay = inputDelay;
                playerInputs = GetComponent<PlayerInput>();

                Image.sprite = characterSelectHandler.ImagesBackup[0];
                for (int i = 0; i <  characterSelectHandler.Datas.Count-1; i++)
                {
                    if (playerInputs.playerIndex == i)
                    {
                        data =  characterSelectHandler.Datas[i];
                        data.ControllerID = playerInputs.playerIndex;
                    }
                    
                }
                SetPlayerImagePosition();
                cachedId = id;
                
            }
            
            private void Update()    
            {
                PlayerBlurOut();
            }
            #endregion

            #region Inputs
            
            public void OnNavigation(InputAction.CallbackContext context)
            {
                if (PlayersIsReady) return;
                int amountOfImages =  characterSelectHandler.ImagesBackup.Count;

                Vector2 value = context.ReadValue<Vector2>();
                
                switch (value.x)
                {
                    
                    case > .9f:
                    {
                        currentDelay -= 0.005f;
                        if (currentDelay <0)
                        {
                            id++;
                            currentDelay = inputDelay;
                        }
                        break;
                    }
                    case < -0.9f:
                    {
                        currentDelay -= 0.005f;
                        if(currentDelay <0)
                        {
                            id--;  
                            currentDelay = inputDelay;
                        }
                        break;
                    }
                }
                
                id = Wrap(id, 0, 4);
                if (id < 0) id += amountOfImages;
                if (id > 3) id -= amountOfImages;

                if (!characterSelectHandler.ImagesBackup.TryGetValue(id, out Sprite sprite)) return;
                    Image.sprite = sprite;
                    cachedId = id;
            }

            public void OnConfirm(InputAction.CallbackContext context)
            {
                if (characterSelectHandler.BeginGame && context.action.WasPerformedThisFrame())
                {
                    LevelManager.Instance.LoadLoadingScreen(characterSelectHandler.TestLevel);
                }
                
                if ((!context.action.WasPerformedThisFrame() || PlayersIsReady)) return;
                if (characterSelectHandler.Images.TryGetValue(id, out Sprite sprite))
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                        data = characterSelectHandler.Datas[id];
                        data.playerIndex = id;
                        data.ControllerID = playerInputs.playerIndex;
                        characterSelectHandler.SelectedData.Add(data);
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

            public void OnCancel(InputAction.CallbackContext context)
            {
                if (!context.performed || !PlayersIsReady) return;
                
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                    characterSelectHandler.Images.Add(cachedId,cachedSprite);
                    characterSelectHandler.SelectedData.Remove(data);

                }
                
                PlayersIsReady = false;

            }
            #endregion
            
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
                Debug.Log(playerInputs.playerIndex);
                Transform targetTransform = characterSelectHandler.imagePosition[playerInputs.playerIndex];
                transform.SetParent(targetTransform);
                transform.position = targetTransform.position;
                transform.rotation = targetTransform.rotation;

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
