// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-06
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;


namespace Game {
    namespace Backend {
        [CreateAssetMenu(fileName = "SpawnStatisticsSO", menuName = "Spawn/SpawnStatisticsSO")]
        public class SpawnStatisticsSO : ScriptableObject
        {
            public int maxEnemies;
            public int currentEnemies;
            [SerializeField] private GameObject[] prefabsToSpawn;

            private void OnEnable()
            {
                currentEnemies = 0;
            }

            public GameObject[] GetPrefabs() {
                return prefabsToSpawn;
            }
            
            public void AddEnemy() {
                currentEnemies++;
            }
            
            public void RemoveEnemy() {
                currentEnemies--;
            }
            
            public bool CanSpawn() {
                return currentEnemies < maxEnemies;
            }
            
            public void SetMaxEnemies(int max) {
                maxEnemies = max;
            }
        }
    }
}
