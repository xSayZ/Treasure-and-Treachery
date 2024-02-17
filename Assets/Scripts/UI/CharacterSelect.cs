
// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/05
// Author: Fredrik
// Description: Character Selection Screen
// --------------------------------
// ------------------------------*/


using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game
{
    namespace UI
    {
        public class CharacterSelect : MonoBehaviour
        {
            public GameObject gameObject;
            public Image Image;

            public bool playersIsReady { get; private set; } = false;
            
            private PlayerInput playerInputs;
            private int currentId;
            private int id;

            private Sprite cachedSprite;
            private int cachedId;

            private float inputDelau;
            // Start is called before the first frame update

            #region Unity functions

            public void Start()
            {
                HorizontalLayoutGroup layoutGroup = FindObjectOfType<HorizontalLayoutGroup>();
                transform.parent = layoutGroup.transform;
                playerInputs = GetComponent<PlayerInput>();
                Image.sprite = SetupSelector.Instance.bank.characterImages[1];
                inputDelau = 1;

            }

            #endregion

            #region Public

            public void OnNavigation(InputAction.CallbackContext context)
            {
                if (playersIsReady) return;

                inputDelau -= Time.deltaTime;
                Vector2 value = context.ReadValue<Vector2>();
                if (value.y > 0)
                {
                    
                    inputDelau -= Time.deltaTime;
                    Debug.Log(inputDelau);
                    if (inputDelau <0)
                    {
                        id += 1;
                        inputDelau = 0.1f;

                    }
                   
                }

                if (value.y < 0)
                {
                    if(inputDelau <0)
                    {
                        id -= 1;
                        inputDelau = 0.1f; 
                    }
                    

                }
                
                id = Wrap(id, 0, 4);
                if (id == 4) id = 0;
                {
                    
                }
                Image.sprite = SetupSelector.Instance.bank.characterImages[id];
                

            }

            public void OnConfirm(InputAction.CallbackContext context)
            {
                if (!context.performed || playersIsReady) return;
                
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }

                playersIsReady = true;


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
