// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-02
// Author: alexa
// Description: Objective of a quest
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Core;
using UnityEngine;


namespace Game {
    namespace Quest {
        public class QuestObjective : MonoBehaviour, IInteractable
        {
            [Header("Quest Settings")]
            [SerializeField] private bool requiredQuest;
            [SerializeField] private List<Item> requiredItems;
            
#region Unity Functions
            private void OnEnable()
            {
                QuestManager.OnItemPickedUp.AddListener(QuestItemPickedUp);
            }
            
            private void OnDisable()
            {
                QuestManager.OnItemPickedUp.RemoveListener(QuestItemPickedUp);
            }
            
            private void Start()
            {
                if (requiredQuest)
                {
                    QuestManager.RegisterRequiredQuest(this);
                }
            }
#endregion

#region Private Functions
            public void Interact(int _playerIndex)
            {
                // Player interacted with quest objective
            }
#endregion

#region Private Functions
            private void QuestItemPickedUp(int _playerIndex, Item _item)
            {
                if (requiredItems.Contains(_item))
                {
                    requiredItems.Remove(_item); // Temporary, remove it form list when player enters objective zone instead
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
