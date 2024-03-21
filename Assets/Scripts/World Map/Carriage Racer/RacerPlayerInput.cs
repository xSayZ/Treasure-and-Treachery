// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-14
// Author: b22alesj
// Description: Handles individual player inputs for racer
// --------------------------------
// ------------------------------*/

using System;
using Game.CharacterSelection;
using Game.Dialogue;
using Game.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Serialization;


namespace Game {
    namespace Racer {
        public class RacerPlayerInput : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] public PlayerInput playerInput;
            [SerializeField] private MultiplayerEventSystem multiplayerEventSystem;
            [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
            [SerializeField] private PauseMenu _pauseMenu;

            [FormerlySerializedAs("dialogueActice")]
            [HideInInspector] public bool dialogueActive;

            private bool isFirstInput;
            private CarriageRacer carriageRacer;
            private DialogueManager dialogueManager;
            private bool isPaused;

            private void Awake()
            {
                _pauseMenu = FindObjectOfType<PauseMenu>(true);

            }

            public void Setup(CarriageRacer _carriageRacer, DialogueManager _dialogueManager, InputDevice _inputDevice)
            {
                dialogueActive = false;
                
                carriageRacer = _carriageRacer;
                dialogueManager = _dialogueManager;
                playerInput.SwitchCurrentControlScheme(_inputDevice);
                
                dialogueManager.racerPlayerInputs.Add(this);
                
                isFirstInput = _inputDevice == CharacterSelect.GetFirstInputDevice();
                
                if (!isFirstInput)
                {
                    inputSystemUIInputModule.enabled = false;
                    multiplayerEventSystem.enabled = false;
                }
            }

            public void OnMove(InputAction.CallbackContext _context)
            {
                carriageRacer.OnMoveInput(playerInput.user.index, _context.ReadValue<Vector2>());
            }

            public void OnSubmit(InputAction.CallbackContext _context)
            {
                if (!dialogueActive)
                {
                    if (_context.started)
                    {
                        carriageRacer.OnSelectPlayMarker();
                    }
                }
                else if (isFirstInput)
                {
                    dialogueManager.SubmitPressed(_context);
                }

                if (isPaused)
                {
                    _pauseMenu.UnPauseOverWorld(_context.started,this);
                    isPaused = false;
                    inputSystemUIInputModule.enabled = false;
                    multiplayerEventSystem.enabled = false;
                }
                
            }

            public void OnPause(InputAction.CallbackContext _context)
            {
                inputSystemUIInputModule.enabled = true;
                multiplayerEventSystem.enabled = true;
                isPaused = true;
                _pauseMenu.PauseOverWorld(_context.started,this,multiplayerEventSystem,inputSystemUIInputModule);
                
            }
        }
    }
}