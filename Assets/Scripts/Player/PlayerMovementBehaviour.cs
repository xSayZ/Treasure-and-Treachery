// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;


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
            private Vector3 smoothMovementDirection;
            
            private Vector3 oldPosition;


            [Header("Test Values")]
            public float DashModifier;
            public float BasedashTime;
            private float currentDashTime;
            private bool dashComplete;
            
            private void OnValidate()
            {
                if (MovementSmoothing <= 0)
                {
                    Debug.LogWarning($"Smoothing must be between 0.1f-2f");
                    MovementSmoothing = 0.1f;
                }
            }


            #region Unity Functions

            private void Start()
            {
                currentDashTime = BasedashTime;
            }

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
            
            movement = Time.deltaTime * MaxmovementSpeed * smoothMovementDirection;
            playerRigidBody.MovePosition(movement+transform.position);

        }
        private void TurnPlayer()
        {
            transform.LookAt(smoothMovementDirection+transform.position);
        }
        
        
        private void SmoothInputMovement()
        {
            smoothMovementDirection = Vector3.Lerp(smoothMovementDirection, rawInputDirection,
                Time.deltaTime * MovementSmoothing);
        }

        
        #endregion
        }
    }
}
