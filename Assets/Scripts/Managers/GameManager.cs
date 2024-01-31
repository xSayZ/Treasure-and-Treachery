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
using System;

namespace Game {
    namespace Backend {

        public class GameManager : MonoBehaviour
        {

            [Header("Setup")]
            [SerializeField] private GameObject playerPrefab;
            [SerializeField] private int numberOfPlayers;   

            [Header("Spawn Variables")]
            [SerializeField] private Transform spawnRingCenter;
            [Range(0.5f, 5f)]
            [SerializeField] private float spawnRingRadius;

            [Space]
            [SerializeField] private List<PlayerController> activePlayerControllers;

            [SerializeField] bool debug;

            #region Unity Functions
            private void OnDrawGizmos()
            {
                if (debug)
                {
                    Utility.Gizmos.GizmoSemiCircle.DrawWireArc(gameObject.transform.position, Vector3.forward, 360, spawnRingRadius, 50);
                }
            }

            void Awake()
            {
            }
            void Start()
            {
                SetupGame();
            }



            // Update is called once per frame
            void Update()
            {
                
            }
#endregion

#region Public Functions

#endregion

#region Private Functions

            private void SetupGame()
            {
                AddPlayers();
                SetObjective();
                
            }
            private void AddPlayers()
            {
                activePlayerControllers = new List<PlayerController>();

                for (int i = 0; i < numberOfPlayers; i++)
                {
                    Vector3 _spawnPosition = CalculatePositionInRing(i, numberOfPlayers);
                    Quaternion _spawnRotation = Quaternion.identity;

                    GameObject _spawnedPlayer = Instantiate(playerPrefab, _spawnPosition, _spawnRotation) as GameObject;
                    AddPlayersToActiveList(_spawnedPlayer.GetComponent<PlayerController>());

                    foreach (var newPlayer in activePlayerControllers)
                    {
                        try
                        {
                            newPlayer.Data.playerIndex = i;
                        }
                        catch (Exception e)
                        {
                            LogWarning("No PlayerData: "+e.Message);
                        }
                    }
                }
                

            }
            
            private void AddPlayersToActiveList(PlayerController _newPlayer)
            {
                activePlayerControllers.Add(_newPlayer);

            }
            
            private void SetObjective()
            {

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

            private void Log(string _msg)
            {
                if (!debug) return;
                Debug.Log("[GameManager]: "+_msg);
            }

            private void LogWarning(string _msg)
            {
                if (!debug) return;
                Debug.Log("[GameManager]: "+_msg);
            }
        }
    }
}
