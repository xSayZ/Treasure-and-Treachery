// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-05
// Author: b22feldy
// Description: This script is to be on an object to spawn enemies
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Backend {
        public class Spawner : MonoBehaviour
        {

            // TODO: Prefabs for what to spawn at this spawner
            // ScriptableObject for amount of enemies in the scene

            [SerializeField] private GameObject[] prefabsToSpawn;

#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                for (int i = 0; i < 100; i++)
                {
                    Instantiate(prefabsToSpawn[0], transform.position, Quaternion.identity);
                }
            }
    
            // Update is called once per frame
            void Update()
            {
            }
#endregion

#region Public Functions

#endregion

#region Private Functions

#endregion
        }
    }
}
