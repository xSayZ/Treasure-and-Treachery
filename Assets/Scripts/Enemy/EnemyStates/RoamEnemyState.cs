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
            protected override void SetUp()
            {
                Name = "Roam";
            }

            //public override void Enter(){}

            public override void FixedUpdate()
            {
                // Roam around here
                
                if (enemyController.targetsInVisionRange.Count + enemyController.targetsInHearingRange.Count > 0)
                {
                    enemyController.ChangeState(enemyController.AlertEnemyState);
                }
            }
            
            //public override void Exit(){}
 #endregion
        }
    }
}