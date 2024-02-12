// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-12
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using Game.Backend;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game {
    namespace Player {
        public class PlayerVisualBehaviour : MonoBehaviour
        {
            //Player ID
            private int playerID;
            private PlayerData playerData;
            
            [SerializeField] private SkinnedMeshRenderer[] playerMeshRenderers;

#region Unity Functions

#endregion

#region Public Functions

            public void SetupBehaviour(PlayerData newPlayerData) {
                playerData = newPlayerData;
                
                UpdatePlayerVisuals();
            }
            
#endregion

#region Private Functions

            public void UpdatePlayerVisuals() {
                UpdateCharacterMaterial();
            }

            void UpdateCharacterMaterial() {
                // Set the player material color
                foreach (var playerMeshRenderer in playerMeshRenderers) {
                    playerMeshRenderer.material.color = playerData.playerMaterialColor;
                }
            }
            
#endregion
        }
    }
}
