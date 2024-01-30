// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Scenes
    {
        public enum Objectives
        {
            SHOVELING,
            CUTTING,
            REPAIRING
        }

        [CreateAssetMenu(fileName = "NewLevel", menuName = "Scene Data/Level")]
        public class Level : GameScene
        {
            [Header("Level Specific")]
            public Objectives objectives;
        }
    }
}
