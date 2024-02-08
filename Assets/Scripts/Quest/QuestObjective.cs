// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-02
// Author: alexa
// Description: Objective of a quest
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Backend;
using Game.Core;
using Game.Player;
using Game.Scene;
using Game.UI;
using UnityEngine;


namespace Game {
    namespace Quest {
        public class QuestObjective : MonoBehaviour, IInteractable
        {
            [Header("Setup")]
            [SerializeField] private GameObject interactionUI;
            [SerializeField] private Transform progressBarCanvas;
            [SerializeField] private GameObject progressBarPrefab;
            
            [Header("Quest Settings")]
            [SerializeField] private bool requiredQuest;
            [SerializeField] private List<Pickup> requiredPickups;
            
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
            
#region Unity Functions
            private void OnEnable()
            {
                QuestManager.OnItemPickedUp.AddListener(ItemPickedUp);
                QuestManager.OnItemDropped.AddListener(ItemDropped);
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
                if (requiredQuest)
                {
                    QuestManager.RegisterRequiredQuest(this);
                }

                requiredItems = new Dictionary<Item, QuestStatus>();
                for (int i = 0; i < requiredPickups.Count; i++)
                {
                    GameObject _progressBar = Instantiate(progressBarPrefab, progressBarCanvas);
                    _progressBar.SetActive(false);
                    requiredItems.Add(requiredPickups[i].GetItem(), new QuestStatus(_progressBar.GetComponent<ProgressBar>()));
                }
            }
            
            private void Update()
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
                            
                            GameManager.Instance.activePlayerControllers[item.Value.PlayerIndex].GetComponent<PlayerMovementBehaviour>().SetMovementActiveState(true);
                            CanInteractWith[item.Value.PlayerIndex] = false;
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
#endregion

#region Public Functions
            public void Interact(int _playerIndex, bool _start)
            {
                PlayerData _playerData = GameManager.Instance.activePlayerControllers[_playerIndex].GetComponent<PlayerController>().PlayerData;
                
                if (_playerData.currentItem == null)
                {
                    return;
                }
                
                if (requiredItems.ContainsKey(_playerData.currentItem))
                {
                    requiredItems[_playerData.currentItem].IsInteracting = _start;
                    requiredItems[_playerData.currentItem].PlayerIndex = _playerIndex;
                    
                    requiredItems[_playerData.currentItem].ProgressBar.gameObject.SetActive(true);
                    GameManager.Instance.activePlayerControllers[_playerIndex].GetComponent<PlayerMovementBehaviour>().SetMovementActiveState(!_start);
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
            
            private void QuestCompleted()
            {
                if (requiredQuest)
                {
                    QuestManager.OnRequiredQuestCompleted(this);
                }
                
                Destroy(gameObject);
            }
#endregion
        }
    }
}