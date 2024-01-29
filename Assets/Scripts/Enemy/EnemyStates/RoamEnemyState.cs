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
                // TEMP
                List<Vector3> targets = LookForTarget(enemyController.transform.position, enemyController.transform.forward, 10, 60, enemyController.playerLayerMask, enemyController.obstacleLayerMask);

                for (int i = 0; i < targets.Count; i++)
                {
                    Debug.Log(targets[i]);
                }
            }
            
            public override void FixedUpdate(){}
 #endregion
        }
    }
}