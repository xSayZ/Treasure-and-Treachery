// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-26
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using Game.Core;
using UnityEngine;


namespace Game {
    namespace Backend {


        [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Player Data")]
        public class PlayerData : ScriptableObject
        {
            [Header("Settings")]
            [SerializeField] public int startingHealth;
            
            [Header("Debug")]
            [SerializeField] public int currentHealth;
            [SerializeField] public int playerIndex;
            [SerializeField] public int currency;
            [SerializeField] public Item currentItem;
            [SerializeField] public bool canPickUp;
            
            public int SetPlayerData(int _currentHealth)
            {
                currentHealth = _currentHealth;
                return playerIndex;
            }
            
            private void OnEnable()
            {
                currentHealth = startingHealth;
                currency = 0;
                currentItem = null;
                canPickUp = true;
            }
        }
    }
}
