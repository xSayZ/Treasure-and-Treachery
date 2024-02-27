// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-24
// Author: Felix
// Description: This script is responsible for managing the world map
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;

namespace Game {
    namespace WorldMap {
        [CreateAssetMenu(fileName = "World Map Manager", menuName = "World Map/Manager", order = 0)]
        public class WorldMapManager : ScriptableObject
        {

            public List<LevelDataSO> completedLevels = new List<LevelDataSO>();
            
            public void MarkLevelAsCompleted(string levelName)
            {
                LevelDataSO level = completedLevels.Find(l => l.levelName == levelName);
                if (level == null) {
                    level = CreateInstance<LevelDataSO>();
                    level.levelName = levelName;
                    level.isCompleted = true;
                    completedLevels.Add(level);
                    
                    // Additional Logic
                    HandlePrerequisites(level);
                    HandleLevelCompleted(level);
                }
            }
            
            private void HandlePrerequisites(LevelDataSO completedLevel)
            {
                foreach (string preReq in completedLevel.prerequisites)
                {
                    LevelDataSO preReqLevel = completedLevels.Find(l => l.levelName == preReq);
                   
                    if (preReqLevel != null && !preReqLevel.isCompleted)
                    {
                        MarkLevelAsCompleted(preReq);
                    }
                }
            }
            
            private void HandleLevelCompleted(LevelDataSO completedLevel)
            {
                if (completedLevel.deleteObject) {
                    
                }
            }
        }
    }
}
