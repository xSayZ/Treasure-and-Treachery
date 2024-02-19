// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Enemy growl state
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Enemy {
        [System.Serializable]
        public class GrowlEnemyState : EnemyState
        {
#region State Machine Functions
            protected override void SetUp()
            {
                Name = "Growl";
            }

            public override void Enter()
            {
                // Alert other zombies
                
                enemyController.ChangeState(enemyController.ChaseEnemyState);
            }

            //public override void FixedUpdate(){}

            //public override void Exit(){}
 #endregion
        }
    }
}