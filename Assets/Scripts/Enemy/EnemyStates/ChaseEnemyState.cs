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
            
            private NavMeshAgent navMeshAgent;
            private Vector3 lastSeenPosition;
            
#region State Machine Functions
            public override void Enter()
            {
                Debug.Log("Chase");
                
                navMeshAgent = enemyController.GetNavMeshAgent();
                
                lastSeenPosition = GetClosestTarget(enemyController.targetsInVisionRange).position;
                navMeshAgent.destination = lastSeenPosition;
            }

            public override void FixedUpdate()
            {
                // Update last seen position
                if (enemyController.targetsInVisionRange.Count > 0)
                {
                    lastSeenPosition = GetClosestTarget(enemyController.targetsInVisionRange).position;
                }
                
                // Update nav mesh agent destination if necessary
                Vector3 _directionToTarget = (navMeshAgent.destination - enemyController.transform.position).normalized;
                Vector3 _directionToLastSeenPosition = (lastSeenPosition - enemyController.transform.position).normalized;
                if (Vector3.Angle(_directionToTarget, _directionToLastSeenPosition) > maxDeviationAngle)
                {
                    navMeshAgent.destination = lastSeenPosition;
                    Debug.Log("Updated nav mesh agent destination");
                }
                
                // Lost target
                if (Vector3.Distance(enemyController.transform.position, lastSeenPosition) <= navMeshAgent.stoppingDistance)
                {
                    enemyController.ChangeState(enemyController.AlertEnemyState);
                }
            }
            
            //public override void Exit(){}
 #endregion
        }
    }
}