// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-28
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Racer {
        public class CarriageAnimationBehaviour : MonoBehaviour
        {
            [SerializeField] private Animator animator;
            
            private int carriageMovementAnimationID;

            public void SetupBehaviour() {
                animator = GetComponent<Animator>();
                SetupAnimationIDs();
            }
            
            void SetupAnimationIDs() {
                carriageMovementAnimationID = Animator.StringToHash("Movement");
            }
            
            public void UpdateMovementAnimation(float movementBlendValue) {
                if(animator != null)
                    animator.SetFloat(carriageMovementAnimationID, movementBlendValue);
            }
        }
    }
}
