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
            private int playerMeleeAttackAnimationID;
            private int playerInteractAnimationID;

#region Unity Functions
            // Start is called before the first frame update
            private void Awake()
            {
                PlayerAnimator = GetComponent<Animator>();
            }

            public void SetupBehaviour() {
                SetupAnimationIDs();
            }

            void SetupAnimationIDs() {
                playerMovementAnimationID = Animator.StringToHash("Movement");
                playerMeleeAttackAnimationID = Animator.StringToHash("MeleeAttack");
                playerInteractAnimationID = Animator.StringToHash("Interact");
            }
#endregion

#region Public Functions

            public void UpdateMovementAnimation(float movementBlendValue) {
                PlayerAnimator.SetFloat(playerMovementAnimationID, movementBlendValue);
            }

            public void PlayMeleeAttackAnimation() {
                PlayerAnimator.SetTrigger(playerMeleeAttackAnimationID);
            }

#endregion
        }
    }
}
