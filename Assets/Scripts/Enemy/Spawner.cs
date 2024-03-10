// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-06
// Author: b22feldy
// Description: Requires to be on a GameObject for spawning.
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game {
    namespace Enemy {
        namespace Systems {
            public class Spawner : MonoBehaviour
            {

                [Header("Spawner Settings")]
                [SerializeField] private float spawnRate; // the rate at which enemies spawn per minute
                [SerializeField] private int maxEnemies; // the maximum amount of enemies allowed to be spawned at once at this spawner
                [SerializeField] private GameObject enemyPrefab; // the enemy prefab to be spawned


                public bool allowSpawnInsideOfCamera;
                [HideInInspector] public bool allowForSpawn = true;
                
                private float elapsedTime;
                private int currentEnemies;

#region Unity Functions
                
                private void Start()
                {
                    allowForSpawn = true;
                }
                
                private void Update()
                {
                    SpawnEnemy();
                }

                private void OnTriggerEnter(Collider other)
                {
                    if (other.CompareTag("Player"))
                    {
                        UpdateAllowedToSpawn(false);
                    }
                }
                
                private void OnTriggerExit(Collider other)
                {
                    if (other.CompareTag("Player"))
                    {
                        UpdateAllowedToSpawn(true);
                    }
                }
#endregion
                
                public void UpdateAllowedToSpawn(bool _allowForSpawn)
                {
                    allowForSpawn = _allowForSpawn;
                }

                private void SpawnEnemy()
                {
                    if (CanSpawn()) {
                        if (EnemyManager.Instance.GetCurrentEnemyCount() < EnemyManager.Instance.GetMaxEnemyCount()) {
                            var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                            EnemyManager.Instance.AddEnemy(enemy.GetComponent<EnemyController>());
                            currentEnemies++;
                        
                            // Reset the timer after spawning an enemy
                            elapsedTime = 0f;
                        }
                    }
                }
                
                private bool CanSpawn()
                {
                    float spawnInterval = 60 / spawnRate;
                    elapsedTime += Time.deltaTime;
                    
                    return elapsedTime > spawnInterval
                        && EnemyManager.Instance.GetCurrentEnemyCount() < maxEnemies 
                        && currentEnemies < maxEnemies 
                        && allowForSpawn;
                }
            }
        }
    }
}
