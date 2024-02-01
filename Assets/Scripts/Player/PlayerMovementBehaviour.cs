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
    namespace Player
    {
        public class PlayerMovementBehaviour : MonoBehaviour
        {

            [Header("MovementSettings")]
            [Tooltip("Effects How effective turning is and inertial movement")]
        [Range(0.1f, 2f)]
            public float MovementSmoothing;

            [SerializeField] private float MaxmovementSpeed;
            [SerializeField]private Rigidbody playerRigidBody;
            private float currentSpeed;
            private Vector3 movement;
            
            private Vector3 rawInputDirection;
            public Vector3 SmoothMovementDirection{ get; private set; }

            private void OnValidate()
            {
                if (MovementSmoothing <= 0)
                {
                    Debug.LogWarning($"Smoothing must be between 0.1f-2f");
                    MovementSmoothing = 0.1f;
                }
            }


            #region Unity Functions
            void FixedUpdate()
            {
                SmoothInputMovement();
                MovePlayer();
                TurnPlayer();
            }
        #endregion

        #region Public Functions

        public void MovementData(Vector3 _directionVector)
        { 
           
            rawInputDirection = _directionVector;

        }
        #endregion

        #region Private Functions
        
        private void MovePlayer()
        {
            
            movement = Time.deltaTime * MaxmovementSpeed * SmoothMovementDirection;
            playerRigidBody.MovePosition(movement + transform.localPosition);

        }
        private void TurnPlayer()
        {
            transform.LookAt(SmoothMovementDirection+transform.position);
        }
        
        
        private void SmoothInputMovement()
        {
            SmoothMovementDirection = Vector3.Lerp(SmoothMovementDirection, rawInputDirection,
                Time.deltaTime * MovementSmoothing);
        }

        
        #endregion
        }
    }
}
