// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Enemy chase state
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
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
                UpdateLastTargetPosition(enemyController.targetsInVisionRange);
                enemyController.NavMeshAgent.destination = GetClosestPointOnNavmesh(lastTargetPosition);
            }

            public override void FixedUpdate()
            {
                // Update last seen position
                if (enemyController.targetsInVisionRange.Count > 0)
                {
                    UpdateLastTargetPosition(enemyController.targetsInVisionRange);
                }
                else if (enemyController.targetsInHearingRange.Count > 0)
                {
                    UpdateLastTargetPosition(enemyController.targetsInHearingRange);
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

#region State Machine Functions
            private void UpdateLastTargetPosition(List<Transform> _targets)
            {
                Transform _closestTarget = GetClosestTarget(_targets);
                
                if (_closestTarget.CompareTag("Carriage"))
                {
                    lastTargetPosition = _closestTarget.GetComponent<BoxCollider>().ClosestPoint(enemyController.transform.position);
                }
                else
                {
                    lastTargetPosition = _closestTarget.position;
                }
            }
#endregion
        }
    }
}