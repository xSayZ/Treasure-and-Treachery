// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-26
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Game {
    namespace Backend {


        [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Player Data")]
        public class PlayerData : ScriptableObject
        {
            [field:SerializeField] public int playerHealth { get; private set; }
            [SerializeField] public int playerIndex ;
            [SerializeField] public int currency;
            [SerializeField] private int questValue;

            private int currentHealth;
            private int currentCurrency;
            private int currentQuestValue;
            
        }
    }
}
