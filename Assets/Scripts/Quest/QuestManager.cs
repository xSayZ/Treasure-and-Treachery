// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-02
// Author: alexa
// Description: Quest manager
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Core;
using UnityEngine.Events;


namespace Game {
    namespace Quest {
        public static class QuestManager
        {
            public static UnityEvent<int, Item> OnItemPickedUp = new UnityEvent<int, Item>(); // Player Index, Item
            public static UnityEvent<int, Item, bool> OnItemDropped = new UnityEvent<int, Item, bool>(); // Player Index, Item, Destroy
            public static UnityEvent<int, int> OnGoldPickedUp = new UnityEvent<int, int>(); // Player Index, Amount
            public static UnityEvent<int> OnMeleeWeaponPickedUp = new UnityEvent<int>(); // Player Index
            public static UnityEvent<int> OnRagedWeaponPickedUp = new UnityEvent<int>(); // Player Index
            
            public static UnityEvent OnRequiredQuestRegistered = new UnityEvent();
            public static UnityEvent OnAllRequiredQuestsCompleted = new UnityEvent();
            
            private static List<QuestObjective> requiredQuestObjectivesLeft = new List<QuestObjective>();
            
            public static void RegisterRequiredQuest(QuestObjective _questObjective)
            {
                requiredQuestObjectivesLeft.Add(_questObjective);
                OnRequiredQuestRegistered.Invoke();
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
