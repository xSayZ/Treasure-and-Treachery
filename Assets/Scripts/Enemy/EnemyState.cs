// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Abstract enemy state class, states inherit from this class
// --------------------------------
// ------------------------------*/

namespace Game {
    namespace Enemy {
        public abstract class EnemyState
        {
            protected EnemyController enemyController;
            
#region Public Functions
            public void Awake(EnemyController e)
            {
                enemyController = e;
            }

            public virtual void Enter(){}
            
            public virtual void FixedUpdate(){}
#endregion
        }
    }
}