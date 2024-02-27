// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-24
// Author: Felix
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/


using System.Collections.Generic;
using UnityEngine;


namespace Game {
    namespace WorldMap {
        [CreateAssetMenu(fileName = "Level Data", menuName = "World Map/Level Data", order = 1)]
        public class LevelDataSO : ScriptableObject
        {
            public string levelName;
            public string levelDescription;
            public Sprite levelImage;
            public bool isCompleted;
            public bool deleteObject;
            
            public List<string> prerequisites = new List<string>();
        }
    }
}
