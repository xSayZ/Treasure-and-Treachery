 // /*------------------------------
// --------------------------------
// Creation Date: 2024/02/05
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using MouseButton = UnityEngine.InputSystem.LowLevel.MouseButton;


namespace Game
{
    namespace UI
    {
        public class GamePadCursor : MonoBehaviour
        {
            [SerializeField]
            private PlayerInput PlayerInput;

            [SerializeField] 
            private RectTransform cursorTransform;

            [SerializeField] 
            private Canvas canvas;
            
            [SerializeField]
            private RectTransform canvasRectTransform;

            [SerializeField] private UnityEngine.Camera mainCamera;
            
            [SerializeField] private float cursorSpeed;
            private Mouse virtualMouse;

            private bool previousMouseState;
            
            private void OnEnable()
            {
                mainCamera = UnityEngine.Camera.main;
                if (virtualMouse == null)
                {
                    virtualMouse = (Mouse) InputSystem.AddDevice("VirtualMouse");
                }       
                else if (!virtualMouse.added)
                {
                    InputSystem.AddDevice(virtualMouse);
                }

                if (cursorTransform != null)
                {
                    Vector2 position = cursorTransform.anchoredPosition;
                    InputState.Change(virtualMouse.position,position);
                }

                InputUser.PerformPairingWithDevice(virtualMouse, PlayerInput.user);

                
                
                InputSystem.onAfterUpdate += UpdateMotion;
            }

            private void OnDisable()
            {
                InputSystem.onAfterUpdate -= UpdateMotion;
            }


            private void UpdateMotion()
            {
                if (virtualMouse == null || Gamepad.current  == null)
                {
                    return;
                }

                Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
                deltaValue *= cursorSpeed * Time.deltaTime;

                Vector2 currentPos = virtualMouse.position.ReadValue();

                Vector2 newPos = currentPos + deltaValue;

                newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
                newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);
                
                InputState.Change(virtualMouse.position,newPos);
                InputState.Change(virtualMouse.delta,deltaValue);


                bool aButtonIsPressed = Gamepad.current.aButton.isPressed;
                if (previousMouseState != aButtonIsPressed)
                {
                    virtualMouse.CopyState<MouseState>(out MouseState mouseState);
                    mouseState.WithButton(MouseButton.Left,aButtonIsPressed);
                    InputState.Change(virtualMouse,mouseState);
                    previousMouseState = aButtonIsPressed;
                }

                AnchorCursor(newPos);

            }

            private void AnchorCursor(Vector2 position)
            {
                Vector2 anchoredPosition; 
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform,position,canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null: mainCamera,out anchoredPosition);
            }
        }
    }
}