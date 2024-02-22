// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-22
// Author: c21frejo
// Description: Stuff For menu
// --------------------------------
// ------------------------------*/

using System.Collections;
using Game.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Game {
    namespace UI {
        public class MenuSelect : MonoBehaviour
        {
            
            private PlayerInput _input;
            
            [Header("Menu Button")]
            [SerializeField] private GameObject[] UIButtons;

            private GameObject selectedObject;
            [Header("Canvases")]
            [SerializeField] private GameObject[] Canvases;

            [Header("Options")] 
            [SerializeField] private Slider VolumeSlider;
            
            
            private void Start()
            {
                _input = GetComponent<PlayerInput>();
                
                StartCoroutine(SelectFirstChoice());
                
            }
            
            public void OnMenuNavigation(InputAction.CallbackContext context)
            {
                context.ReadValue<Vector2>();
            }
            public void OnSubmit(InputAction.CallbackContext context)
            {
                bool isPressed =context.action.WasPressedThisFrame();
                selectedObject = EventSystem.current.currentSelectedGameObject;
                if (selectedObject.gameObject == UIButtons[0] && isPressed)
                    LevelManager.Instance.LoadScene(6);

                if (selectedObject.gameObject == UIButtons[1]&& isPressed)
                {
                    Canvases[1].SetActive(true);
                    Canvases[0].SetActive(false);
                    
                }
                
                if (selectedObject.gameObject == UIButtons[3]&& isPressed)
                {
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                    #endif
                    Application.Quit();
                }
            }

            public void OnCancel(InputAction.CallbackContext context)
            {
                if (!Canvases[0].activeSelf)
                {
                    if (context.action.WasPerformedThisFrame())
                    {
                        Canvases[0].SetActive(true);
                        Canvases[2].SetActive(false);
                    }
                   
                    
                }
            }
            
            private IEnumerator SelectFirstChoice() 
            {
                // Event System requires we clear it first, then wait
                // for at least one frame before we set the current selected object.
                EventSystem.current.SetSelectedGameObject(null);
                yield return new WaitForEndOfFrame();
                EventSystem.current.SetSelectedGameObject(UIButtons[0].gameObject);
            }
        }
    }
}
