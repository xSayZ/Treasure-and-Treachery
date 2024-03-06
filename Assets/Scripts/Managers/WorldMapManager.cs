// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-24
// Author: Felix
// Description: This script is responsible for managing the world map
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    namespace WorldMap {
        [CreateAssetMenu(fileName = "World Map Manager", menuName = "World Map/Manager", order = 0)]
        public class WorldMapManager : ScriptableObject
        {
            public List<LevelDataSO> completedLevels = new List<LevelDataSO>();
            
            public LevelDataSO levelToLoad;

            [HideInInspector] public Vector3 carriagePosition;

            private void OnEnable() {
                carriagePosition = new Vector3(62.75653f, 2f, 143.5298f);
                levelToLoad = null;
            }
            public void MarkLevelAsCompleted(LevelDataSO level) {
                if (level == null)
                    return;
                
                level.isCompleted = true;
                completedLevels.Add(level);
                    
                // Additional Logic
                HandlePrerequisites(level);
                HandleLevelCompleted(level);
            }
            
            private void HandlePrerequisites(LevelDataSO completedLevel)
            {
                foreach (LevelDataSO _preReq in completedLevel.prerequisites)
                {
                    LevelDataSO _preReqLevel = completedLevels.Find(l => l.isCompleted == _preReq.isCompleted);
                   
                    if (_preReqLevel != null && !_preReqLevel.isCompleted)
                    {
                        MarkLevelAsCompleted(_preReqLevel);
                    }
                }
            }
            
            private void HandleLevelCompleted(LevelDataSO completedLevel)
            {
                completedLevel.OnLevelCompleted.Invoke();
                completedLevel.isCompleted = true;
            }

        }
    }
}
