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
            [SerializeField] public bool hasStartingMeleeWeapon;
            [SerializeField] public bool hasStartingRangedWeapon;
            [SerializeField] public int personalObjectiveMultiplier;
            
            [Header("Debug")]
            [SerializeField] public int playerIndex;
            [SerializeField] public int currentHealth;
            [SerializeField] public Item currentItem;
            [SerializeField] public bool canPickUp;
            
            [SerializeField] public int points;
            
            [SerializeField] public int personalObjective;
            [SerializeField] public int personalObjectiveThisLevel;
            [SerializeField] public int currency;
            [SerializeField] public int currencyThisLevel;
            [SerializeField] public int kills;
            [SerializeField] public int killsThisLevel;
            
            [SerializeField] public bool hasMeleeWeapon;
            [SerializeField] public bool hasRangedWeapon;
            
            public int ControllerID;

            public void NewScene()
            {
                currentHealth = startingHealth;
                personalObjectiveThisLevel = 0;
                currencyThisLevel = 0;
                killsThisLevel = 0;
                currentItem = null;
                canPickUp = true;
            }
            
            private void OnEnable()
            {
                currentHealth = startingHealth;
                currentItem = null;
                canPickUp = true;
                
                points = 0;
                
                personalObjective = 0;
                personalObjectiveThisLevel = 0;
                currency = 0;
                currencyThisLevel = 0;
                kills = 0;
                killsThisLevel = 0;
                
                hasMeleeWeapon = hasStartingMeleeWeapon;
                hasRangedWeapon = hasStartingRangedWeapon;
            }
        }
    }
}
