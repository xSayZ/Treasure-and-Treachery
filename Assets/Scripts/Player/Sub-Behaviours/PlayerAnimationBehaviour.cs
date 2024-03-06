// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;


namespace Game {
    namespace Player {
        public class PlayerAnimationBehaviour : MonoBehaviour
        {
            [Header("Component References")]
            public Animator PlayerAnimator;

            private int playerMovementAnimationID;
            private int playerAttackAnimationID;
            private int playerChargeAnimationID;
            private int playerInteractAnimationID;

#region Unity Functions

            public void SetupBehaviour() {
                PlayerAnimator = GetComponent<Animator>();
                SetupAnimationIDs();
            }

            void SetupAnimationIDs() {
                playerMovementAnimationID = Animator.StringToHash("Movement");
                playerAttackAnimationID = Animator.StringToHash("Attack");
                playerChargeAnimationID = Animator.StringToHash("AttackCharge");
                playerInteractAnimationID = Animator.StringToHash("Interact");
            }
#endregion

#region Public Functions

            public void UpdateMovementAnimation(float _movementBlendValue) {
                PlayerAnimator.SetFloat(playerMovementAnimationID, _movementBlendValue);
            }

            public void UpdateAttackChargeAnimation(float _movementBlendValue) {
                PlayerAnimator.SetFloat(playerChargeAnimationID, _movementBlendValue);
            }

            public void PlayAttackAnimation() {
                PlayerAnimator.SetTrigger(playerAttackAnimationID);
            }

#endregion
        }
    }
}
