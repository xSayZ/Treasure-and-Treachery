// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: GameManager controlling the Gameloop inside of the levels
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.CharacterSelection;
using UnityEngine;
using Game.Player;
using Game.Quest;
using UnityEngine.Events;
using Game.WorldMap;
using UnityEngine.InputSystem;

namespace Game {
    namespace Backend {

        /// <summary>
        /// The GameManager class serves as the central hub for managing key game functionalities and configurations.
        /// It encompasses settings for rounds, timers, local multiplayer, player spawning, and player management.
        /// </summary>
        public class GameManager : Singleton<GameManager>
        {
            [Header("Setup")]
            [SerializeField] private WorldMapManager worldMapManager;
            
            [Header("Round Settings")]
            [Range(0, 1200), Tooltip("Amount of time per round in seconds")]
            [SerializeField] public float roundTime;
            
            [Header("Spawn Variables")]
            [Tooltip("Assign the spawn point where players are to be instantiated from")]
            [SerializeField] public Transform spawnRingCenter;
            [Range(0.5f, 15f), Space]
            [SerializeField] private float spawnRingRadius;
            
            [Header("Debug")]
            [SerializeField] private bool showDebug;
            [SerializeField] private bool autoDetectPlayers = true;
            [Range(1, 4)]
            [SerializeField] private int playersToSpawn = 1;
            [Tooltip("Player Prefabs needs to be assigned")]
            [SerializeField] private List<GameObject> playerVariants = new List<GameObject>();
            
            public Dictionary<int, PlayerController> ActivePlayerControllers;
            [HideInInspector] public Utility.Timer timer;
            
            public static UnityEvent<int> OnPlayerDeath = new UnityEvent<int>();
            
            private bool isPaused;
            private LevelDataSO level;
            private PlayerController focusedPlayerController;

#region Unity Functions
            private void OnDrawGizmos() 
            {
                if (showDebug)
                {
                    Utility.Gizmos.GizmosExtra.DrawCircle(spawnRingCenter.position, spawnRingRadius);
                }
            }

            private void Start()
            {
                level = worldMapManager.levelToLoad;
                
                if (CharacterSelect.selectedCharacters.Count == 0)
                {
                    if (autoDetectPlayers)
                    {
                        playersToSpawn = Input.GetJoystickNames().Length + 1; // +1 is for the keyboard
                        
                        if (playersToSpawn > 4)
                        {
                            playersToSpawn = 4;
                        }
                    }
                }
                
                if (!level) // Null check
                {
                    SetupLocalMultiplayer();
                    LogWarning("Level variable is not set, spawning players anyways");
                }
                else if (level.isGameplayScene)
                {
                    SetupLocalMultiplayer();
                }
            }
#endregion

#region Public Functions 
            /// <summary>
            /// Toggle the pause state of the game
            /// It is toggled off by default
            /// </summary>
            /// <param name="newFocusedPlayerController">The PlayerController assigned as the controller.</param>
            public void TogglePauseState(PlayerController newFocusedPlayerController)
            {
                focusedPlayerController = newFocusedPlayerController;
                isPaused = !isPaused;
                
                ToggleTimeScale();
                UpdateActivePlayerInputs();
                SwitchFocusedPlayerControlScheme();
            }
#endregion

#region Private Functions
            /// <summary>
            /// Setup local multiplayer for the current scene
            /// If this is not called, no players will be spawned
            /// </summary>
            private void SetupLocalMultiplayer()
            {
                QuestManager.SetUp();
                
                OnPlayerDeath.AddListener(RemovePlayerFromCurrentPlayersList);
                isPaused = false;
                
                // Setup timer
                if (!timer)
                {
                    timer = gameObject.AddComponent<Utility.Timer>();
                }
                timer.StartTimer(roundTime);
                
                DestroyExistingPlayerInstances();
                AddPlayers();
                SetupActivePlayers();
            }

            /// <summary>
            /// Destroy any existing player instances in the scene
            /// </summary>
            private static void DestroyExistingPlayerInstances()
            {
                GameObject[] _playersAlreadyInScene = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject _playerInstances in _playersAlreadyInScene)
                { 
                    Destroy(_playerInstances);
                }
            }

            /// <summary>
            /// Add players to the scene
            /// </summary>
            private void AddPlayers()
            {
                ActivePlayerControllers = new Dictionary<int, PlayerController>();
                
                if (CharacterSelect.selectedCharacters.Count == 0)
                {
                    for (int i = 0; i < playersToSpawn; i++)
                    {
                        SpawnPlayers(playerVariants[i], playerVariants[i].GetComponent<PlayerController>().PlayerData.playerIndex, playersToSpawn);
                    }
                }
                else
                {
                    foreach (KeyValuePair<InputDevice, PlayerData> _kvp in CharacterSelect.selectedCharacters)
                    {
                        SpawnPlayers(_kvp.Value.playerPrefab, _kvp.Value.playerIndex, CharacterSelect.selectedCharacters.Count);
                    }
                }
            }

            /// <summary>
            /// Setup the active players in the scene
            /// </summary>
            private void SetupActivePlayers()
            {
                if (CharacterSelect.selectedCharacters.Count == 0)
                {
                    foreach (KeyValuePair<int, PlayerController> _kvp in ActivePlayerControllers)
                    {
                        _kvp.Value.SetupPlayer(null);
                    }
                }
                else
                {
                    foreach (KeyValuePair<InputDevice, PlayerData> _kvp in CharacterSelect.selectedCharacters)
                    {
                        ActivePlayerControllers[_kvp.Value.playerIndex].SetupPlayer(_kvp.Key);
                    }
                }
            }

            private void UpdateActivePlayerInputs()
            {
                foreach (KeyValuePair<int, PlayerController> _kvp in ActivePlayerControllers)
                {
                    if (_kvp.Value != focusedPlayerController)
                    {
                        _kvp.Value.SetInputPausedState(isPaused);
                    }
                }
            }

            private void SwitchFocusedPlayerControlScheme()
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

            /// <summary>
            /// Spawns the players in the scene.
            /// </summary>
            /// <param name="_playerID">The unique identifier of the player to be spawned.</param>
            /// <param name="_numberOfPlayers">The total number of players to be spawned.</param>
            private void SpawnPlayers(GameObject _playerPrefab, int _playerID, int _numberOfPlayers)
            {
                Vector3 _spawnPosition = CalculatePositionInRing(_playerID, _numberOfPlayers);
                Quaternion _spawnRotation = Quaternion.identity;
                
                GameObject _spawnedPlayer = Instantiate(_playerPrefab, _spawnPosition, _spawnRotation);
                AddPlayersToActiveList(_playerID, _spawnedPlayer.GetComponent<PlayerController>());
            }

            /// <summary>
            /// Adds a player to the active player list.
            /// </summary>
            /// <param name="_playerIndex">The unique identifier of the player added to the list.</param>
            /// <param name="newPlayer">The PlayerController assigned to said player.</param>
            private void AddPlayersToActiveList(int _playerIndex, PlayerController newPlayer)
            {
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
            private Vector3 CalculatePositionInRing(int positionID, int numberOfPlayers)
            {
                // If there is only one player, return the center of the ring
                if (numberOfPlayers == 1)
                    return spawnRingCenter.position;
                
                // Calculate the angle
                float _angle = (positionID) + Mathf.PI * 2 / numberOfPlayers;
                
                // Calculate the position
                float _x = Mathf.Cos(_angle) * spawnRingRadius;
                float _z = Mathf.Sin(_angle) * spawnRingRadius;
                
                // Return the position
                return spawnRingCenter.position + new Vector3(_x, 0, _z);
            }

            private void ToggleTimeScale()
            {
                float _newTimeScale = isPaused switch {
                    true => 0f,
                    false => 1f
                };
                
                Time.timeScale = _newTimeScale;
            }

            private void RemovePlayerFromCurrentPlayersList(int _playerID)
            {
                ActivePlayerControllers.Remove(_playerID);
            }

            private void DestroyExistingInputInstances()
            {
                GameObject[] _inputsAlreadyInScene = GameObject.FindGameObjectsWithTag("Input");
                if (_inputsAlreadyInScene.Length >= 1)
                {
                    foreach (GameObject _inputInstances in _inputsAlreadyInScene)
                    {
                        Destroy(_inputInstances);
                    }
                }
            }
#endregion

            private void Log(string msg) {
                if (!showDebug) return;
                Debug.Log("[GameManager]: "+msg);
            }

            private void LogWarning(string msg) {
                if (!showDebug) return;
                Debug.LogWarning("[GameManager]: "+msg);
            }
        }
    }
}
