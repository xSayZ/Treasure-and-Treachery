// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-14
// Author: b22feldy
// Description: Treasure and Treachery
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace ScriptableObjects {
        public class EventModifiersSO : MonoBehaviour {
            // TODO: Remove SerializeField after testing
            [SerializeField] private int healthModifier;
            [SerializeField] private int goldModifier;
            [SerializeField] private int personalObjectiveModifier;
            [SerializeField] private int dashModifier;
            [SerializeField]private float damageModifier;
            
            public int HealthModifier {
                get => healthModifier;
                set => healthModifier = value;
            }
            
            public int GoldModifier {
                get => goldModifier;
                set => goldModifier = value;
            }
            
            public int PersonalObjectiveModifier {
                get => personalObjectiveModifier;
                set => personalObjectiveModifier = value;
            }
            
            public int DashModifier {
                get => dashModifier;
                set => dashModifier = value;
            }
            
            public float DamageModifier {
                get => damageModifier;
                set => damageModifier = value;
            }
            
            public void ResetModifiers() {
                healthModifier = 0;
                goldModifier = 0;
                personalObjectiveModifier = 0;
                dashModifier = 0;
                damageModifier = 0;
            }
        }
    }
}
