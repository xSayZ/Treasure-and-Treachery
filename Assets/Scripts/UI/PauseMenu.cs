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
using Game.WorldMap;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


namespace Game {
    namespace UI {
        public class PauseMenu : MonoBehaviour
        {
            [Header("Pause Stuff")]
            [SerializeField] private GameObject pauseCanvas;
            public GameObject[] UIButtons;
            [SerializeField] private LevelDataSO level;
            
            private PlayerController playerController;
            private bool isActive;
            
#region Unity Functions
            // Start is called before the first frame update

            private void Start()
            {
                StartCoroutine(SelectFirstChoice(playerController));
            }

            public void StartPause(bool pressed, PlayerController controller)
            {
                playerController = controller;
                if (pressed && !isActive)
                {
                    pauseCanvas.SetActive(true);
                    GameManager.Instance.TogglePauseState(controller);
                    isActive = true;
                }
            }

            public void UnPause(bool pressed, PlayerController controller)
            {
                if (pressed && isActive && EventSystem.current.currentSelectedGameObject == UIButtons[0].gameObject && controller)
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
#endregion

#region Public Functions

#endregion

#region Private Functions
        private IEnumerator SelectFirstChoice(PlayerController controller) 
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
