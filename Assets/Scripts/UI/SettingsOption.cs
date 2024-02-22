// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-22
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Game {
    namespace UI {
        public class SettingsOption : MonoBehaviour
        {
            public Slider slider;
            public float sliderValue;
            public PlayerInput _input;

            public void Start()
            {
                
            }

            public void OnMenuNavigation(InputAction.CallbackContext context)
            {
                float sliderValue = context.ReadValue<float>();
                slider.value = sliderValue;

            }
        }
 
    }
}
