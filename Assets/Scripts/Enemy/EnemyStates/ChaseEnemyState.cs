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
                
                // Update nav mesh agent destination if angle is to much
                Vector3 _directionToTarget = (navMeshAgent.destination - enemyController.transform.position).normalized;
                Vector3 _directionToLastSeenPosition = (lastSeenPosition - enemyController.transform.position).normalized;
                if (Vector3.Angle(_directionToTarget, _directionToLastSeenPosition) > maxDeviationAngle)
                {
                    navMeshAgent.destination = lastSeenPosition;
                    //Debug.Log("Updated nav mesh agent destination (angle)");
                }
                
                // Update nav mesh agent destination if player is further away and destination has almost been reached
                if (navMeshAgent.remainingDistance < updateDistance && (navMeshAgent.destination.x != lastSeenPosition.x || navMeshAgent.destination.z != lastSeenPosition.z) && navMeshAgent.hasPath)
                {
                    navMeshAgent.destination = lastSeenPosition;
                    //Debug.Log("Updated nav mesh agent destination (distance)");
                }
                
                // Reached last known target position
                if (!navMeshAgent.hasPath)
                {
                    enemyController.ChangeState(enemyController.AlertEnemyState);
                }
            }
            
            //public override void Exit(){}
 #endregion
        }
    }
}