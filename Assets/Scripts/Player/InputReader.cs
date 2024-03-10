// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-05
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace Game {
    namespace Player {
        [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
        public class InputReader : ScriptableObject,PlayerControls.IPlayerActions,PlayerControls.IEventsActions
        {
            public event UnityAction<Vector2> movementEvent = delegate { };
            public event UnityAction meleeEvent = delegate { };
            public event UnityAction rangedEvent = delegate { };
            public event UnityAction pauseEvent = delegate { };
            public event UnityAction startedDashing = delegate { };


            private PlayerControls _playerControls;
            private void OnEnable()
            {
                if (_playerControls != null) return;
                
                _playerControls = new PlayerControls();
                _playerControls.Player.SetCallbacks(this);
                _playerControls.Events.SetCallbacks(this);
            }


            public void OnMovement(InputAction.CallbackContext context)
            {
                movementEvent.Invoke(context.ReadValue<Vector2>());
            }

            public void OnRangedAttack(InputAction.CallbackContext context)
            {
                if (context.performed)
                {
                    rangedEvent.Invoke();
                }
            }

            public void OnMeleeAttack(InputAction.CallbackContext context)
            {
                throw new System.NotImplementedException();
            }

            public void OnTogglePause(InputAction.CallbackContext context)
            {
                
            }

            public void OnSubmit(InputAction.CallbackContext context)
            {
                
            }
        }
    }
}
