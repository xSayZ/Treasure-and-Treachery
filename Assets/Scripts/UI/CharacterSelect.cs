
// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/05
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
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
            [SerializeField] private GameObject UIImage;
            [SerializeField] PlayerData data;
            public Image Image;

            public bool playersIsReady;

            [SerializeField] private GameObject Ready;
            private PlayerInput playerInputs;
            private int currentId;
            private int id;

            private Sprite cachedSprite;
            private int cachedId;


            // Start is called before the first frame update

            #region Unity functions

            public void Start()
            {
                playerInputs = GetComponent<PlayerInput>();
                
            }

            #endregion

            #region Public

            public void OnNavigation(InputAction.CallbackContext context)
            {
                if (playersIsReady) return;
                
                Vector2 value = context.ReadValue<Vector2>();
                id += (int)value.y;
                id = Wrap(id, 0, 4);
                if (SetupSelector.Images.TryGetValue(id,out Sprite sprite))
                {
                    Image.sprite = sprite;
                }

            }

            public void OnConfirm(InputAction.CallbackContext context)
            {
                if (!context.performed || playersIsReady) return;
                
                data.playerIndex = currentId;
                    
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                    
                cachedSprite = SetupSelector.Images[currentId];
                cachedId = currentId;
                
                playersIsReady = true;
                SetupSelector.Images.Remove(currentId);
            }

            #endregion

            public void OnCancel(InputAction.CallbackContext context)
            {
                if (!context.performed || !playersIsReady) return;
                
                
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                    
                playersIsReady = false;
                SetupSelector.Images.Add(cachedId,cachedSprite);

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


            #endregion
            
        }
    }
}