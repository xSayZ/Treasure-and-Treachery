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
using UnityEngine;


namespace Game {
    namespace Quest {
        public class QuestObjective : MonoBehaviour, IInteractable
        {
            [Header("Quest Settings")]
            [SerializeField] private bool requiredQuest;
            [SerializeField] private List<Pickup> requiredPickups;
            
            private class QuestStatus
            {
                public bool IsInteracting;
                public float CurrentInteractTime;
            }
            
            private Dictionary<Item, QuestStatus> requiredItems;
            
#region Unity Functions
            private void Start()
            {
                if (requiredQuest)
                {
                    QuestManager.RegisterRequiredQuest(this);
                }

                requiredItems = new Dictionary<Item, QuestStatus>();
                for (int i = 0; i < requiredPickups.Count; i++)
                {
                    requiredItems.Add(requiredPickups[i].GetItem(), new QuestStatus());
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
                        
                        if (item.Value.CurrentInteractTime >= item.Key.InteractionTime)
                        {
                            _itemsToRemove.Add(item.Key);
                        }
                    }
                }
                
                for (int i = 0; i < _itemsToRemove.Count; i++)
                {
                    requiredItems.Remove(_itemsToRemove[i]);
                }
                
                if (requiredItems.Count <= 0)
                {
                    QuestCompleted();
                }
            }
#endregion

#region Public Functions
            public void Interact(int _playerIndex, bool _start) // Check if start or end
            {
                PlayerData _playerData = GameManager.Instance.activePlayerControllers[_playerIndex].GetComponent<PlayerController>().PlayerData;
                if (requiredItems.ContainsKey(_playerData.currentItem))
                {
                    requiredItems[_playerData.currentItem].IsInteracting = _start;
                }
            }
#endregion

#region Private Functions
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
