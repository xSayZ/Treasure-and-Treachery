// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-22
// Author: c21frejo
// Description: Stuff For menu
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using Game.Managers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Game {
    namespace UI {
        public class MenuSelect : MonoBehaviour
        {
            
            public PlayerInput _input;
            
            [Header("Menu Button")]
            [SerializeField] private GameObject[] UIButtons;

            public GameObject selectedObject;
            [Header("Canvases")]
            [SerializeField] private GameObject[] Canvases;

            [Header("Options")] 
            [SerializeField] private Slider[] VolumeSlider;

            private GameObject _cachedSelectedObject;

            private void Start()
            {
                
                StartCoroutine(SelectFirstChoice()); 
            }
            
            //public void OnNavigation(InputAction.CallbackContext context) { }
            
            public void OnSubmit(InputAction.CallbackContext context)
            {
                
                bool isPressed = context.action.WasPressedThisFrame();
                selectedObject = EventSystem.current.currentSelectedGameObject;
                if (selectedObject == null) return;
                if (selectedObject.gameObject == UIButtons[0] && isPressed)
                    //LevelManager.Instance.LoadScene(1);
                
                if (selectedObject.gameObject == UIButtons[1] && isPressed)
                {
                    StartCoroutine(SelectFirstSlider());
                    StopCoroutine(SelectFirstChoice());
                    selectedObject = EventSystem.current.currentSelectedGameObject;
                    Canvases[1].SetActive(true);
                    Canvases[0].SetActive(false);
                }
                if ( Canvases[0].activeSelf &&selectedObject.gameObject == UIButtons[3] && isPressed)
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
                    if (context.action.WasPerformedThisFrame() && Canvases[1])
                    {
                        StartCoroutine(SelectFirstChoice());
                        StopCoroutine(SelectFirstSlider());
                        Canvases[0].SetActive(true);
                        Canvases[1].SetActive(false);
                        Canvases[2].SetActive(false);
                    }
                }

                Debug.Log(EventSystem.current.currentSelectedGameObject);
            }
            
            private IEnumerator SelectFirstChoice() 
            {
                // Event System requires we clear it first, then wait
                // for at least one frame before we set the current selected object.
                EventSystem.current.SetSelectedGameObject(null);
                yield return new WaitForEndOfFrame();
                EventSystem.current.SetSelectedGameObject(UIButtons[0].gameObject);
                
            }
            private IEnumerator SelectFirstSlider() 
            {
                // Event System requires we clear it first, then wait
                // for at least one frame before we set the current selected object.
                EventSystem.current.SetSelectedGameObject(null);
                yield return new WaitForEndOfFrame();
                EventSystem.current.SetSelectedGameObject(VolumeSlider[0].gameObject);
                
            }
            
        }
    }
}
