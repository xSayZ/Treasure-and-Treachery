// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Enemy chase state
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Enemy {
        [System.Serializable]
        public class ChaseEnemyState : EnemyState
        {
            [SerializeField] private float moveSpeed;
            [SerializeField] private float minMoveDistance;
            
            private Vector3 lastTargetPosition;
            private Vector3 positionLastUpdate;
            
#region State Machine Functions
            protected override void SetUp()
            {
                Name = "Chase";
            }

            public override void Enter()
            {
                enemyController.NavMeshAgent.speed = moveSpeed;
                lastTargetPosition = GetClosestTarget(enemyController.targetsInVisionRange).position;
                enemyController.NavMeshAgent.destination = lastTargetPosition;
            }

            public override void FixedUpdate()
            {
                // Update last seen position
                if (enemyController.targetsInVisionRange.Count > 0)
                {
                    lastTargetPosition = GetClosestTarget(enemyController.targetsInVisionRange).position;
                }
                else if (enemyController.targetsInHearingRange.Count > 0)
                {
                    lastTargetPosition = GetClosestTarget(enemyController.targetsInHearingRange).position;
                }
                
                // No longer chasing
                if (IsStuck(positionLastUpdate, enemyController.transform.position, minMoveDistance))
                {
                    enemyController.ChangeState(enemyController.AlertEnemyState);
                }
                
                positionLastUpdate = enemyController.transform.position;
                
                NavmeshUpdateCheck(lastTargetPosition);
            }
            
            //public override void Exit(){}
 #endregion
        }
    }
}