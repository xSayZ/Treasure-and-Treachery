// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-02
// Author: alexa
// Description: Quest manager
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Core;
using Game.Scene;
using UnityEngine.Events;


namespace Game {
    namespace Quest {
        public static class QuestManager
        {
            public static UnityEvent<int, Item> OnItemPickedUp = new UnityEvent<int, Item>(); // Player Index, Item, Quest Objective
            public static UnityEvent<int, int> OnGoldPickedUp = new UnityEvent<int, int>(); // Player Index, Amount
            
            public static UnityEvent OnAllRequiredQuestsCompleted = new UnityEvent();
            
            private static List<QuestObjective> requiredQuestObjectivesLeft = new List<QuestObjective>();
            
            public static void RegisterRequiredQuest(QuestObjective _questObjective)
            {
                requiredQuestObjectivesLeft.Add(_questObjective);
            }
            
            public static void OnRequiredQuestCompleted(QuestObjective _questObjective)
            {
                requiredQuestObjectivesLeft.Remove(_questObjective);
                if (requiredQuestObjectivesLeft.Count <= 0)
                {
                    OnAllRequiredQuestsCompleted.Invoke();
                }
            }
            
            public static void Reset()
            {
                requiredQuestObjectivesLeft = new List<QuestObjective>();
            }
        }
    }
}
