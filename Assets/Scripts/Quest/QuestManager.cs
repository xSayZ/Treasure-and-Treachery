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
            
            private static List<QuestObjective> _requiredQuestObjectivesLeft = new List<QuestObjective>();
            
            private static int _indexOfLeadingPlayer;
            private static int _scoreOfLeadingPlayer;

            public static void SetUp()
            {
                QuestObjective[] _questObjectives = Object.FindObjectsByType<QuestObjective>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                
                for (int i = 0; i < _questObjectives.Length; i++)
                {
                    if (_questObjectives[i].GetRequiredStatus())
                    {
                        _requiredQuestObjectivesLeft.Add(_questObjectives[i]);
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
                    _indexOfLeadingPlayer = _playerIndex;
                    _scoreOfLeadingPlayer = _score;
                }
            }
 
            public static void Reset() // Not used yet and probably not needed either
            {
                _requiredQuestObjectivesLeft = new List<QuestObjective>();
            }
        }
    }
}