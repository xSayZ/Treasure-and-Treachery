// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-06
// Author: b22feldy
// Description: Requires to be on a GameObject for spawning.
// --------------------------------
// ------------------------------*/


using Game.Enemy;
using UnityEngine;

namespace Game
{
    namespace Backend
    {
        public class Spawner : MonoBehaviour
        {
            public bool allowForSpawn;

            public float spawnCooldown;

            private void Start() {
                spawnCooldown = SpawnManager.Instance.spawnTime;
            }

            private void Update() {
                spawnCooldown -= SpawnManager.Instance.spawnTimeModifier;
                
                if (spawnCooldown <= 0f) {
                    SpawnEnemy();
                    spawnCooldown = SpawnManager.Instance.spawnTime;
                }
            }
            private void SpawnEnemy() {
                Debug.Log("Entered SpawnEnemy");
                if (allowForSpawn && EnemyManager.Instance.GetCurrentEnemyCount() < EnemyManager.Instance.GetMaxEnemyCount()){
                    GameObject _spawnedEnemy =  Instantiate(SpawnManager.Instance.enemyPrefabs[UnityEngine.Random.Range(0, SpawnManager.Instance.enemyPrefabs.Length)], transform.position, Quaternion.identity);
                    EnemyManager.Instance.AddEnemy(_spawnedEnemy.GetComponent<EnemyController>());
                }
            }

        }
    }
}
