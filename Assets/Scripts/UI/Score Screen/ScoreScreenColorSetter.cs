// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: b22alesj
// Description: Sets the color for the player image
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Backend;
using UnityEngine;


namespace Game {
    namespace NAME {
        public class ScoreScreenColorSetter : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private PlayerData playerData;
            [SerializeField] private SkinnedMeshRenderer[] playerMeshRenderers;
            
            private void Start()
            {
                foreach (var _playerMeshRenderer in playerMeshRenderers)
                {
                    _playerMeshRenderer.materials[0].color = playerData.playerMaterialColor;
                }
            }
        }
    }
}
