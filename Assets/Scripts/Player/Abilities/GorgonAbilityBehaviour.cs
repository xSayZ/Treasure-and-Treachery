// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-23
// Author: b22alesj
// Description: The gorgons ability
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Player {
        public class GorgonAbilityBehaviour : PlayerAbilityBehaviour
        {
            [Header("Settings")]
            [SerializeField] private int healthOnStunnedDashKill;
            
            protected override void Setup()
            {
                playerController.PlayerAttackBehaviour.MeleeIsStunAttack = true;
            }

            protected override void OnDashKill(bool _stunned)
            {
                if (_stunned)
                {
                    playerController.Heal(healthOnStunnedDashKill);
                }
            }
        }
    }
}