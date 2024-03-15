// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-14
// Author: b22alesj
// Description: Handles individual player inputs for racer
// --------------------------------
// ------------------------------*/

using Game.CharacterSelection;
using Game.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


namespace Game {
    namespace Racer {
        public class RacerPlayerInput : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private PlayerInput playerInput;
            [SerializeField] private MultiplayerEventSystem multiplayerEventSystem;
            [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
            
            [HideInInspector] public bool dialogueActice;

            private bool isFirstInput;
            private CarriageRacer carriageRacer;
            private DialogueManager dialogueManager;

            public void Setup(CarriageRacer _carriageRacer, DialogueManager _dialogueManager, InputDevice _inputDevice)
            {
                dialogueActice = false;
                
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
                if (!dialogueActice)
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
            }
        }
    }
}