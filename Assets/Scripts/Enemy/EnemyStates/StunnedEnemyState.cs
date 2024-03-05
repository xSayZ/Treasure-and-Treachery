// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-23
// Author: b22alesj
// Description: Enemy stunned state
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;


namespace Game {
    namespace Enemy {
        [System.Serializable]
        public class StunnedEnemyState : EnemyState
        {
            [SerializeField] private float stunTime;
            [SerializeField] private Animator animator;
            [SerializeField] private SkinnedMeshRenderer meshRenderer;
            [SerializeField] private Material stunnedMaterial;
            
            private float currentStunTime;
            private Material previousMaterial;

#region State Machine Functions
            protected override void SetUp()
            {
                Name = "Stunned";
            }

            public override void Enter()
            {
                currentStunTime = 0;
                animator.speed = 0;
                previousMaterial = meshRenderer.material;
                meshRenderer.material = stunnedMaterial;
                enemyController.GetEnemyAttackBehaviour().SetCanAttack(false);
                
                try
                {
                    enemyController.playerAudio.PetrifyAudio(enemyController.gameObject);
                } 
                catch (Exception e)
                {
                    Debug.LogError("[{StunnedEnemyState}]: Error Exception " + e);
                }
            }

            public override void FixedUpdate()
            {
                currentStunTime += Time.fixedDeltaTime;
                
                if (currentStunTime >= stunTime)
                {
                    animator.speed = 1;
                    meshRenderer.material = previousMaterial;
                    enemyController.GetEnemyAttackBehaviour().SetCanAttack(true);
                    
                    enemyController.ChangeState(enemyController.AlertEnemyState);
                }
            }

            //public override void Exit(){}
#endregion
        }
    }
}