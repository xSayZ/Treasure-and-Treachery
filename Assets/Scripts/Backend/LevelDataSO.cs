// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-24
// Author: Felix
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Game {
    namespace WorldMap {
        [CreateAssetMenu(fileName = "Level Data", menuName = "World Map/Level Data", order = 1)]
        public class LevelDataSO : ScriptableObject
        {
            public string levelName;
            public string levelDescription;
            public Image levelImage;
            public bool isCompleted;

            public bool isGameplayScene = true;
            
            private UnityEvent onLevelCompleted;
            
            public List<LevelDataSO> prerequisites = new List<LevelDataSO>();
            
            public UnityEvent OnLevelCompleted
            {
                get => onLevelCompleted;
                set => onLevelCompleted = value;
            }
        }
    }
}
