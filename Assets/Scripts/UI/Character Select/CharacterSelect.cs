
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
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Game
{
    namespace UI
    {
        public class CharacterSelect : MonoBehaviour
        {
            [Header("Player Settings")]
            [SerializeField] private Image Image;
            [SerializeField] private float inputDelay;
            public bool PlayersIsReady { get; private set; }
            [HideInInspector] public PlayerInput playerInput;
            private int id;
            
            private Sprite cachedSprite;
            private int cachedId = 10;
            public float currentDelay;
            public  PlayerData data;

            public int deviceID;
            private CharacterSelectHandler characterSelectHandler;
            #region Unity functions

            private void Awake()
            {
                characterSelectHandler = FindObjectOfType<CharacterSelectHandler>();
            }

            private void Start()
            {
                currentDelay = inputDelay;
                playerInput = GetComponent<PlayerInput>();

                Image.sprite = characterSelectHandler.ImagesBackup[0];
                for (int i = 0; i < characterSelectHandler.PlayerDatas.Count-1; i++)
                {
                    if (playerInput.playerIndex == i)
                    {
                        data = characterSelectHandler.PlayerDatas[i];
                        data.ControllerID = playerInput.playerIndex;
                        //Debug.Log(Gamepad.current.deviceId);
                        data.ControllerID = Gamepad.current.deviceId;
                    }
                }

                SetPlayerImagePosition();
                cachedId = id;
                deviceID = playerInput.playerIndex;
            }
            
            private void Update()    
            {
                PlayerBlurOut();
            }
            #endregion

            #region Inputs
            
            public void OnNavigation(InputAction.CallbackContext context)
            {
                Gamepad gamepad = Gamepad.all[playerInput.playerIndex];
               
                if (PlayersIsReady) return;
                int amountOfImages =  characterSelectHandler.ImagesBackup.Count;

                Vector2 value = context.ReadValue<Vector2>();
                
                if (gamepad.leftStick.IsActuated() || gamepad.dpad.IsActuated())
                {
                    float movementThreshold = 0.9f;
                    float adjustmentValue = 0.005f;

                    if (value.x > movementThreshold || value.x < -movementThreshold)
                    {
                        currentDelay -= adjustmentValue;

                        if (currentDelay < 0)
                        {
                            id += (gamepad.leftStick.IsActuated()) ? 1 : 0;
                            

                            if (gamepad.leftStick.IsActuated())
                                currentDelay = inputDelay;
                        }

                        if (gamepad.dpad.IsActuated())
                        {
                            id++;
                            currentDelay = inputDelay;
                        }
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
                if (characterSelectHandler.BeginGame && context.action.WasPerformedThisFrame() && deviceID == 0)
                {
                    LevelManager.Instance.LoadLevel(characterSelectHandler.LevelToLoad);
                }
                
                if (!context.action.WasPerformedThisFrame() || PlayersIsReady) return;
                
                if (characterSelectHandler.Images.TryGetValue(id, out Sprite sprite))
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                        
                        data = characterSelectHandler.PlayerDatas[id];
                        data.ControllerID = Gamepad.current.deviceId;
                        
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
                Transform targetTransform = characterSelectHandler.ImagePosition[playerInput.playerIndex];
                transform.SetParent(targetTransform);
                transform.position = targetTransform.position;
                transform.rotation = targetTransform.rotation;
                transform.localScale = targetTransform.GetChild(0).localScale;

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
