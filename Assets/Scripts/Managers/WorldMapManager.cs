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
            [HideInInspector] public Quaternion carriageRotation;

            private void OnEnable() {
                Reset();
            }

            public void Reset() {
                carriagePosition = new Vector3(62.75653f, 2f, 143.5298f);
                carriageRotation = new Quaternion();
                levelToLoad = null;
                completedLevels.Clear();
            }

            public void MarkLevelAsCompleted(LevelDataSO _level) {
                if (_level == null)
                    return;
                
                _level.isCompleted = true;
                completedLevels.Add(_level);
                    
                // Additional Logic
                HandlePrerequisites(_level);
                HandleLevelCompleted(_level);
            }
            
            private void HandlePrerequisites(LevelDataSO _completedLevel)
            {
                foreach (LevelDataSO _preReq in _completedLevel.prerequisites)
                {
                    LevelDataSO _preReqLevel = completedLevels.Find(l => l.isCompleted == _preReq.isCompleted);
                   
                    if (_preReqLevel != null && !_preReqLevel.isCompleted)
                    {
                        MarkLevelAsCompleted(_preReqLevel);
                    }
                }
            }
            
            private void HandleLevelCompleted(LevelDataSO _completedLevel)
            {
                _completedLevel.isCompleted = true;
            }

        }
    }
}
