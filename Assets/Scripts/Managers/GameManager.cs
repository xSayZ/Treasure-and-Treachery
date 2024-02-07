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
using System.Linq;

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
            [Range(0, 1200)]
            [Tooltip("Amount of time per round in seconds")]
            public float roundTime;

            [Header("Singleplayer")]
            [Tooltip("Player inside scene needs to be assigned")]
            [SerializeField] private GameObject inScenePlayer;

            [Header("Local Multiplayer")]
            [Tooltip("Player Prefabs needs to be assigned")]
            [SerializeField] private GameObject playerPrefab;
            [Range(1, 4)]
            [Tooltip("Assign amount of players to be spawned")]
            [SerializeField] private int numberOfPlayers;

            [Header("Spawn Variables")]
            [Tooltip("Assign the spawn point where players are to be instantiated from")]
            [SerializeField] private Transform spawnRingCenter;
            [Range(0.5f, 15f)]
            [SerializeField] private float spawnRingRadius;


            [Space]
            
            public List<GameObject> activePlayerControllers;
            private PlayerController focusedPlayerController;

            [Header("Debug")]
            [SerializeField] bool debug;
            private bool isPaused;

            #region Unity Functions
            private void OnDrawGizmos()  {
                if (debug)  {
                    Utility.Gizmos.GizmosExtra.DrawCircle(spawnRingCenter.position, spawnRingRadius);
                }
            }
            void Start()  {
                isPaused = false;

                SetupBasedOnGameState();
            }
            
#endregion

#region Private Functions
            private void SetupBasedOnGameState() {
                switch (currentGameMode)  {
                    case GameMode.SinglePlayer:
                        SetupSinglePlayer();
                        break;
                    case GameMode.LocalMultiplayer:
                        SetupLocalMultiplayer();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(); 
                }

                Timer _timer = gameObject.AddComponent<Timer>();
                _timer.StartTimer(roundTime);
            }

            private void SetupSinglePlayer() {
                activePlayerControllers = new List<GameObject>();

                if(inScenePlayer == true)  {
                    AddPlayersToActiveList(inScenePlayer);
                }
            }

            private void SetupLocalMultiplayer() {
                if(inScenePlayer == true)
                {
                    Destroy(inScenePlayer);
                }

                AddPlayers();
            }

            private void AddPlayers() {
                activePlayerControllers = new List<GameObject>();

                string[] _controllers = Input.GetJoystickNames();
                for (int i = 0; i < numberOfPlayers; i++) {
                    if (i >= _controllers.Length) {
                        LogWarning("No controller found for player " + i);
                        continue;
                    }
                    
                    Vector3 _spawnPosition = CalculatePositionInRing(i, numberOfPlayers);
                    Quaternion _spawnRotation = Quaternion.identity;

                    var _spawnedPlayer = Instantiate(playerPrefab, _spawnPosition, _spawnRotation) as GameObject;
                    _spawnedPlayer.GetComponent<PlayerController>();
                    AddPlayersToActiveList(_spawnedPlayer);
                    
                    // Set the player index
                    foreach (GameObject _newPlayer in activePlayerControllers)
                    {
                        try
                        {
                            _newPlayer.GetComponent<PlayerController>().PlayerData.playerIndex = i;
                        }
                        catch (Exception e)
                        {
                            LogWarning("No PlayerData: "+e.Message);
                        }
                    }
                }

            }
            
            private void AddPlayersToActiveList(GameObject newPlayer) {
                activePlayerControllers.Add(newPlayer);
            }

            public void TogglePauseState(PlayerController newFocusedPlayerController) {
                focusedPlayerController = newFocusedPlayerController;
                isPaused = !isPaused;

                ToggleTimeScale();
                UpdateActivePlayerInputs();
                SwitchFocusedPlayerControlScheme();
            }

            private void UpdateActivePlayerInputs() {
                foreach (GameObject _t in activePlayerControllers.Where(t => t != focusedPlayerController.gameObject)) {
                    _t.GetComponent<PlayerController>().SetInputPausedState(isPaused);
                }
            }

            private void SwitchFocusedPlayerControlScheme() {
                switch (isPaused) {
                    case true:
                        focusedPlayerController.EnableEventControls();
                        break;
                    case false:
                        focusedPlayerController.EnableGamePlayControls();
                        break;
                }
            }
            // Calculate the position of the players in the ring
            private Vector3 CalculatePositionInRing(int positionID, int numberOfPlayers) {
                // If there is only one player, return the center of the ring
                if (numberOfPlayers == 1)
                    return spawnRingCenter.position;

                // Calculate the angle
                float _angle = (positionID) * Mathf.PI * 2 / numberOfPlayers;
                // Calculate the position
                float _x = Mathf.Cos(_angle) * spawnRingRadius;
                float _z = Mathf.Sin(_angle) * spawnRingRadius;
                // Return the position
                return spawnRingCenter.position + new Vector3(_x, 0, _z);
            }
            private void ToggleTimeScale() {
                float _newTimeScale = isPaused switch {
                    true => 0f,
                    false => 1f
                };

                Time.timeScale = _newTimeScale;
            }
            #endregion

            private void Log(string msg) {
                if (!debug) return;
                Debug.Log("[GameManager]: "+msg);
            }

            private void LogWarning(string msg) {
                if (!debug) return;
                Debug.Log("[GameManager]: "+msg);
            }
        }
    }
}
