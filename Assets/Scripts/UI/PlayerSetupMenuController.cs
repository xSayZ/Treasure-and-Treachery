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


namespace Game {
    namespace NAME {
        public class PlayerSetupMenuController : MonoBehaviour
        {
            public List<GameObject> Buttons;
            private PlayerData data;
        

            public void OnMenuNavigation(InputAction.CallbackContext value)
            {
                value.ReadValue<Vector2>();
            }

            public void GetPlayerIndex()
            {
                for (int i = 0; i < GameManager.Instance.activePlayerControllers.Count; i++)
                {
                    
                }

            }

            
            public 
            
            
            
            
            
#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                GetPlayerIndex();

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
