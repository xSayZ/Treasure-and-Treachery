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
            private Animator Animator;

#region Unity Functions
            // Start is called before the first frame update
            private void Awake()
            {
                Animator = GetComponent<Animator>();
            }



            private void SmoothAnimation(float smoothing)
            {
                
                
            }
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
                
            }
#endregion

#region Public Functions

#endregion

#region Private Functions

#endregion
        }
    }
}
