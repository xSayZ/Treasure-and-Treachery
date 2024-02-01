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
            
            public float MovementSmoothing;
            [SerializeField] private float MaxmovementSpeed;
            [SerializeField]private Rigidbody playerRigidBody;
            private float currentSpeed = 0.01f;
            private Vector3 movement;
            private Vector3 rawInputDirection;
            private Vector3 smoothMovementDirection;
            
            private Vector3 oldPosition;
            

            [Header("Test Values")]
            public float DashModifier;
            public float BasedashTime;
            private float currentDashTime;
            private bool dashComplete;

            [Header("Movement Type Temp stuff"),Tooltip("If using Velocity Modify MaxmovementSpeed to 1000")] 
            public bool useVelocity;

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

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider)
            {
                playerRigidBody.velocity = Vector3.zero;

            }
        }

        private void MovePlayer()
        {
            
            if (useVelocity)
            {
                movement = Time.fixedDeltaTime * MaxmovementSpeed * smoothMovementDirection;
                playerRigidBody.velocity = smoothMovementDirection + movement;
            }
            else
            {
                currentSpeed = MaxmovementSpeed;
                movement = Time.deltaTime * currentSpeed * smoothMovementDirection;
                playerRigidBody.MovePosition(transform.position+movement);

            }

        }
        public void TurnPlayer()
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
