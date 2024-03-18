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
using Game.WorldMap;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Game {
    namespace UI {
        public class MenuSelect : MonoBehaviour
        {
            [Header("References")]
            public PlayerInput _input;
            public LevelDataSO level;

            [Header("Menu Button")]
            [SerializeField] private GameObject[] UIButtons;
            [SerializeField] public GameObject[] settingsFirstSelect;

            public GameObject selectedObject;
            [Header("Canvases")]
            [SerializeField] private GameObject[] Canvases;

            [Header("Options")] 
            [SerializeField] private Slider[] VolumeSlider;

            private GameObject _cachedSelectedObject;
            private void Start()
            {
                StartCoroutine(SelectFirstChoice(UIButtons)); 
            }
            
            public void OnSubmit(InputAction.CallbackContext context)
            {
                bool isPressed = context.action.WasPressedThisFrame();
                    selectedObject = EventSystem.current.currentSelectedGameObject;
                if (selectedObject == null) return;
                
                CanvasSwapper(isPressed);
            }

            private void CanvasSwapper(bool isPressed)
            {
                if (selectedObject.gameObject == UIButtons[0] && isPressed)
                    LevelManager.Instance.LoadLevel(level);
                
                if (selectedObject.gameObject == UIButtons[1] && isPressed)
                {
                    StopCoroutine(SelectFirstChoice(UIButtons));
                    StartCoroutine(SelectFirstChoice(settingsFirstSelect));
                    selectedObject = EventSystem.current.currentSelectedGameObject;
                    Canvases[1].SetActive(true);
                    Canvases[0].SetActive(false);
                }
                if ( Canvases[0].activeSelf && selectedObject.gameObject == UIButtons[3] && isPressed)
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
                        StartCoroutine(SelectFirstChoice(UIButtons));
                        StopCoroutine(SelectFirstSlider());
                        
                        Canvases[0].SetActive(true);
                        Canvases[1].SetActive(false);
                        Canvases[2].SetActive(false);
                    }
                }

                Debug.Log(EventSystem.current.currentSelectedGameObject);
            }
            
            private IEnumerator SelectFirstChoice(GameObject[] _UIButtons) 
            {
                // Event System requires we clear it first, then wait
                // for at least one frame before we set the current selected object.
                EventSystem.current.SetSelectedGameObject(null);
                yield return new WaitForEndOfFrame();
                EventSystem.current.SetSelectedGameObject(_UIButtons[0].gameObject);
                
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
