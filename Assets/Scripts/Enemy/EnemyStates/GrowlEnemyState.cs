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
            public override void Enter()
            {
                Debug.Log("Growl");
                
                // Alert other zombies
                
                enemyController.ChangeState(enemyController.ChaseEnemyState);
            }
            
            //public override void FixedUpdate(){}
            
            //public override void Exit(){}
 #endregion
        }
    }
}