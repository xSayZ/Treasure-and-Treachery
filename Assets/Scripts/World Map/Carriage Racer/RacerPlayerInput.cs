// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-14
// Author: b22alesj
// Description: Handles individual player inputs for racer
// --------------------------------
// ------------------------------*/

using UnityEngine;
using UnityEngine.InputSystem;


namespace Game {
    namespace Racer {
        public class RacerPlayerInput : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private PlayerInput playerInput;
            
            private CarriageRacer carriageRacer;

            public void Setup(CarriageRacer _carriageRacer, InputDevice _inputDevice)
            {
                carriageRacer = _carriageRacer;
                playerInput.SwitchCurrentControlScheme(_inputDevice);
            }

            public void OnMove(InputAction.CallbackContext _context)
            {
                carriageRacer.OnMoveInput(playerInput.user.index, _context.ReadValue<Vector2>());
            }

            public void OnSubmit(InputAction.CallbackContext _context)
            {
                if (_context.started)
                {
                    carriageRacer.OnSelectPlayMarker();
                }
            }
        }
    }
}