// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-02
// Author: alexa
// Description: Objective of a quest
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Scene;
using UnityEngine;


namespace Game {
    namespace Quest {
        public class QuestObjective : MonoBehaviour
        {
            [Header("Quest Settings")]
            [SerializeField] private bool requiredQuest;
            [SerializeField] private List<Pickup> requiredPickups;
            
#region Unity Functions
            private void OnEnable()
            {
                QuestManager.OnQuestItemPickedUp.AddListener(QuestItemPickedUp);
            }
            
            private void OnDisable()
            {
                QuestManager.OnQuestItemPickedUp.RemoveListener(QuestItemPickedUp);
            }
            
            private void Start()
            {
                if (requiredQuest)
                {
                    QuestManager.RegisterRequiredQuest(this);
                }
            }
            
            private void OnTriggerEnter(Collider other)
            {
                if (other.CompareTag("Player"))
                {
                    // Quest is completed immediately at the moment, count down timer instead
                    if (requiredPickups.Count <= 0) // Dose not account for what player has item
                    {
                       QuestCompleted(); 
                    }
                }
            }
            
            private void OnTriggerExit(Collider other)
            {
                if (other.CompareTag("Player"))
                {
                    // Dose nothing at the moment, will stop timer
                }
            }
#endregion

#region Private Functions
            private void QuestItemPickedUp(int _playerIndex, int _weight, Pickup _pickup)
            {
                if (requiredPickups.Contains(_pickup))
                {
                    requiredPickups.Remove(_pickup); // Temporary, remove it form list when player enters objective zone instead
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
