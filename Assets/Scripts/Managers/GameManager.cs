// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: GameManager controlling the Gameloop inside of the levels
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Game.Quest;
using UnityEngine.Events;
using Game.UI;

namespace Game {
    namespace Backend {

        /// <summary>
        /// The GameManager class serves as the central hub for managing key game functionalities and configurations.
        /// It encompasses settings for rounds, timers, local multiplayer, player spawning, and player management.
        /// </summary>
        public class GameManager : Singleton<GameManager>
        {
            public static UnityEvent<int> OnPlayerDeath = new UnityEvent<int>();

            [Header("Round Settings")]
            [Range(0, 1200), Tooltip("Amount of time per round in seconds")]
            public float roundTime;
    
            [Header("Timer")]
            public Timer timer;

            [Header("Local Multiplayer")]
            [Tooltip("Player Prefabs needs to be assigned")]
            public List<GameObject> playerVariants;

            [Header("Spawn Variables")]
            [Tooltip("Assign the spawn point where players are to be instantiated from")]
            [SerializeField] public Transform spawnRingCenter;
    
            [Range(0.5f, 15f), Space]
            [SerializeField] private float spawnRingRadius;

            [Range(1, 4)]
            [SerializeField] private int playersToSpawn = 1;

            [Header("Player Management")]
            public Dictionary<int, PlayerController> ActivePlayerControllers;
            private PlayerController focusedPlayerController;

            [Header("Debug")]
            [SerializeField] private bool debug;
            private bool isPaused;

            // TODO: Remove when WorldMap is implemented
            [HideInInspector] public static int NextSceneBuildIndex;
            
            

#region Unity Functions
            private void OnDrawGizmos()  {
                if (debug)  {
                    Utility.Gizmos.GizmosExtra.DrawCircle(spawnRingCenter.position, spawnRingRadius);
                }
            }

            private void Start()
            {
                if (CharacterSelectHandler.playerList.Count == 0) {
                    SetupLocalMultiplayer();
                }
            }

#endregion

#region Public Functions

            /// <summary>
            /// Setup local multiplayer for the current scene
            /// If this is not called, no players will be spawned
            /// </summary>
            public void SetupLocalMultiplayer() {
                QuestManager.SetUp();
                
                OnPlayerDeath.AddListener(RemovePlayerFromCurrentPlayersList);
                isPaused = false;
                
                timer = gameObject.AddComponent<Timer>();
                timer.StartTimer(roundTime);
                
                DestroyExistingPlayerInstances();
                AddPlayers();
                SetupActivePlayers();
            }
            
            /// <summary>
            /// Toggle the pause state of the game
            /// It is toggled off by default
            /// </summary>
            /// <param name="newFocusedPlayerController">The PlayerController assigned as the controller.</param>
            public void TogglePauseState(PlayerController newFocusedPlayerController) {
                focusedPlayerController = newFocusedPlayerController;
                isPaused = !isPaused;

                ToggleTimeScale();
                UpdateActivePlayerInputs();
                SwitchFocusedPlayerControlScheme();
            }
#endregion            

#region Private Functions
            /// <summary>
            /// Destroy any existing player instances in the scene
            /// </summary>
            private static void DestroyExistingPlayerInstances() {

                GameObject[] _playersAlreadyInScene = GameObject.FindGameObjectsWithTag("Player");
                if (_playersAlreadyInScene.Length >= 1) {
                    foreach (GameObject _playerInstances in _playersAlreadyInScene) {
                        Destroy(_playerInstances);
                    }
                }
            }

            /// <summary>
            /// Add players to the scene
            /// </summary>
            private void AddPlayers() {
                
                ActivePlayerControllers = new Dictionary<int, PlayerController>();

                if (CharacterSelectHandler.playerList.Count == 0) {
                    LogWarning("No controllers detected; spawning default player.");
                    for (int i = 0; i < playersToSpawn; i++) {
                        SpawnPlayers(i, playersToSpawn);
                    }
                } else {
                    for (int i = 0; i < CharacterSelectHandler.playerList.Count; i++) {
                        SpawnPlayers(i, CharacterSelectHandler.playerList.Count);
                    }
                }
            }
            
            /// <summary>
            /// Setup the active players in the scene
            /// </summary>
            private void SetupActivePlayers()
            {
                for (int i = 0; i < ActivePlayerControllers.Count; i++)
                {
                    ActivePlayerControllers[i].SetupPlayer(i);
                }
            }

            private void UpdateActivePlayerInputs() {
                for (int i = 0; i < ActivePlayerControllers.Count; i++) {
                    if (ActivePlayerControllers[i] != focusedPlayerController) {
                        ActivePlayerControllers[i].SetInputPausedState(isPaused);
                    }
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
            
            /// <summary>
            /// Spawns the players in the scene.
            /// </summary>
            /// <param name="_playerID">The unique identifier of the player to be spawned.</param>
            /// <param name="_numberOfPlayers">The total number of players to be spawned.</param>
            private void SpawnPlayers(int _playerID, int _numberOfPlayers) {
                Vector3 _spawnPosition = CalculatePositionInRing(_playerID, _numberOfPlayers);
                Quaternion _spawnRotation = Quaternion.identity;
                
                GameObject _spawnedPlayer = Instantiate(playerVariants[_playerID], _spawnPosition, _spawnRotation);
                AddPlayersToActiveList(_playerID, _spawnedPlayer.GetComponent<PlayerController>());
            }
            
            /// <summary>
            /// Adds a player to the active player list.
            /// </summary>
            /// <param name="_playerIndex">The unique identifier of the player added to the list.</param>
            /// <param name="newPlayer">The PlayerController assigned to said player.</param>
            private void AddPlayersToActiveList(int _playerIndex, PlayerController newPlayer) {
                ActivePlayerControllers[_playerIndex] = newPlayer;
            }
            
            /// <summary>
            /// Calculates and returns the position of a player within a ring arrangement.
            /// </summary>
            /// <param name="positionID">The unique identifier of the player's position within the ring.</param>
            /// <param name="numberOfPlayers">The total number of players participating in the ring.</param>
            /// <returns>
            /// If there is only one player, returns the center position of the ring.
            /// Otherwise, calculates the player's position based on the position ID and total number of players,
            /// and returns the position relative to the ring's center.
            /// </returns>
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
            
            private void RemovePlayerFromCurrentPlayersList(int _playerID) {
                ActivePlayerControllers.Remove(_playerID);
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
