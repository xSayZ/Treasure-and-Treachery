
// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/05
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/
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


            public enum Select
            {
                wolf,
                lilith,
                gorgon,
                kobold
            }


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
                if (!playersIsReady)
                {
                    Vector2 value = context.ReadValue<Vector2>();
                    id += (int)value.y;
                    id = Wrap(id, 0, 4);
                    if (id == 4) id = 0;
                    Image.sprite = SetupSelector.Images[id % 4];
                    currentId = id % 4;

                }

            }

            public void OnConfirm(InputAction.CallbackContext context)
            {
                if (context.performed && !playersIsReady)
                {
                    
                    data.playerIndex = currentId;
                    
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }
                    
                    playersIsReady = true;


                }
            }

            #endregion

            public void OnCancel(InputAction.CallbackContext context)
            {
                if (context.performed && playersIsReady)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                    playersIsReady = false;
                }
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
