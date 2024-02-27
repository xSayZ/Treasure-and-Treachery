// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-23
// Author: b22alesj
// Description: The witch's ability
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Player {
        public class WitchAbilityBehaviour : PlayerAbilityBehaviour
        {
            [Header("Settings")]
            [SerializeField] private int healPerNormalKill;
            
            protected override void Setup()
            {
                playerController.PlayerAttackBehaviour.OnKill.AddListener(EnemyKilled);
                playerController.PlayerAttackBehaviour.OnWaveKill.AddListener(WaveKill);
            }
            
            protected override void OnDisable()
            {
                base.OnDisable();
                
                if (playerController)
                {
                    playerController.PlayerAttackBehaviour.OnKill.RemoveListener(EnemyKilled);
                    playerController.PlayerAttackBehaviour.OnWaveKill.RemoveListener(WaveKill);
                }
            }
            
            // protected override void OnDashKill(bool _stunned) {}
            
            private void EnemyKilled()
            {
                playerController.Heal(healPerNormalKill);
            }
            
            private void WaveKill()
            {
                // Give extra souls here
            }
        }
    }
}