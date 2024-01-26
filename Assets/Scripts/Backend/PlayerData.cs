// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-26
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Backend {

        [CreateAssetMenu(fileName = "Player Data", menuName = "ScriptableObjects/Player Data")]
        public class PlayerData : ScriptableObject
        {
            [SerializeField] private int playerHealth;
            [SerializeField] private int currency;
            [SerializeField] private int questValue;
        }
    }
}
