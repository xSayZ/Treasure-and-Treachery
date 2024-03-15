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
        [CreateAssetMenu(fileName = "EventModifiers", menuName = "ScriptableObjects/Event Modifiers")]
        public class EventModifiersSO : ScriptableObject {
            // TODO: Remove SerializeField after testing
            [SerializeField] private int healthModifier;
            [SerializeField] private int goldModifier;
            [SerializeField] private int personalObjective;
            [SerializeField] private int personalObjectiveModifier;
            public int HealthModifier {
                get => healthModifier;
                set => healthModifier = value;
            }
            
            public int GoldModifier {
                get => goldModifier;
                set => goldModifier = value;
            }
            
            public int PersonObjective {
                get => personalObjective;
                set => personalObjective = value;
            }
            
            public int PersonalObjectiveModifier {
                get => personalObjectiveModifier;
                set => personalObjectiveModifier = value;
            }
            
            public void ResetModifiers() {
                healthModifier = 0;
                goldModifier = 0;
                personalObjectiveModifier = 0;
            }
        }
    }
}
