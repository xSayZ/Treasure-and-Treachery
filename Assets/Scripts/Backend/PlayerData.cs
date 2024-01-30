// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-26
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Events;
using UnityEngine;
using UnityEngine.Events;


namespace Game {
    namespace Backend {


        [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Player Data")]
        public class PlayerData : ScriptableObject
        {
            [SerializeField] private List<int> playerHealth;
            [SerializeField] public List<int> playerIndex ;
            [SerializeField] public List<int> currency;
            [SerializeField] private int questValue;

            [SerializeField]private int currentHealth = 10;
            private int currentCurrency;
            private int currentQuestValue;


            public int CurrentHealth
            {
                get => currentHealth;
                set
                {
                    
                    for (int i = 0; i < playerIndex.Count; i++)
                    {
                        int healthChange = value - currentHealth;
                        currentHealth = Mathf.Clamp(value, 0, playerHealth[i]);
                        EventManager.OnHealthChange?.Invoke(healthChange,playerIndex[i]);
                    }
                   
                }
            }
        }
    }
}
