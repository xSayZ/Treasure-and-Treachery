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

        public enum GameMode
        {
            SinglePlayer,
            LocalMultiplayer
        }

        public class GameManager : Singleton<GameManager>
        {
            public GameMode currentGameMode;

            [Header("Singleplayer")]
            [SerializeField] private GameObject inScenePlayer;

            [Header("Local Multiplayer")]
            [SerializeField] private GameObject playerPrefab;
            [SerializeField] private int numberOfPlayers;   

            [Header("Spawn Variables")]
            [SerializeField] private Transform spawnRingCenter;
            [Range(0.5f, 15f)]
            [SerializeField] private float spawnRingRadius;

            [Space]
            public List<GameObject> activePlayerControllers;
            [SerializeField] private bool isPaused;
            private PlayerController focusedPlayerController;

            [SerializeField] bool debug;

            #region Unity Functions
            private void OnDrawGizmos()
            {
                if (debug)
                {
                    Utility.Gizmos.GizmoSemiCircle.DrawWireArc(spawnRingCenter.transform.position, Vector3.forward, 360, spawnRingRadius, 50);
                }
            }

            void Awake()
            {

            }

            void Start()
            {
                isPaused = false;

                SetupBasedOnGameState();
            }

#endregion

#region Public Functions

#endregion

#region Private Functions

            void SetupBasedOnGameState()
            {
                switch (currentGameMode)
                {
                    case GameMode.SinglePlayer:
                        SetupSinglePlayer();
                        break;
                    case GameMode.LocalMultiplayer:
                        SetupLocalMultiplayer();
                        break;
                }
            }

            void SetupSinglePlayer()
            {
                activePlayerControllers = new List<GameObject>();

                if(inScenePlayer == true)
                {
                    AddPlayersToActiveList(inScenePlayer);
                }
            }

            void SetupLocalMultiplayer()
            {
                if(inScenePlayer == true)
                {
                    Destroy(inScenePlayer);
                }

                AddPlayers();
                SetObjective();
            }

            private void AddPlayers()
            {
                activePlayerControllers = new List<GameObject>();

                for (int i = 0; i < numberOfPlayers; i++)
                {
                    Vector3 _spawnPosition = CalculatePositionInRing(i, numberOfPlayers);
                    Quaternion _spawnRotation = Quaternion.identity;

                    GameObject _spawnedPlayer = Instantiate(playerPrefab, _spawnPosition, _spawnRotation) as GameObject;
                    _spawnedPlayer.GetComponent<PlayerController>();
                    AddPlayersToActiveList(_spawnedPlayer);

                    foreach (var newPlayer in activePlayerControllers)
                    {
                        try
                        {
                            newPlayer.GetComponent<PlayerController>().PlayerData.playerIndex = i;
                        }
                        catch (Exception e)
                        {
                            LogWarning("No PlayerData: "+e.Message);
                        }
                    }
                }
                

            }
            
            private void AddPlayersToActiveList(GameObject _newPlayer)
            {
                activePlayerControllers.Add(_newPlayer);
            }
            
            private void SetObjective()
            {

            }

            public void TogglePauseState(PlayerController newFocusedPlayerController)
            {
                focusedPlayerController = newFocusedPlayerController;

                isPaused = !isPaused;

                ToggleTimeScale();

                UpdateActivePlayerInputs();

                SwitchFocusedPlayerControlScheme();
            }

            private void UpdateActivePlayerInputs()
            {
                for (int i = 0; i < activePlayerControllers.Count; i++)
                {
                    if(activePlayerControllers[i] != focusedPlayerController)
                    {
                        //activePlayerControllers[i].SetInputActiveState(isPaused);
                    }
                }
            }

            void SwitchFocusedPlayerControlScheme()
            {
                switch (isPaused)
                {
                    case true:
                        focusedPlayerController.EnableEventControls();
                        break;
                    case false:
                        focusedPlayerController.EnableGamePlayControls();
                        break;
                }
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


            void ToggleTimeScale()
            {
                float _newTimeScale = 0f;

                switch(isPaused)
                {
                    case true:
                        _newTimeScale = 0f;
                        break;
                    case false:
                        _newTimeScale = 1f;
                        break;

                }

                Time.timeScale = _newTimeScale;
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
