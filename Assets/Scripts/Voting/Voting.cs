// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-27
// Author: Felix
// Description: Handles Inputs from players between the different buttons
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.UI;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game {
    namespace Voting {
        public class Voting : MonoBehaviour
        {
            public List<VotingButton> votingButtons;
            public List<InputAction> playerInputs = new List<InputAction>();
            
            public InputActionAsset playerInputsAsset;
#region Unity Functions

            void Start()
            {
                foreach (var actionMap in playerInputsAsset.actionMaps) {
                    foreach (var action in actionMap.actions) {
                        action.Enable();
                        playerInputs.Add(action);
                    }
                }
            }
            
            void Update()
            {
                // Access input values for each player
                for (int i = 1; i <= CharacterSelect.CharacterSelect.selectedCharacters.Count; i++)
                {
                    Debug.Log("Test");
                }
            }
            
#endregion

#region Public Functions

#endregion

#region Private Functions

#endregion
        }
    }
}
