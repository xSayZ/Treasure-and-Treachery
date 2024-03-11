// /*------------------------------
// --------------------------------
// Creation Date: 2024/03/11
// Author: Fredrik
// Description: Treasure and Treachery
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game
{
    namespace UI
    {
        public class PauseController : MonoBehaviour
        {
            [SerializeField] private PauseMenu pauseMenu;
            
            // Start is called before the first frame update

            // Update is called once per frame
            private void Start()
            {
                
            }

            private void Update()
            {
                foreach (Gamepad pad in Gamepad.all)
                {
                    pauseMenu.PauseOverWorld(pad.startButton.isPressed, pad);
                    pauseMenu.UnPauseOverWorld(pad.aButton.isPressed,pad);

                }
                
            }
            
        }
    }
}