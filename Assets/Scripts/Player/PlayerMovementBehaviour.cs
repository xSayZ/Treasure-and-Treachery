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
            [Tooltip("Effects How smooth turning is")]
            
            public float MovementSmoothing;
            [SerializeField] private float MaxmovementSpeed;
            public float turnSpeed;
            [SerializeField]private Rigidbody playerRigidBody;
            private float currentSpeed = 0.01f;
            private Vector3 movement;
            private Vector3 rawInputDirection = Vector3.zero;
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
                if (rawInputDirection == Vector3.zero)
                {
                    rawInputDirection = Vector3.zero;
                }
                movement = Time.fixedDeltaTime * MaxmovementSpeed * rawInputDirection;
                playerRigidBody.AddForce(movement,ForceMode.VelocityChange);
                
            }
            else
            {
                Vector3 movement = CameraDirection(IsoVectorConvert(smoothMovementDirection)) *MaxmovementSpeed * Time.deltaTime;
                playerRigidBody.MovePosition(transform.position + movement);

            }

        }
        public void TurnPlayer()
        {
            if(smoothMovementDirection.sqrMagnitude > 0.01f)
            {

                Quaternion rotation = Quaternion.Slerp(playerRigidBody.rotation,
                    Quaternion.LookRotation (IsoVectorConvert(CameraDirection(smoothMovementDirection))),
                    turnSpeed);

                playerRigidBody.MoveRotation(rotation);

            }

            if (useVelocity)
            {
                transform.LookAt(transform.position+smoothMovementDirection);

            }
        }
        
        
        private void SmoothInputMovement()
        {
            smoothMovementDirection = Vector3.Lerp(smoothMovementDirection, rawInputDirection,
                Time.deltaTime * MovementSmoothing);
        }

        Vector3 CameraDirection(Vector3 movementDirection)
        {
            var cameraForward = UnityEngine.Camera.main.transform.forward;
            var cameraRight = UnityEngine.Camera.main.transform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;
        
            return cameraForward * movementDirection.z + cameraRight * movementDirection.x; 
   
        }
        
        private Vector3 IsoVectorConvert(Vector3 vector)
        {
            Vector3 cameraRot = UnityEngine.Camera.main.transform.rotation.eulerAngles;
            Quaternion rotation = Quaternion.Euler(0,cameraRot.y-90, 0);
            Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
            Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
            return result;

        }
      
        
        #endregion
        }
    }
}
