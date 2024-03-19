// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-06
// Author: b22feldy
// Description: Requires to be on a GameObject for spawning.
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Backend;
using Game.Player;
using UnityEngine;


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
                private List<int> playersInSpawner;
                
                public int currentEnemies;
                public List<EnemyController> enemies;

#region Unity Functions
                private void OnEnable()
                {
                    EnemyManager.OnEnemyDeath.AddListener(RemoveEnemy);
                    GameManager.OnPlayerDeath.AddListener(PlayerDied);
                }

                private void OnDisable()
                {
                    EnemyManager.OnEnemyDeath.RemoveListener(RemoveEnemy);
                    GameManager.OnPlayerDeath.RemoveListener(PlayerDied);
                }

                private void Start()
                {
                    playersInSpawner = new List<int>();
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
                        int _index = other.GetComponent<PlayerController>().PlayerData.playerIndex;
                        if (!playersInSpawner.Contains(_index))
                        {
                            playersInSpawner.Add(_index);
                        }
                        
                        if (playersInSpawner.Count > 0)
                        {
                            UpdateAllowedToSpawn(false);
                        }
                    }
                }

                private void OnTriggerExit(Collider other)
                {
                    if (other.CompareTag("Player"))
                    {
                        int _index = other.GetComponent<PlayerController>().PlayerData.playerIndex;
                        if (playersInSpawner.Contains(_index))
                        {
                            playersInSpawner.Remove(_index);
                        }
                        
                        if (playersInSpawner.Count == 0)
                        {
                            UpdateAllowedToSpawn(true);
                        }
                    }
                }
#endregion

                public void UpdateAllowedToSpawn(bool _allowForSpawn)
                {
                    allowForSpawn = _allowForSpawn;
                }

                private void PlayerDied(int _index)
                {
                    if (playersInSpawner.Contains(_index))
                    {
                        playersInSpawner.Remove(_index);
                    }
                    
                    if (playersInSpawner.Count == 0)
                    {
                        UpdateAllowedToSpawn(true);
                    }
                }

                private void SpawnEnemy()
                {
                    if (CanSpawn()) {
                        var _enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                        var _enemyController = _enemy.GetComponent<EnemyController>();
                        enemies.Add(_enemyController);
                        EnemyManager.Instance.AddEnemy(_enemyController);
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

                private void RemoveEnemy(EnemyController _enemy) {
                    if (!enemies.Contains(_enemy))
                        return;
                    
                    enemies.Remove(_enemy);
                    currentEnemies--;
                }
            }
        }
    }
}
