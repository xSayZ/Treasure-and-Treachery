// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Enemy roam state
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;


namespace Game {
    namespace Enemy {
        [System.Serializable]
        public class RoamEnemyState : EnemyState
        {
            
#region Public Functions
            public override void Enter()
            {
                List<Vector3> visibleTargets = LookForTarget(enemyController.transform.position, enemyController.transform.forward, enemyController.VisionRange, enemyController.VisionFov, enemyController.PlayerLayerMask, enemyController.ObstacleLayerMask);
                List<Vector3> audibleTargets = ListenForTarget(enemyController.transform.position, enemyController.HearingRange, enemyController.PlayerLayerMask);
                
                if (visibleTargets.Count + audibleTargets.Count > 0)
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
            
            public override void FixedUpdate(){}
 #endregion
        }
    }
}