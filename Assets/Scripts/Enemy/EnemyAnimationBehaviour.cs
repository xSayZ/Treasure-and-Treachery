// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;


namespace Game {
    namespace NAME {
        public class EnemyAnimationBehaviour : MonoBehaviour
        {
            [Header("Component References")]
            public Animator EnemyAnimator;

            private int enemyMovementAnimationID;
            private int enemyAttackAnimationID;

            public void SetupBehaviour()
            {
                EnemyAnimator = GetComponent<Animator>();
                SetupAnimationIDs();
            }

            void SetupAnimationIDs()
            {
                enemyMovementAnimationID = Animator.StringToHash("Movement");
                enemyAttackAnimationID = Animator.StringToHash("Attack");
            }

            public void UpdateMovementAnimation(float movementBlendValue)
            {
                EnemyAnimator.SetFloat(enemyMovementAnimationID, movementBlendValue);
            }

            public void PlayAttackAnimation()
            {
                EnemyAnimator.SetTrigger(enemyAttackAnimationID);
            }
        }
    }
}
