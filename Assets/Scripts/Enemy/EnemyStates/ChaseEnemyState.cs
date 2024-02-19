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

            private EnemyAttackBehaviour enemyAttackBehaviour;
            private Vector3 lastTargetPosition;
            private Vector3 positionLastUpdate;
            private bool isRotating;
            private float rotationTime;

#region State Machine Functions
            protected override void SetUp()
            {
                Name = "Chase";
                
                isRotating = false;
                rotationTime = 0;
                enemyAttackBehaviour = enemyController.GetEnemyAttackBehaviour();
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
                    if (enemyAttackBehaviour.GetTargetsInAttackRangeCount() <= 0)
                    {
                        enemyController.ChangeState(enemyController.AlertEnemyState);
                    }
                    else
                    {
                        isRotating = true;
                        
                        Vector3 _lookVector = lastTargetPosition - enemyController.transform.position;
                        _lookVector.y = 0;
                        Quaternion _lookRotation = Quaternion.LookRotation(_lookVector);
                        enemyController.transform.rotation = Quaternion.Slerp(enemyController.transform.rotation, _lookRotation, rotationTime);
                    }
                }
                
                // Update rotation time
                if (isRotating)
                {
                    rotationTime += Time.fixedDeltaTime;
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