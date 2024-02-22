// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-02
// Author: alexa
// Description: Objective of a quest
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using FMOD.Studio;
using Game.Audio;
using Game.Backend;
using Game.Core;
using Game.Enemy;
using Game.Player;
using Game.Scene;
using Game.UI;
using UnityEngine;
using UnityEngine.Events;


namespace Game {
    namespace Quest {
        public class QuestObjective : MonoBehaviour, IInteractable
        {
            public enum QuestTypes
            {
                Fetch,
                Kill
            }
            
            [Header("Setup")]
            [SerializeField] private GameObject interactionUI;
            [SerializeField] private Transform progressBarCanvas;
            [SerializeField] private GameObject progressBarPrefab;
            
            [Header("Quest Settings")]
            [SerializeField] private bool requiredQuest;
            public QuestTypes QuestType;
            
            [Header("Quest Events")]
            [SerializeField] private UnityEvent questCompleted = new UnityEvent();
            
            [Header("Audio")] 
            [SerializeField] 
            private PlayerAudio playerAudio;
            private EventInstance _eventInstance;
            
            // Fetch variables
            [HideInInspector] public List<Pickup> RequiredPickups;
            
            // Kill variables
            [HideInInspector] public int RequiredKills;
            
            // Interaction variables
            [HideInInspector] public bool[] CanInteractWith { get; set; }
            [HideInInspector] public bool[] PlayersThatWantsToInteract { get; set; }
            [HideInInspector] public Transform InteractionTransform { get; set; }
            
            private class QuestStatus
            {
                public bool IsInteracting;
                public float CurrentInteractTime;
                public int PlayerIndex;
                public ProgressBar ProgressBar;
                
                public QuestStatus(ProgressBar _progressBar)
                {
                    ProgressBar = _progressBar;
                }
            }
            
            private Dictionary<Item, QuestStatus> requiredItems;
            private int killsSoFar;

#region Unity Functions
            private void OnEnable()
            {
                QuestManager.OnItemPickedUp.AddListener(ItemPickedUp);
                QuestManager.OnItemDropped.AddListener(ItemDropped);
                EnemyManager.OnEnemyDeath.AddListener(EnemyKilled);
            }

            private void OnDisable()
            {
                QuestManager.OnItemPickedUp.RemoveListener(ItemPickedUp);
                QuestManager.OnItemDropped.RemoveListener(ItemDropped);
            }

            private void Awake()
            {
                CanInteractWith = new bool[4]; // Hard coded to max 4 players
                PlayersThatWantsToInteract = new bool[4]; // Hard coded to max 4 players
                InteractionTransform = transform;
            }

            private void Start()
            {
                requiredItems = new Dictionary<Item, QuestStatus>();
                
                if (QuestType == QuestTypes.Fetch)
                {
                    for (int i = 0; i < RequiredPickups.Count; i++)
                    {
                        GameObject _progressBar = Instantiate(progressBarPrefab, progressBarCanvas);
                        _progressBar.SetActive(false);
                        requiredItems.Add(RequiredPickups[i].GetItem(), new QuestStatus(_progressBar.GetComponent<ProgressBar>()));
                    }
                }
            }

            private void Update()
            {
                if (QuestType == QuestTypes.Fetch)
                {
                    List<Item> _itemsToRemove = new List<Item>();
                    
                    foreach(KeyValuePair<Item, QuestStatus> item in requiredItems)
                    {
                        if (item.Value.IsInteracting)
                        {
                            item.Value.CurrentInteractTime += Time.deltaTime;
                            
                            float _currentProgress = item.Value.CurrentInteractTime / item.Key.InteractionTime;
                            item.Value.ProgressBar.SetProgress(_currentProgress);
                            
                            if (item.Value.CurrentInteractTime >= item.Key.InteractionTime)
                            {
                                item.Value.ProgressBar.gameObject.SetActive(false);
                                _itemsToRemove.Add(item.Key);
                                
                                GameManager.Instance.activePlayerControllers[item.Value.PlayerIndex].gameObject.GetComponent<PlayerMovementBehaviour>().SetMovementActiveState(true, true);
                                CanInteractWith[item.Value.PlayerIndex] = false;
                                
                                try  
                                {
                                    playerAudio.InteractionAudio(_eventInstance, gameObject, 2, false);
                                } 
                                catch (Exception e)
                                {
                                    Debug.LogError("[{QuestObjective}]: Error Exception " + e);
                                }
                            }
                        }
                    }
                    
                    for (int i = 0; i < _itemsToRemove.Count; i++)
                    {
                        QuestManager.OnItemDropped.Invoke(requiredItems[_itemsToRemove[i]].PlayerIndex, _itemsToRemove[i], true);
                        requiredItems.Remove(_itemsToRemove[i]);
                    }
                    
                    if (requiredItems.Count <= 0)
                    {
                        QuestCompleted();
                    }
                }
                else if (QuestType == QuestTypes.Kill)
                {
                    if (killsSoFar >= RequiredKills)
                    {
                        QuestCompleted();
                    }
                }
            }
#endregion

#region Public Functions
            public void Interact(int _playerIndex, bool _start)
            {
                PlayerData _playerData = GameManager.Instance.activePlayerControllers[_playerIndex].PlayerData;
                
                if (_playerData.currentItem == null)
                {
                    return;
                }
                
                if (requiredItems.ContainsKey(_playerData.currentItem))
                {
                    requiredItems[_playerData.currentItem].IsInteracting = _start;
                    requiredItems[_playerData.currentItem].PlayerIndex = _playerIndex;
                    
                    requiredItems[_playerData.currentItem].ProgressBar.gameObject.SetActive(true);
                    
                    GameManager.Instance.activePlayerControllers[_playerIndex].gameObject.GetComponent<PlayerMovementBehaviour>().SetMovementActiveState(!_start, !_start);
                    
                    if (_start == true)
                    {
                        try
                        {
                            _eventInstance = playerAudio.InteractionAudio(_eventInstance, gameObject, 0, true);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("[{QuestObjective}]: Error Exception " + e);
                        }
                    }
                    else
                    {
                        try
                        {
                            _eventInstance = playerAudio.InteractionAudio(_eventInstance, gameObject, 1, false);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("[{QuestObjective}]: Error Exception " + e);
                        }
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

            public bool GetRequiredStatus()
            {
                return requiredQuest;
            }
#endregion

#region Private Functions
            private void ItemPickedUp(int _playerIndex, Item _item)
            {
                if (requiredItems.ContainsKey(_item))
                {
                    CanInteractWith[_playerIndex] = true;
                }
            }

            private void ItemDropped(int _playerIndex, Item _item, bool _destroy)
            {
                if (requiredItems.ContainsKey(_item))
                {
                    CanInteractWith[_playerIndex] = false;
                }
            }

            private void EnemyKilled(EnemyController _enemyController)
            {
                killsSoFar++;
            }

            private void QuestCompleted()
            {
                if (requiredQuest)
                {
                    QuestManager.OnRequiredQuestCompleted(this);
                }
                
                questCompleted.Invoke();
                
                Destroy(gameObject);
            }
#endregion
        }
    }
}