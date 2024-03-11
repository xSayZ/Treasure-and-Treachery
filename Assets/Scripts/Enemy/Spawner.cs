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
                private int currentEnemies;
                private List<EnemyController> enemies;

#region Unity Functions
                
                private void Start()
                {
                    EnemyManager.OnEnemyDeath.AddListener(RemoveEnemy);
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
                        var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<EnemyController>();
                        enemies.Add(enemy);
                        EnemyManager.Instance.AddEnemy(enemy);
                        currentEnemies++;
                        
                        // Reset the timer after spawning an enemy
                        elapsedTime = 0f;
                    }
                }
                
                private bool CanSpawn()
                {
                    float spawnInterval = 60 / spawnRate;
                    elapsedTime += Time.deltaTime;
                    
                    return elapsedTime > spawnInterval
                        && EnemyManager.Instance.GetMaxEnemyCount() > EnemyManager.Instance.GetCurrentEnemyCount() 
                        && currentEnemies < maxEnemies 
                        && allowForSpawn;
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
