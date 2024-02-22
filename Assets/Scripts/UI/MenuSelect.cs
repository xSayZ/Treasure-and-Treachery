// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-22
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


namespace Game {
    namespace UI {
        public class MenuSelect : MonoBehaviour
        {
            
            private PlayerInput _input;
            [Header("Menu Button")]
            [SerializeField] private GameObject[] UIButtons;

            private GameObject selectedObject;

            [SerializeField] private GameObject settingsCanvas;
            [SerializeField] private GameObject menuCanvas;
            
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
                context.action.WasPressedThisFrame();
                selectedObject = EventSystem.current.currentSelectedGameObject;
                if (selectedObject.gameObject == UIButtons[0])
                {
                    LevelManager.Instance.LoadScene(6);
                }

                if (selectedObject.gameObject == UIButtons[1])
                {
                    
                }
                
                if (selectedObject.gameObject == UIButtons[2])
                {
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                    #endif
                    
                    Application.Quit();
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
