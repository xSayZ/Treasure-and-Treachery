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
                Kill,
                Time
            }
            
            [Header("Setup")]
            [SerializeField] private GameObject interactionUI;
            [SerializeField] private Transform progressBarCanvas;
            [SerializeField] private GameObject progressBarPrefab;
            
            [Header("Quest Settings")]
            [SerializeField] private bool requiredQuest;
            [SerializeField] private bool setRequiredPickupsDynamically;
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
            
            // Kill variables
            [HideInInspector] public int WaitTime;
            [HideInInspector] public string CounterTextBeforeNumber;
            [HideInInspector] public string CounterTextAfterNumber;
            
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
            private float currentWaitTime;
            private List<Item>[] heldQuestItems;

#region Unity Functions
            private void OnEnable()
            {
                QuestManager.OnItemPickedUp.AddListener(ItemPickedUp);
                QuestManager.OnItemDropped.AddListener(ItemDropped);
                Enemy.Systems.EnemyManager.OnEnemyDeath.AddListener(EnemyKilled);
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
                heldQuestItems = new List<Item>[4]; // Hard coded to max 4 players
                for (int i = 0; i < 4; i++) // Hard coded to max 4 players
                {
                    heldQuestItems[i] = new List<Item>();
                }
                
            }

            private void Start()
            {
                requiredItems = new Dictionary<Item, QuestStatus>();
                
                if (setRequiredPickupsDynamically) {
                    // Remove from RequiredPickups if players are not on
                    int playerCount = GameManager.Instance.ActivePlayerControllers.Count;
                    for (int i = 0; i < RequiredPickups.Count; i++) {
                        if (i <= playerCount - 1)
                            continue;
                        
                        RequiredPickups.RemoveAt(i);
                        i--;
                    }
                }
                
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

                                GameManager.Instance.ActivePlayerControllers[item.Value.PlayerIndex].gameObject.GetComponent<PlayerMovementBehaviour>().QuestMoveRotateLock = false;
                                CanInteractWith[item.Value.PlayerIndex] = false;
                                
                                try  
                                {
                                    playerAudio.InteractionAudio(_eventInstance, gameObject, 2, false);
                                } 
                                catch (Exception e)
                                {
                                    Debug.LogWarning("[{QuestObjective}]: Error Exception " + e);
                                }
                            }
                        }
                    }
                    
                    RemoveRequiredItemFromList(_itemsToRemove);
                    
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
                else if (QuestType == QuestTypes.Time)
                {
                    currentWaitTime += Time.deltaTime;
                    
                    if (currentWaitTime >= WaitTime)
                    {
                        QuestCompleted();
                    }
                }
            }
            private void RemoveRequiredItemFromList(List<Item> _itemsToRemove) {

                for (int i = 0; i < _itemsToRemove.Count; i++)
                {
                    QuestManager.OnItemDropped.Invoke(requiredItems[_itemsToRemove[i]].PlayerIndex, _itemsToRemove[i], true);
                    requiredItems.Remove(_itemsToRemove[i]);
                }
            }
#endregion

#region Public Functions
            public void Interact(int _playerIndex, bool _start)
            {
                PlayerData _playerData = GameManager.Instance.ActivePlayerControllers[_playerIndex].PlayerData;
                
                if (_playerData.currentItem == null)
                {
                    return;
                }
                
                if (requiredItems.ContainsKey(_playerData.currentItem))
                {
                    requiredItems[_playerData.currentItem].IsInteracting = _start;
                    requiredItems[_playerData.currentItem].PlayerIndex = _playerIndex;
                    
                    requiredItems[_playerData.currentItem].ProgressBar.gameObject.SetActive(true);

                    GameManager.Instance.ActivePlayerControllers[_playerIndex].gameObject.GetComponent<PlayerMovementBehaviour>().QuestMoveRotateLock = _start;
                    
                    if (_start == true)
                    {
                        try
                        {
                            _eventInstance = playerAudio.InteractionAudio(_eventInstance, gameObject, 0, true);
                        }
                        catch (Exception e)
                        {
                            Debug.LogWarning("[{QuestObjective}]: Error Exception " + e);
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
                            Debug.LogWarning("[{QuestObjective}]: Error Exception " + e);
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
                    heldQuestItems[_playerIndex].Add(_item);
                    
                    if (heldQuestItems[_playerIndex].Count > 0)
                    {
                        CanInteractWith[_playerIndex] = true;
                    }
                }
            }

            private void ItemDropped(int _playerIndex, Item _item, bool _destroy)
            {
                if (heldQuestItems[_playerIndex].Contains(_item))
                {
                    heldQuestItems[_playerIndex].Remove(_item);
                    
                    if (heldQuestItems[_playerIndex].Count <= 0)
                    {
                        CanInteractWith[_playerIndex] = false;
                    }
                }
                
                // Stop progress if player is interacting
                if (requiredItems.ContainsKey(_item))
                {
                    requiredItems[_item].IsInteracting = false;
                    
                    if (GameManager.Instance.ActivePlayerControllers.ContainsKey(requiredItems[_item].PlayerIndex))
                    {
                        GameManager.Instance.ActivePlayerControllers[requiredItems[_item].PlayerIndex].gameObject.GetComponent<PlayerMovementBehaviour>().QuestMoveRotateLock = false;
                    }
                }
            }

            private void EnemyKilled(EnemyController _enemyController)
            {
                killsSoFar++;
                
                if (killsSoFar < RequiredKills)
                {
                    QuestManager.OnKillQuestProgress.Invoke(CounterTextBeforeNumber, RequiredKills - killsSoFar, CounterTextAfterNumber);
                }
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