// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-14
// Author: b22alesj
// Description: Handles individual player input for tutorial screen
// --------------------------------
// ------------------------------*/

using UnityEngine;
using UnityEngine.InputSystem;


namespace Game {
    namespace Tutorial {
        public class TutorialPlayerInput : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private PlayerInput playerInput;
            
            private TutorialScreen tutorialScreen;
            private int playerIndex;
            private bool isDone;

            public void Setup(TutorialScreen _tutorialScreen, int _playerIndex, InputDevice _inputDevice)
            {
                tutorialScreen = _tutorialScreen;
                playerIndex = _playerIndex;

                playerInput.SwitchCurrentControlScheme(_inputDevice);
            }

            public void OnSubmit(InputAction.CallbackContext _context)
            {
                if (_context.started && !isDone)
                {
                    isDone = true;
                    tutorialScreen.PlayerDone(playerIndex);
                }
            }
        }
    }
}