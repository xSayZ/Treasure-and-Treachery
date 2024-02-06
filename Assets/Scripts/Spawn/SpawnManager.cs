// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-06
// Author: b22feldy
// Description: SpawnManager for spawning enemies.
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace Game {
    namespace Backend {
        public class SpawnManager : MonoBehaviour
        {
            [Header("Time Curve")]
            [Tooltip("Curve for modifying the spawn time")]
            [SerializeField] public AnimationCurve timeCurve;
            
            [Header("Spawn Variables")]
            [Tooltip("Time between enemy spawns")]
            [SerializeField] private float spawnTime = 20f;
            private float spawnTimeModifier;
            [Tooltip("Debugging purposes")]
            [SerializeField] private float debugTime;
            
            [Header("Assignable Lists")]
            [Tooltip("Assign the enemy prefabs to be spawned")]
            [SerializeField] private GameObject[] enemyPrefabs;
            [Tooltip("Assign the spawn points for the enemies")]
            [SerializeField] private Spawner[] spawnPoints;

            [Header("Internal Variables")]
            private UnityEngine.Camera camera;

            private Timer timer;
            float percentage = 0f;
            
            private void Start()
            {
                camera = UnityEngine.Camera.main;
                StartCoroutine(EnemySpawn(spawnTime - spawnTimeModifier));
                timer = GameManager.Instance.GetComponent<Timer>();
            }

            private void Update()
            {
                // Get the percentage of the curve
                spawnTimeModifier = timeCurve.Evaluate(percentage);
                
                if (percentage < 1f)
                {
                    // Get the percentage of the current time
                    percentage = timer.GetCurrentTime() / GameManager.Instance.roundTime;
                }
                
                debugTime = spawnTime - spawnTimeModifier;
                
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    // Get the view position of the spawn point
                    Vector3 _viewPos = camera.WorldToViewportPoint(spawnPoints[i].transform.position);
                    
                    // Check if the spawn point is within the camera view
                    if(_viewPos.x >= 0 && _viewPos.x <= 1 && _viewPos.y >= 0 && _viewPos.y <= 1 && _viewPos.z > 0) {
                        spawnPoints[i].allowForSpawn = false;
                    } else {
                        if (EnemyManager.Instance.GetCurrentEnemyCount() < EnemyManager.Instance.GetMaxEnemyCount()) {
                            spawnPoints[i].allowForSpawn = true;
                        }
                    }
                }
            }

            // Coroutine for spawning enemies
            private IEnumerator EnemySpawn(float _spawnTime)
            {
                while (true)
                {
                    for (int i = 0; i < spawnPoints.Length; i++)
                    {
                        if (spawnPoints[i].allowForSpawn != true)
                            yield break;
                        
                        yield return new WaitForSeconds(_spawnTime);
                        SpawnEnemy(spawnPoints[i].transform);
                    }
                }
            }
            
            // Function for spawning enemies
            private void SpawnEnemy(Transform _spawnPointIndex)
            {
                int _enemyIndex = Random.Range(0, enemyPrefabs.Length);
                var _enemy = Instantiate(enemyPrefabs[_enemyIndex], _spawnPointIndex.position, Quaternion.identity);
                EnemyManager.Instance.AddEnemy(_enemy.GetComponent<Game.Enemy.EnemyController>());
            }
        }
        
    }
}
