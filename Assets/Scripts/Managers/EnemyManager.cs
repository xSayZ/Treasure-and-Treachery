// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-06
// Author: b22feldy
// Description: EnemyManager for managing enemies.
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Enemy;
using UnityEngine;
using UnityEngine.Events;


namespace Game {
    namespace Backend {
        public class EnemyManager : Singleton<EnemyManager>
        {
            [Header("Enemy Count")]
            [SerializeField] private int maxEnemies = 10;
            
            [HideInInspector] public List<EnemyController> enemies;
            
            public static UnityEvent<EnemyController> OnEnemyDeath = new UnityEvent<EnemyController>();
            
            // Start is called before the first frame update
            void Start()
            {
                OnEnemyDeath.AddListener(RemoveEnemy);
                
                enemies = new List<EnemyController>();
                
                foreach (var enemy in FindObjectsOfType<EnemyController>()) {
                    enemies.Add(enemy);
                }
            }
            
            public void AddEnemy(EnemyController _enemy)
            {
                enemies.Add(_enemy);
            }
            
            public void RemoveEnemy(EnemyController _enemy)
            {
                enemies.Remove(_enemy);
            }

            public int GetMaxEnemyCount()
            {
                return maxEnemies;
            }

            public int GetCurrentEnemyCount()
            {
                return enemies.Count;
            }
        }
    }
}
