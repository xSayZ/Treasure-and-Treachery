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
using UnityEngine.UI;
using UnityEditor.SceneManagement;

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
            private void OnDrawGizmos()
            {
                if (debug)
                {
                    Utility.Gizmos.GizmosExtra.DrawCircle(spawnRingCenter.position, spawnRingRadius);
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

            private void Update()
            {
                
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

                Timer timer = gameObject.AddComponent<Timer>();
                timer.StartTimer(roundTime);
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

                var controllers = Input.GetJoystickNames();
                for (int i = 0; i < numberOfPlayers; i++)
                {
                    Vector3 _spawnPosition = CalculatePositionInRing(i, numberOfPlayers);
                    Quaternion _spawnRotation = Quaternion.identity;

                    GameObject _spawnedPlayer = Instantiate(playerPrefab, _spawnPosition, _spawnRotation) as GameObject;
                    _spawnedPlayer.GetComponent<PlayerController>();
                    AddPlayersToActiveList(_spawnedPlayer);

                    Log("Added " + _spawnedPlayer + " to the scene");

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
                        activePlayerControllers[i].GetComponent<PlayerController>().SetInputActiveState(isPaused);
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
