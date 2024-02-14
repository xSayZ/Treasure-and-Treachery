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
using UnityEngine.Events;

namespace Game {
    namespace Backend {

        public class GameManager : Singleton<GameManager>
        {
            public static UnityEvent<int> OnPlayerDeath = new UnityEvent<int>();
            
            [Range(0, 1200)]
            [Tooltip("Amount of time per round in seconds")]
            public float roundTime;
            public Timer timer;

            [Header("Local Multiplayer")]
            [Tooltip("Player Prefabs needs to be assigned")]
            [SerializeField] private GameObject playerPrefab;

            [Header("Spawn Variables")]
            [Tooltip("Assign the spawn point where players are to be instantiated from")]
            [SerializeField] private Transform spawnRingCenter;
            [Range(0.5f, 15f)]
            [SerializeField] private float spawnRingRadius;
            [Space]
            [SerializeField] private PlayerData[] activePlayerPlayerData;
            
            public List<PlayerController> activePlayerControllers;
            private PlayerController focusedPlayerController;

            [Header("Debug")]
            [SerializeField] bool debug;
            private bool isPaused;
            
            // Temporary
            [HideInInspector] public int nextSceneBuildIndex;

            #region Unity Functions
            private void OnDrawGizmos()  {
                if (debug)  {
                    Utility.Gizmos.GizmosExtra.DrawCircle(spawnRingCenter.position, spawnRingRadius);
                }
            }
            
            void Start()  {
                OnPlayerDeath.AddListener(RemovePlayerFromCurrentPlayersList);
                isPaused = false;
                
                timer = gameObject.AddComponent<Timer>();
                timer.StartTimer(roundTime);
                
                SetupLocalMultiplayer();
                
            }
#endregion

#region Private Functions
            
            private void SetupLocalMultiplayer() {
                DestroyExistingPlayerInstances();
                AddPlayers();
                SetupActivePlayers();
            }
            private static void DestroyExistingPlayerInstances() {

                GameObject[] _playersAlreadyInScene = GameObject.FindGameObjectsWithTag("Player");
                if (_playersAlreadyInScene.Length >= 1) {
                    foreach (GameObject _playerInstances in _playersAlreadyInScene) {
                        Destroy(_playerInstances);
                    }
                }
            }

            private void AddPlayers() {
                activePlayerControllers = new List<PlayerController>();

                string[] _controllers = Input.GetJoystickNames();
                if (_controllers.Length == 0)
                {
                    LogWarning("No controllers detected");
                    SpawnPlayers(0, 1);
                }
                
                for (int i = 0; i < _controllers.Length; i++) {
                    SpawnPlayers(i, _controllers.Length);
                }
            }
            
            private void SetupActivePlayers() {
                for (int i = 0; i < activePlayerControllers.Count; i++) {
                    activePlayerControllers[i].SetupPlayer(i);
                }
            }

            public void TogglePauseState(PlayerController newFocusedPlayerController) {
                focusedPlayerController = newFocusedPlayerController;
                isPaused = !isPaused;

                ToggleTimeScale();
                UpdateActivePlayerInputs();
                SwitchFocusedPlayerControlScheme();
            }

            private void UpdateActivePlayerInputs() {
                for (int i = 0; i < activePlayerControllers.Count; i++) {
                    if (activePlayerControllers[i] != focusedPlayerController) {
                        activePlayerControllers[i].SetInputPausedState(isPaused);
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

            private void SpawnPlayers(int _playerID, int _numberOfPlayers) {
                Vector3 _spawnPosition = CalculatePositionInRing(_playerID, _numberOfPlayers);
                Quaternion _spawnRotation = Quaternion.identity;
                    
                GameObject _spawnedPlayer = Instantiate(playerPrefab, _spawnPosition, _spawnRotation);
                AddPlayersToActiveList(_spawnedPlayer.GetComponent<PlayerController>());
                
                // Get PlayerData from List and assign it based on playerID
                PlayerData _playerData  = activePlayerPlayerData[_playerID];
                _spawnedPlayer.GetComponent<PlayerController>().PlayerData = _playerData;
            }
            
            private void AddPlayersToActiveList(PlayerController newPlayer) {
                activePlayerControllers.Add(newPlayer);
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
            
            private void RemovePlayerFromCurrentPlayersList(int _playerID) {
                activePlayerControllers.RemoveAt(_playerID);
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
