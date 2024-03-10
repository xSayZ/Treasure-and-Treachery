// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-09
// Author: b22alesj
// Description: Carriage data
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Backend {
        [CreateAssetMenu(fileName = "CarriageData", menuName = "ScriptableObjects/Carriage Data")]
        public class CarriageData : ScriptableObject
        {
            [Header("Settings")]
            [SerializeField] public int startingHealth;
            
            [Header("Debug")]
            [SerializeField] public int currentHealth;
            
            private void OnEnable()
            {
                currentHealth = startingHealth;
            }
        }
    }
}
