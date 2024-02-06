// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-05
// Author: b22feldy
// Description: This script is to be on an object to spawn enemies
// --------------------------------
// ------------------------------*/

using System.Collections;
using UnityEngine;


namespace Game {
    namespace Backend {
        public class Spawner : MonoBehaviour
        {
            UnityEngine.Camera cam; 
            
            [SerializeField] SpawnStatisticsSO spawnDictionary;
            
            [SerializeField] private float spawnTime = 20f;

            private bool allowForSpawn;
            
#region Unity Functions

            // Start is called before the first frame update
            void Start()
            {
                cam = UnityEngine.Camera.main;
                StartCoroutine(SpawnCoroutine(spawnTime));
                allowForSpawn = true;
            }
    
            // Update is called once per frame
            void Update()
            {
                Vector3 viewPos = cam.WorldToViewportPoint(this.transform.position);
                // If the spawner is in view, don't spawn anything
                if(viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0) {
                    allowForSpawn = false;
                    return;
                }

                allowForSpawn = true;
            }
#endregion

#region Public Functions

#endregion

#region Private Functions

#endregion

#region Coroutines

        private IEnumerator SpawnCoroutine(float _time)
        {
            while (true)
            {
                if (!allowForSpawn)
                {
                    yield return null;   
                }
            
                yield return new WaitForSeconds(_time);
            
                if(spawnDictionary.CanSpawn() && allowForSpawn) {
                    GameObject[] prefabs = spawnDictionary.GetPrefabs();
                    GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
                    Instantiate(prefab, this.transform.position, Quaternion.identity);
                    spawnDictionary.AddEnemy();
                }
            }
        }

#endregion
        }
    }
}
