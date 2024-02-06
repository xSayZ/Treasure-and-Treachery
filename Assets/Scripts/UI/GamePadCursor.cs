 // /*------------------------------
// --------------------------------
// Creation Date: 2024/02/05
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
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

            private void Awake()
            {
                canvas = FindObjectOfType<Canvas>();
                canvasRectTransform = GameObject.FindGameObjectWithTag("Selection").GetComponent<RectTransform>();
            }

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

                for (int i = 0; i < Gamepad.all.Count; i++)
                {
                    InputUser.PerformPairingWithDevice(virtualMouse, PlayerInput.user);
                    Debug.Log(PlayerInput.user);
                }
                
                
                InputSystem.onAfterUpdate += UpdateMotion;
                

            }

            private void OnDisable()
            {
                if (virtualMouse != null && virtualMouse.added)
                {
                    InputSystem.RemoveDevice(virtualMouse);
                }
                InputSystem.onAfterUpdate -= UpdateMotion;
            }


            private void UpdateMotion()
            {
                if (virtualMouse == null || Gamepad.all[PlayerInput.user.index]  == null)
                {
                    return;
                }

                Vector2 deltaValue = Gamepad.all[PlayerInput.user.index] .leftStick.ReadValue();
                deltaValue *= cursorSpeed * Time.deltaTime;

                Vector2 currentPos = virtualMouse.position.ReadValue();

                Vector2 newPos = currentPos + deltaValue;

                newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
                newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);
                
                InputState.Change(virtualMouse.position,newPos);
                InputState.Change(virtualMouse.delta,deltaValue);


                bool aButtonIsPressed = Gamepad.all[PlayerInput.user.index].aButton.isPressed;
                if (previousMouseState != aButtonIsPressed)
                {
                    virtualMouse.CopyState<MouseState>(out MouseState mouseState);
                    mouseState.WithButton(MouseButton.Left,aButtonIsPressed);
                    InputState.Change(virtualMouse,mouseState);
                    previousMouseState = aButtonIsPressed;
                }

                AnchorCursor(newPos);
                Debug.Log(virtualMouse.position);
            }

            private void AnchorCursor(Vector2 position)
            {
                Vector2 anchoredPosition; 
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform,position,canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null: mainCamera,out anchoredPosition);
                cursorTransform.anchoredPosition = anchoredPosition;   
            }
        }
    }
}