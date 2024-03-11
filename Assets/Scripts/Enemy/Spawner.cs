// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-06
// Author: b22feldy
// Description: Requires to be on a GameObject for spawning.
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
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
                public int currentEnemies;
                public List<EnemyController> enemies;

#region Unity Functions
                
                private void Start()
                {
                    EnemyManager.OnEnemyDeath.AddListener(RemoveEnemy);
                    allowForSpawn = true;
                }
                
                private void Update()
                {
                    SpawnEnemy();
                    elapsedTime += Time.deltaTime;
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
                        var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                        var enemyController = enemy.GetComponent<EnemyController>();
                        enemies.Add(enemyController);
                        EnemyManager.Instance.AddEnemy(enemyController);
                        currentEnemies++;
                        
                        // Reset the timer after spawning an enemy
                        elapsedTime = 0f;
                    }
                }
                
                private bool CanSpawn()
                {
                    float _spawnInterval = 60 / spawnRate;

                    if (elapsedTime > _spawnInterval
                        && EnemyManager.Instance.GetMaxEnemyCount() > EnemyManager.Instance.GetCurrentEnemyCount()
                        && currentEnemies < maxEnemies
                        && allowForSpawn) return true;

                    return false;
                }
                
                private void RemoveEnemy(EnemyController _enemy)
                {
                    enemies.Remove(_enemy);
                    currentEnemies--;
                }
            }
        }
    }
}
