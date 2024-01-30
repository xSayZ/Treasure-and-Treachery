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

            private int currentHealth;
            private int currentCurrency;
            private int currentQuestValue;
            
            
        }
    }
}
