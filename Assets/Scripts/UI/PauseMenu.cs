// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-11
// Author: c21frejo
// Description: Treasure and Treachery
// --------------------------------
// ------------------------------*/

using System.Collections;
using Game.Backend;
using Game.Managers;
using Game.Player;
using Game.Racer;
using Game.WorldMap;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


namespace Game
{
    namespace UI
    {
        public class PauseMenu : MonoBehaviour
        {
            [Header("References")] [SerializeField]
            private GameObject pauseCanvas;

            public GameObject[] UIButtons;
            [SerializeField] private LevelDataSO level;
            
            private bool isActive;
            private PlayerInput focusedPad;
            [SerializeField]private MultiplayerEventSystem multiplayerpauseEventSystem;
            [SerializeField]private InputSystemUIInputModule uiInputModule;
            
            [Header("Reset")]
            [SerializeField] private PlayerData[] playerDatasToReset;
            
            #region Unity Functions
            
            public void StartPauseGameplay(bool pressed, PlayerController controller,MultiplayerEventSystem multiplayerEventSystem,InputSystemUIInputModule pauseInput)
            {
                if (pressed && !isActive)
                {
                    pauseCanvas.SetActive(true);
                    focusedPad = controller.GetComponent<PlayerInput>();
                    uiInputModule = pauseInput;
                    multiplayerpauseEventSystem = multiplayerEventSystem;

                    StartCoroutine(SelectFirstChoice());
                    
                    GameManager.Instance.TogglePauseState(controller);
                    isActive = true;

                }
            }

            public void UnPauseGameplay(bool pressed, PlayerController controller)
            {
                if (pressed && isActive && EventSystem.current.currentSelectedGameObject == UIButtons[0].gameObject && focusedPad == controller.GetComponent<PlayerInput>())
                {
                    
                    isActive = false;
                    pauseCanvas.SetActive(false);
                    GameManager.Instance.TogglePauseState(controller);

                }

                if (pressed && isActive && EventSystem.current.currentSelectedGameObject == UIButtons[1].gameObject)
                {
                    ResetPlayerDatas();
                    LevelManager.Instance.LoadLevel(level);
                    GameManager.Instance.TogglePauseState(controller);
                }
            }

            public void PauseOverWorld(bool pressed, RacerPlayerInput racerPlayerInput,MultiplayerEventSystem eventSystem,InputSystemUIInputModule ui)
            {
                if (pressed && !isActive)
                {

                    focusedPad = racerPlayerInput.playerInput;
                    multiplayerpauseEventSystem = eventSystem;
                    uiInputModule = ui;

                    isActive = true;
                    pauseCanvas.SetActive(true);

                    StartCoroutine(SelectFirstChoice());

                    ToggleTimeScale();

                }
            }

            public void UnPauseOverWorld(bool pressed,RacerPlayerInput racerPlayerInput)
            {
                if (pressed && EventSystem.current.currentSelectedGameObject == UIButtons[0].gameObject && focusedPad == racerPlayerInput.playerInput)
                {
                    
                    isActive = false;
                    pauseCanvas.SetActive(false);
                    ToggleTimeScale();
                }
                if (pressed && isActive && EventSystem.current.currentSelectedGameObject == UIButtons[1].gameObject && focusedPad == racerPlayerInput.playerInput)
                {
                    LevelManager.Instance.LoadLevel(level);
                    isActive = false;
                    ToggleTimeScale();
                }
            }

            #endregion

            #region Private Functions

            private void ToggleTimeScale()
            {
                float _newTimeScale = isActive switch
                {
                    true => 0f,
                    false => 1f
                };

                Time.timeScale = _newTimeScale;
            }


            private IEnumerator SelectFirstChoice()
            {
                // Event System requires we clear it first, then wait
                // for at least one frame before we set the current selected object.

                EventSystem.current.SetSelectedGameObject(null);
                yield return new WaitForEndOfFrame();
                EventSystem.current.SetSelectedGameObject(UIButtons[0].gameObject);
            }

            private void ResetPlayerDatas()
            {
                // Reset score gained during this level
                foreach (PlayerData _playerData in playerDatasToReset)
                {
                    _playerData.CancelScene();
                }
            }

            #endregion
        }
    }
}