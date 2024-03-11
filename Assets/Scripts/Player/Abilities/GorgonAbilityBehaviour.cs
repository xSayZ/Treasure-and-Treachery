// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-23
// Author: b22alesj
// Description: The gorgons ability
// --------------------------------
// ------------------------------*/

using Game.Quest;
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
                
                playerController.PlayerOverheadUIBehaviour.UpdatePersonalObjective(playerController.PlayerData.personalObjective, 0);
                
                playerController.PlayerAttackBehaviour.OnKill.AddListener(EnemyKilled);
            }

            protected override void OnDisable()
            {
                base.OnDisable();
                
                if (playerController)
                {
                    playerController.PlayerAttackBehaviour.OnKill.RemoveListener(EnemyKilled);
                }
            }

            protected override void OnDashKill(bool _stunned)
            {
                EnemyKilled(_stunned);
                
                if (_stunned)
                {
                    playerController.PlayerMovementBehaviour.UpdateCurrentNumberOfDashes(1);
                }
            }

            private void EnemyKilled(bool _stunned)
            {
                if (_stunned)
                {
                    playerController.Heal(healthOnStunnedDashKill);
                    
                    playerController.PlayerData.personalObjective += 1;
                    playerController.PlayerData.personalObjectiveThisLevel += 1;
                    playerController.PlayerOverheadUIBehaviour.UpdatePersonalObjective(playerController.PlayerData.personalObjective, 1);
                    QuestManager.PersonalObjectiveScoreUpdated(playerController.PlayerIndex, playerController.PlayerData.personalObjective);
                }
            }
        }
    }
}