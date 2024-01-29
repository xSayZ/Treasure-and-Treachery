// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using Game.Player;


namespace Game {
    namespace Backend {
        public class GameManager : MonoBehaviour
        {

            public GameObject playerPrefab;
            public int numberOfPlayers;

            public Transform spawnRingCenter;
            [Range(0.5f, 5f)]
            public float spawnRingRadius;

            [SerializeField] private List<PlayerController> activePlayerControllers;



            #region Unity Functions
            void Awake()
            {
            }
            void Start()
            {
                activePlayerControllers = new List<PlayerController>();

                for (int i = 0; i < numberOfPlayers; i++)
                {
                    Vector3 _spawnPosition = CalculatePositionInRing(i, numberOfPlayers);
                    Quaternion _spawnRotation = Quaternion.identity;

                    // TODO: Add spawnPosition and spawnRotation
                    GameObject _spawnedPlayer = Instantiate(playerPrefab, _spawnPosition, _spawnRotation) as GameObject;
                    AddPlayersToActiveList(_spawnedPlayer.GetComponent<PlayerController>());
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


            private void AddPlayersToActiveList(PlayerController newPlayer)
            {
                activePlayerControllers.Add(newPlayer);
            }

            Vector3 CalculatePositionInRing(int positionID, int numberOfPlayers)
            {
                if (numberOfPlayers == 1)
                    return spawnRingCenter.position;

                float angle = (positionID) * Mathf.PI * 2 / numberOfPlayers;
                float x = Mathf.Cos(angle) * spawnRingRadius;
                float z = Mathf.Sin(angle) * spawnRingRadius;
                return spawnRingCenter.position + new Vector3(x, 0, z);
            }
            #endregion
        }
    }
}
