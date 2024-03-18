// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-11
// Author: c21frejo
// Description: Treasure and Treachery
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using Game.Backend;
using Game.Managers;
using Game.Player;
using Game.Racer;
using Game.WorldMap;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


namespace Game
{
    namespace UI
    {
        public class PauseMenu : MonoBehaviour
        {
            [Header("Pause Stuff")] [SerializeField]
            private GameObject pauseCanvas;

            public GameObject[] UIButtons;
            [SerializeField] private LevelDataSO level;
            private List<PlayerInput> inputs;
            private bool isActive;
            private bool isPaused;
            private PlayerInput focusedPad;
            #region Unity Functions

            // Start is called before the first frame update
            

            private void Start()
            {
                StartCoroutine(SelectFirstChoice());
            }

            public void StartPauseGameplay(bool pressed, PlayerController controller)
            {
                if (pressed && !isActive)
                {
                    pauseCanvas.SetActive(true);
                    GameManager.Instance.TogglePauseState(controller);
                    
                    
                    isActive = true;
                }
            }

            public void UnPauseGameplay(bool pressed, PlayerController controller)
            {
                if (pressed && isActive && EventSystem.current.currentSelectedGameObject == UIButtons[0].gameObject)
                {
                    
                    isActive = false;
                    pauseCanvas.SetActive(false);
                    GameManager.Instance.TogglePauseState(controller);
                }

                if (pressed && isActive && EventSystem.current.currentSelectedGameObject == UIButtons[1].gameObject)
                {
                    LevelManager.Instance.LoadLevel(level);
                    GameManager.Instance.TogglePauseState(controller);
                }
            }

            public void PauseOverWorld(bool pressed, RacerPlayerInput racerPlayerInput)
            {
                if (pressed && !isActive)
                {
                    focusedPad = racerPlayerInput.playerInput;
                    isActive = true;
                    isPaused = true;
                    pauseCanvas.SetActive(true);
                    ToggleTimeScale();

                }
            }

            public void UnPauseOverWorld(bool pressed,RacerPlayerInput racerPlayerInput)
            {
                if (pressed && EventSystem.current.currentSelectedGameObject == UIButtons[0].gameObject && focusedPad == racerPlayerInput.playerInput)
                {
                    isActive = false;
                    isPaused = false;
                    pauseCanvas.SetActive(false);
                    ToggleTimeScale();
                }
                if (pressed && isActive && EventSystem.current.currentSelectedGameObject == UIButtons[1].gameObject && focusedPad == racerPlayerInput.playerInput)
                {
                    LevelManager.Instance.LoadLevel(level);
                    isActive = false;
                    isPaused = false;
                    ToggleTimeScale();
                }
            }

            #endregion

            #region Private Functions

            private void ToggleTimeScale()
            {
                float _newTimeScale = isPaused switch
                {
                    true => 0f,
                    false => 1f
                };

                Time.timeScale = _newTimeScale;
            }


            public IEnumerator SelectFirstChoice()
            {
                // Event System requires we clear it first, then wait
                // for at least one frame before we set the current selected object.

                EventSystem.current.SetSelectedGameObject(null);
                yield return new WaitForEndOfFrame();
                EventSystem.current.SetSelectedGameObject(UIButtons[0].gameObject);
            }

            #endregion
        }
    }
}