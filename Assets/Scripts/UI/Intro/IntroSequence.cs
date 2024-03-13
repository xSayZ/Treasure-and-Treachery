// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: c21frejo
// Description: Introsekvens
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Backend;
using Game.CharacterSelection;
using Game.Managers;
using Game.WorldMap;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Game
{
    namespace UI
    {
        public class IntroSequence : MonoBehaviour
        {

            [Header("References")]
            [SerializeField] private PlayerInput playerInput;
            [SerializeField] private LevelDataSO levelToLoad;
            [SerializeField] private Image introPanel;
            
            [Header("Intro Images")]
            [SerializeField] private Sprite[] introImages;
            
            [Header("Timer Before next intro slide")]
            public float timeBeforeChange;
            
            private float currentTime;
            private float elapsedTime;
            private int index;
            private bool done;
            private bool stopUpdating;

            private bool skipped;

            #region Unity Functions

            private void Awake()
            {
                // TEMPORARY
                foreach (KeyValuePair<InputDevice, PlayerData> _kvp in CharacterSelect.selectedCharacters)
                {
                    playerInput.SwitchCurrentControlScheme(_kvp.Key);
                }
            }

            void Start() {
                InitializeIntro();
            }
            
            private void InitializeIntro() {
                stopUpdating = false;
                introPanel.sprite = introImages[0];
                currentTime = timeBeforeChange;
            }
            
            void Update() {
                ChangeImage();
                ChangeScene();
                HandleSkipIntro();
            }

            #endregion

            public void OnSkip(InputAction.CallbackContext context) {
                if (context.action.WasPerformedThisFrame())
                {
                    skipped = true;
                }

                if (context.action.WasReleasedThisFrame())
                    skipped = false;
            }
            
            #region Private Functionn

            private void ChangeImage() {
                currentTime -= Time.deltaTime;
                
                if (((currentTime <= 0) || skipped) && index < introImages.Length - 1)
                {
                    currentTime = timeBeforeChange;
                    introPanel.sprite = introImages[++index];
                }

                if (index != introImages.Length - 1 || done)
                    return;
                
                elapsedTime += Time.deltaTime;
                done = true;
                skipped = false;
            }

            private void ChangeScene() {
                if ((!done || !(elapsedTime > timeBeforeChange) || stopUpdating) && (!done || !skipped))
                    return;
                
                stopUpdating = true;
                LevelManager.Instance.LoadLevel(levelToLoad);
            }
            
            private void HandleSkipIntro() {
                if (skipped) {
                    skipped = false;
                }            
            }

            #endregion
        }
    }
}