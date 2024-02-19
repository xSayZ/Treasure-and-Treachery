
// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/05
// Author: Fredrik
// Description: Character Selection Screen
// --------------------------------
// ------------------------------*/


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
            public enum Characters
            {
                w  = 0,
                g  = 1,
                ds = 2,
                po = 3,
            }
            
            
            public GameObject gameObject;
            public Image Image;

            public bool playersIsReady = false;
            
            private PlayerInput playerInputs;
            private int currentId;
            private int id;

            private Sprite cachedSprite;
            private int cachedId;

            private float inputDelay;

            public PlayerData data;
            // Start is called before the first frame update

            #region Unity functions

            public void Start()
            {
                HorizontalLayoutGroup layoutGroup = FindObjectOfType<HorizontalLayoutGroup>();
                transform.parent = layoutGroup.transform;
                playerInputs = GetComponent<PlayerInput>();
                Image.sprite = CharacterSelectManager.Instance.bank.characterImages[0];
                
                inputDelay = 0.01f;
            }

            #endregion

            #region Public

            public void OnNavigation(InputAction.CallbackContext context)
            {
                if (playersIsReady) return;

               
                Vector2 value = context.ReadValue<Vector2>();
                if (value.y > 0)
                {
                    
                    inputDelay -= Time.deltaTime;
                    
                    if (inputDelay <0)
                    {
                        id += 1;
                        inputDelay = 0.01f;
                    }
                }
                if (value.y < 0)
                {
                    if(inputDelay <0)
                    {
                        id -= 1;
                        inputDelay = 0.01f; 
                    }
                }
                
                id = Wrap(id, 0, 4);
                if (id == 4) id = 0;

                if (CharacterSelectManager.Instance.Images.TryGetValue(id,out Sprite sprite))
                {
                    Image.sprite = sprite;
                    cachedId = id;
                    cachedSprite = sprite;
                }
                

            }

            public void OnConfirm(InputAction.CallbackContext context)
            {
                if (!context.performed || playersIsReady) return;
                
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                    
                    data = CharacterSelectManager.Instance.Datas[id];
                    data.CharacterID= id;
                    CharacterSelectManager.Instance.Images.Remove(cachedId);

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
                    CharacterSelectManager.Instance.Images.Add(cachedId,cachedSprite);

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
