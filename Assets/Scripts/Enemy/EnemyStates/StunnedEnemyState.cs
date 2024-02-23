// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-23
// Author: b22alesj
// Description: Enemy stunned state
// --------------------------------
// ------------------------------*/

using UnityEngine;



namespace Game {
    namespace Enemy {
        [System.Serializable]
        public class StunnedEnemyState : EnemyState
        {
            [SerializeField] private Animator animator;
            [SerializeField] private SkinnedMeshRenderer meshRenderer;
            [SerializeField] private Material stunnedMaterial;

#region State Machine Functions
            protected override void SetUp()
            {
                Name = "Stunned";
            }

            public override void Enter()
            {
                animator.speed = 0;
                meshRenderer.material = stunnedMaterial;
                enemyController.GetEnemyAttackBehaviour().SetCanAttack(false);
            }

            //public override void FixedUpdate(){}

            //public override void Exit(){}
#endregion
        }
    }
}