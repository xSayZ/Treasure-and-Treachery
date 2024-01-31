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
            [SerializeField] private float lookTime;
            [SerializeField] private float rotationSpeed;
            
            private float currentLookTime;
            private bool hasHeardSomething;
            private Vector3 lastHeardPosition;
            
#region State Machine Functions
            protected override void SetUp()
            {
                Name = "Alert";
            }

            public override void Enter()
            {
                currentLookTime = 0;
                hasHeardSomething = false;
                if (enemyController.targetsInHearingRange.Count > 0)
                {
                    hasHeardSomething = true;
                    lastHeardPosition = GetClosestTarget(enemyController.targetsInHearingRange).position;
                }
            }

            public override void FixedUpdate()
            {
                // Check if seen target
                if (enemyController.targetsInVisionRange.Count > 0)
                {
                    enemyController.ChangeState(enemyController.GrowlEnemyState);
                }
                
                // Look cooldown
                currentLookTime += Time.fixedDeltaTime;
                if (currentLookTime >= lookTime)
                {
                    enemyController.ChangeState(enemyController.RoamEnemyState);
                }
                
                // Update last heard position
                if (enemyController.targetsInHearingRange.Count > 0)
                {
                    hasHeardSomething = true;
                    lastHeardPosition = GetClosestTarget(enemyController.targetsInHearingRange).position;
                }
                
                // Turn towards last heard position
                if (hasHeardSomething)
                {
                    Vector3 _targetDirection = (lastHeardPosition - enemyController.transform.position).normalized;
                    Quaternion _lookRotation = Quaternion.LookRotation(_targetDirection);
                    enemyController.transform.rotation = Quaternion.Slerp(enemyController.transform.rotation, _lookRotation, Time.fixedDeltaTime * rotationSpeed);
                }
            }
            
            //public override void Exit(){}
 #endregion
        }
    }
}