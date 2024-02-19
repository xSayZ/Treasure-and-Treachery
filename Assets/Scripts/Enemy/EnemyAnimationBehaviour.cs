// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: b22feldy
// Description: Controls enemy animations
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Enemy {
        public class EnemyAnimationBehaviour : MonoBehaviour
        {
            private Animator enemyAnimator;
            private int enemyMovementAnimationID;
            private int enemyAttackAnimationID;

#region Public Functions
            public void SetupBehaviour()
            {
                enemyAnimator = GetComponent<Animator>();
                SetupAnimationIDs();
            }
            
            public void UpdateMovementAnimation(float movementBlendValue)
            {
                enemyAnimator.SetFloat(enemyMovementAnimationID, movementBlendValue);
            }
            
            public void PlayAttackAnimation()
            {
                enemyAnimator.SetTrigger(enemyAttackAnimationID);
            }
#endregion

#region Private Functions
            private void SetupAnimationIDs()
            {
                enemyMovementAnimationID = Animator.StringToHash("Movement");
                enemyAttackAnimationID = Animator.StringToHash("Attack");
            }
#endregion
        }
    }
}