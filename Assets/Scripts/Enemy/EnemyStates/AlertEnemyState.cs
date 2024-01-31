// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Enemy alert state
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Enemy {
        [System.Serializable]
        public class AlertEnemyState : EnemyState
        {
            [SerializeField] private float alertTime;
            
            private float currentAlertTime;
            private bool hasHeardSomething;
            private Vector3 lastHeardPosition;
            
#region State Machine Functions
            protected override void SetUp()
            {
                Name = "Alert";
            }

            public override void Enter()
            {
                currentAlertTime = 0;
                hasHeardSomething = false;
                if (enemyController.targetsInHearingRange.Count > 0)
                {
                    hasHeardSomething = true;
                    lastHeardPosition = GetClosestTarget(enemyController.targetsInHearingRange).position;
                    enemyController.NavMeshAgent.destination = lastHeardPosition;
                }
            }

            public override void FixedUpdate()
            {
                // Check if seen target
                if (enemyController.targetsInVisionRange.Count > 0)
                {
                    enemyController.ChangeState(enemyController.GrowlEnemyState);
                }
                
                // Update last heard position
                if (enemyController.targetsInHearingRange.Count > 0)
                {
                    hasHeardSomething = true;
                    lastHeardPosition = GetClosestTarget(enemyController.targetsInHearingRange).position;
                }
                
                if (hasHeardSomething)
                {
                   NavmeshUpdateCheck(lastHeardPosition); 
                }
                
                // Reached last known target position
                if (!enemyController.NavMeshAgent.hasPath)
                {
                    currentAlertTime += Time.fixedDeltaTime;
                    if (currentAlertTime >= alertTime)
                    {
                        enemyController.ChangeState(enemyController.RoamEnemyState);
                    }
                }
            }
            
            //public override void Exit(){}
 #endregion
        }
    }
}