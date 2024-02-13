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
            [SerializeField] public Color playerMaterialColor;
            
            [Header("Debug")]
            [SerializeField] public int playerIndex;
            [SerializeField] public int currentHealth;
            [SerializeField] public Item currentItem;
            [SerializeField] public bool canPickUp;
            
            [SerializeField] public int currency;
            [SerializeField] public int currencyThisLevel;
            [SerializeField] public int kills;
            [SerializeField] public int killsThisLevel;
            
            public void NewScene()
            {
                currencyThisLevel = 0;
                killsThisLevel = 0;
            }
            
            private void OnEnable()
            {
                currentHealth = startingHealth;
                currentItem = null;
                canPickUp = true;
                
                currency = 0;
                currencyThisLevel = 0;
                kills = 0;
                killsThisLevel = 0;
            }
        }
    }
}
