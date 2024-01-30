// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Enemy roam state
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Enemy {
        [System.Serializable]
        public class RoamEnemyState : EnemyState
        {
            
#region State Machine Functions
            //public override void Enter(){}

            public override void FixedUpdate()
            {
                if (enemyController.targetsInVisionRange.Count + enemyController.targetsInHearingRange.Count > 0)
                {
                    // This is only needed in alert state
                    /*Vector3 closestTarget;
                    float closestDistance = float.MaxValue;

                    for (int i = 0; i < targets.Count; i++)
                    {
                        float distance = Vector3.Distance(enemyController.transform.position, targets[i]);

                        if (distance < closestDistance)
                        {
                            closestTarget = targets[i];
                            closestDistance = distance;
                        }
                    }*/
                    
                    enemyController.ChangeState(enemyController.AlertEnemyState);
                }
            }
            
            //public override void Exit(){}
 #endregion
        }
    }
}