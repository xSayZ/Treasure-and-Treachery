// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-05
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using Game.Backend;
using Game.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Game {
    namespace UI {
        public class PlayerSetupMenuController : MonoBehaviour
        {
            
            public GameObject playerCursor;

            public void OnMenuNavigation(InputAction.CallbackContext value)
            {
                value.ReadValue<Vector2>();
            }
            

            public void SpawnPlayerIndex()
            {
                GameObject canvas = GameObject.FindGameObjectWithTag("Selection");

                for (int i = 0; i < Gamepad.all.Count; i++)
                {
                    GameObject cursor = Instantiate(playerCursor, canvas.transform);
                }

            }
#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                SpawnPlayerIndex();
                for (int i = 0; i < Gamepad.all.Count; i++)
                {
                }
                
            }

            void SelectPlayer(int index)
            {
                
            }
            
    
            // Update is called once per frame
            void Update()
            {
            }
#endregion



#region Public Functions
public void SetPlayerColor(int index,Material color)
{

    GameManager.Instance.activePlayerControllers[index].gameObject.GetComponent<Material>().color = color.color;
}


public void ReadyPlayer(int index)
{
    //if(_GameManager.activePlayerControllers.Count == MaxPlayers)
}
#endregion

#region Private Functions

#endregion
        }
    }
}
