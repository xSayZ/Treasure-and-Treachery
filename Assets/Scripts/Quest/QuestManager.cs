// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-02
// Author: alexa
// Description: Quest manager
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Core;
using UnityEngine;
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
            
            public static UnityEvent<string, int, string> OnKillQuestProgress = new UnityEvent<string, int, string>(); // Player Index
            
            public static UnityEvent OnRequiredQuestRegistered = new UnityEvent();
            public static UnityEvent OnAllRequiredQuestsCompleted = new UnityEvent();
            
            public static int IndexOfLeadingPlayer { get; private set; }
            private static int _scoreOfLeadingPlayer;
            
            private static List<QuestObjective> _requiredQuestObjectivesLeft = new List<QuestObjective>();

            public static void SetUp()
            {
                Reset();
                
                // Get all required quests
                QuestObjective[] _questObjectives = Object.FindObjectsByType<QuestObjective>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                
                foreach (var objective in _questObjectives) {
                    if (objective.GetRequiredStatus()) {
                        _requiredQuestObjectivesLeft.Add(objective);
                    }
                }
                
                if (_requiredQuestObjectivesLeft.Count > 0)
                {
                    OnRequiredQuestRegistered.Invoke();
                }
            }

            public static void OnRequiredQuestCompleted(QuestObjective _questObjective)
            {
                _requiredQuestObjectivesLeft.Remove(_questObjective);
                if (_requiredQuestObjectivesLeft.Count <= 0)
                {
                    OnAllRequiredQuestsCompleted.Invoke();
                }
            }

            public static void PersonalObjectiveScoreUpdated(int _playerIndex, int _score)
            {
                if (_score > _scoreOfLeadingPlayer)
                {
                    IndexOfLeadingPlayer = _playerIndex;
                    _scoreOfLeadingPlayer = _score;
                }
            }

            private static void Reset()
            {
                _requiredQuestObjectivesLeft = new List<QuestObjective>();
            }
        }
    }
}