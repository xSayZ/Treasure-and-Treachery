// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-06
// Author: b22feldy
// Description: SpawnManager for spawning enemies.
// --------------------------------
// ------------------------------*/


using System;
using UnityEngine;

using Random = UnityEngine.Random;


namespace Game {
    namespace Enemy {
        public class SpawnManager : Singleton<SpawnManager>
        {
            [Header("Internal Variables")]
            private UnityEngine.Camera sceneCamera;
            private Systems.Spawner[] spawnPoints;
            
            private void Awake()
            {
                sceneCamera = UnityEngine.Camera.main;
                spawnPoints = Array.Empty<Systems.Spawner>();
                spawnPoints = FindObjectsOfType<Systems.Spawner>();
            }

            private void Update()
            {
                foreach (var _spawnPoint in spawnPoints)
                {
                    if (_spawnPoint.allowSpawnInsideOfCamera) continue;
                    // Get the view position of the spawn point
                    Vector3 _viewPos = sceneCamera.WorldToViewportPoint(_spawnPoint.transform.position);
                    
                    // Check if the spawn point is within the camera view
                    if(_viewPos.x >= 0 && _viewPos.x <= 1 && _viewPos.y >= 0 && _viewPos.y <= 1 && _viewPos.z > 0) {
                        _spawnPoint.allowForSpawn = false;
                    } else {
                        _spawnPoint.allowForSpawn = true;
                    }
                }
            }
            
            // Function for spawning enemies
            private void SpawnEnemy(Transform _spawnPointIndex)
            {

            }
        }
        
    }
}
