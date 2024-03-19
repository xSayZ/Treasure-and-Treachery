// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-22
// Author: c21frejo / b22feldy
// Description: This class is responsible for handling the main menu interactions in the game.
// --------------------------------
// ------------------------------*/

using System.Collections;
using Game.WorldMap;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Game {
    namespace UI {
        public class MenuSelect : MonoBehaviour
        {
            [Header("References")]
            [SerializeField] private PlayerInput input;
            [SerializeField] private WorldMap.LevelDataSO levelToLoad;

            [Header("Menu Button")]
            [SerializeField] private GameObject[] uiButtons;
            [SerializeField] public GameObject[] settingsFirstSelect;
            
            [Header("Canvases")]
            [SerializeField] private GameObject[] canvases;
            private GameObject selectedObject;

            [Header("Options")] 
            [SerializeField] private Slider[] volumeSliders;
            
            [Header("Resets")]
            [SerializeField] private WorldMap.WorldMapManager worldMapManager;
            [SerializeField] private Backend.CarriageData carriageData;
            [SerializeField] private Dialogue.CurrentDialogueSO[] currentDialogueSO;
            [SerializeField] private Backend.PlayerData[] playerDatas;
            [SerializeField] private LevelDataSO[] levels;

            private GameObject cachedSelectedObject;
            
            
            public void Setup(PlayerInput _input) {
                StartCoroutine(SelectFirstChoice(uiButtons));
                input = _input;
                input.actions["Submit"].performed += OnSubmit;
                input.actions["Cancel"].performed += OnCancel;
            }

            private void OnDisable() {
                input.actions["Submit"].performed -= OnSubmit;
                input.actions["Cancel"].performed -= OnCancel;
            }

            public void OnSubmit(InputAction.CallbackContext context)
            {
                bool _isPressed = context.action.WasPressedThisFrame();
                    selectedObject = EventSystem.current.currentSelectedGameObject;
                if (selectedObject == null) return;
                
                CanvasSwapper(_isPressed);
            }

            private void CanvasSwapper(bool isPressed)
            {
                if (selectedObject.gameObject == uiButtons[0] && isPressed) {
                    Managers.LevelManager.Instance.LoadLevel(levelToLoad);
                    
                    //Reset
                    ResetSO();
                }
                
                if (selectedObject.gameObject == uiButtons[1] && isPressed)
                {
                    StopCoroutine(SelectFirstChoice(uiButtons));
                    StartCoroutine(SelectFirstChoice(settingsFirstSelect));
                    selectedObject = EventSystem.current.currentSelectedGameObject;
                    canvases[1].SetActive(true);
                    canvases[0].SetActive(false);
                }
                if(selectedObject.gameObject == uiButtons[2] && isPressed)
                {
                    canvases[2].SetActive(true);
                    canvases[0].SetActive(false);
                }
                if ( canvases[0].activeSelf && selectedObject.gameObject == uiButtons[3] && isPressed)
                {
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                    #endif
                    Application.Quit();
                }
            }

            public void OnCancel(InputAction.CallbackContext context)
            {
                if (!canvases[0].activeSelf)
                {
                    if (context.action.WasPerformedThisFrame() && canvases[1])
                    {
                        StartCoroutine(SelectFirstChoice(uiButtons));
                        StopCoroutine(SelectFirstSlider());
                        
                        canvases[0].SetActive(true);
                        canvases[1].SetActive(false);
                        canvases[2].SetActive(false);
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
                EventSystem.current.SetSelectedGameObject(volumeSliders[0].gameObject);
                
            }
            
            private void ResetSO() {

                worldMapManager.Reset();
                carriageData.Reset();
                foreach (Dialogue.CurrentDialogueSO _currentDialogueSO in currentDialogueSO)
                {
                    _currentDialogueSO.Reset();
                }
                foreach (Backend.PlayerData _playerData in playerDatas)
                {
                    _playerData.Reset();
                }
                foreach (LevelDataSO _level in levels)
                {
                    _level.Reset();
                }
            }
        }
    }
}
