// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Carriage for leaving level
// --------------------------------
// ------------------------------*/

using System;
using Game.Audio;
using Game.Backend;
using Game.Core;
using Game.Managers;
using UnityEngine;
using Game.Quest;
using Game.Player;
using Game.WorldMap;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Game {
    namespace Scenes {
        public class CarriageBehaviour : MonoBehaviour, IInteractable, IDamageable {
            [Header("Setup")]
            [SerializeField] private CarriageData carriageData;
            [SerializeField] private GameObject interactionUI;
            [SerializeField] private GameObject playerTeleportPosition;
            [SerializeField] private Slider healthBar;
            [SerializeField] private GameObject lostCanvas;

            [Header("Settings")]
            [SerializeField] private string allPlayersDiedText;
            [SerializeField] private string carriageDestroyedText;
            [SerializeField] private string timeRanOutText;
            
            [Header("Audio")]
            [SerializeField] private InteractablesAudio interactablesAudio;
            
            // Interaction variables
            public bool[] CanInteractWith { get; set; }
            public bool[] PlayersThatWantsToInteract { get; set; }
            public Transform InteractionTransform { get; set; }
            
            // Health variables
            public int Health { get; set; }
            public bool Invincible { get; set; }
            
            private bool canLeave = true;
            private int playersInCarriage;
            private bool levelOver;
            private Utility.Timer timer;

#region Unity Functions
            private void OnEnable()
            {
                QuestManager.OnRequiredQuestRegistered.AddListener(RequiredQuestRegistered);
                QuestManager.OnAllRequiredQuestsCompleted.AddListener(AllRequiredQuestsCompleted);
            }

            private void OnDisable()
            {
                QuestManager.OnRequiredQuestRegistered.RemoveListener(RequiredQuestRegistered);
                QuestManager.OnAllRequiredQuestsCompleted.RemoveListener(AllRequiredQuestsCompleted);
            }

            private void Awake()
            {
                CanInteractWith = new bool[4]; // Hard coded to max 4 players
                for (int i = 0; i < CanInteractWith.Length; i++)
                {
                    CanInteractWith[i] = true;
                }
                PlayersThatWantsToInteract = new bool[4]; // Hard coded to max 4 players
                InteractionTransform = transform;
                
                Health = carriageData.currentHealth;
                UpdateHealthBar();
            }

            private void Start() {
                timer = GameManager.Instance.timer;
            }

            private void Update()
            {
                if (timer.GetCurrentTime() >= GameManager.Instance.roundTime && !levelOver)
                {
                    LevelLost(timeRanOutText);
                }
                
                if (levelOver)
                {
                    return;
                }
                
                // All players are dead
                if (GameManager.Instance.ActivePlayerControllers.Count == 0)
                {
                    if (!levelOver) {
                        LevelLost(allPlayersDiedText);
                    }
                }
                
                // All players in carriage
                else if (playersInCarriage >= GameManager.Instance.ActivePlayerControllers.Count)
                {
                    LevelCompleted();
                }
            }
#endregion

#region Public Functions
            public void Interact(int _playerIndex, bool _start)
            {
                if (_start && canLeave)
                {
                    CanInteractWith[_playerIndex] = false;
                    
                    PlayerController _player = GameManager.Instance.ActivePlayerControllers[_playerIndex];
                    _player.GetComponent<PlayerController>().SetInputPausedState(true);
                    Transform _transform = _player.transform;
                    _transform.position = playerTeleportPosition.transform.position;
                    _transform.localScale = new Vector3(0,0,0);
                    
                    
                    playersInCarriage++;
                    if (playersInCarriage >= GameManager.Instance.ActivePlayerControllers.Count)
                    {
                        LevelCompleted();
                    }
                }
            }

            public void ToggleInteractionUI(int _playerIndex, bool _active)
            {
                PlayersThatWantsToInteract[_playerIndex] = _active;
                
                bool _displayUI = false;
                for (int i = 0; i < PlayersThatWantsToInteract.Length; i++)
                {
                    if (PlayersThatWantsToInteract[i])
                    {
                        _displayUI = true;
                    }
                }
                
                interactionUI.SetActive(_displayUI);
            }

            public void Death()
            {
                carriageData.currentHealth = 0;
                
                healthBar.value = 0;
                
                if (!levelOver)
                {
                    LevelLost(carriageDestroyedText);
                }
            }

            public void DamageTaken(Vector3 _damagePosition, float _knockbackForce)
            {
                carriageData.currentHealth = Health;
                
                UpdateHealthBar();
                
                try
                {
                    interactablesAudio.CarriageHitAudio(gameObject);
                }
                catch (Exception e)
                {
                    Debug.LogError("[{PlayerController}]: Error Exception " + e);
                }
            }
#endregion

#region Private Functions
            private void RequiredQuestRegistered()
            {
                canLeave = false;
                
                for (int i = 0; i < CanInteractWith.Length; i++)
                {
                    CanInteractWith[i] = false;
                }
            }

            private void AllRequiredQuestsCompleted()
            {
                canLeave = true;
                
                for (int i = 0; i < CanInteractWith.Length; i++)
                {
                    CanInteractWith[i] = true;
                }
            }

            private void LevelCompleted()
            {
                var _levelManager = LevelManager.Instance;
                var _gameManager = GameManager.Instance;
                LevelDataSO _levelData = _levelManager.worldMapManager.levelToLoad;

                levelOver = true; 
                _gameManager.LevelComplete();
                _levelManager.worldMapManager.MarkLevelAsCompleted(_levelData);
                _levelManager.LoadScoreScreen();
                
            }

            private void LevelLost(string _reason)
            {
                levelOver = true;
                KillAllPlayers();
                lostCanvas.SetActive(true);
                lostCanvas.GetComponent<LostCanvas>().Setup(_reason);
                AudioMananger.Instance.GameOverStinger();
                
                // Reset carriage health
                Invincible = true;
                carriageData.currentHealth = carriageData.startingHealth;
            }

            private void UpdateHealthBar()
            {
                float _currentProgress = carriageData.currentHealth / (float)carriageData.startingHealth;
                healthBar.value = _currentProgress;
            }
            
            private void KillAllPlayers() {

                for (int i = 0; i < 4; i++) // Hard coded to max 4 players
                {
                    if (GameManager.Instance.ActivePlayerControllers.ContainsKey(i))
                    {
                        GameManager.Instance.ActivePlayerControllers[i].Death();
                    }
                }
            }
#endregion
        }
    }
}