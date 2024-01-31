// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Enemy chase state
// --------------------------------
// ------------------------------*/

using UnityEngine;
using UnityEngine.AI;


namespace Game {
    namespace Enemy {
        [System.Serializable]
        public class ChaseEnemyState : EnemyState
        {
            [SerializeField] private float maxDeviationAngle;
            [SerializeField] private float updateDistance;
            
            private Vector3 lastSeenPosition;
            
#region State Machine Functions
            protected override void SetUp()
            {
                Name = "Chase";
            }

            public override void Enter()
            {
                lastSeenPosition = GetClosestTarget(enemyController.targetsInVisionRange).position;
                enemyController.NavMeshAgent.destination = lastSeenPosition;
            }

            public override void FixedUpdate()
            {
                // Update last seen position
                if (enemyController.targetsInVisionRange.Count > 0)
                {
                    lastSeenPosition = GetClosestTarget(enemyController.targetsInVisionRange).position;
                }
                
                NavmeshUpdateCheck(lastSeenPosition);
                
                // Reached last known target position
                if (!enemyController.NavMeshAgent.hasPath)
                {
                    enemyController.ChangeState(enemyController.AlertEnemyState);
                }
            }
            
            //public override void Exit(){}
 #endregion
        }
    }
}