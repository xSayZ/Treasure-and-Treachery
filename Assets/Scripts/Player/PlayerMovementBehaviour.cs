// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;


namespace Game {
    namespace Player
    {
        public class PlayerMovementBehaviour : MonoBehaviour
        {
            

            [Header("MovementSettings")]
            [Tooltip("Effects How smooth the movement Interpolation is  is")]
            [SerializeField] public float MovementSmoothing;

            [SerializeField] private float BaseMoveSpeed;
            [SerializeField] private float turnSpeed;
            private float currentSpeed;
            
            private Rigidbody playerRigidBody;

           
            private Vector3 movement;
            private Vector3 rawInputDirection = Vector3.zero;
            private Vector3 smoothMovementDirection;

            [Header("Dash Settings")] 
            [SerializeField,Tooltip("Addition modifier adds modified speed to DashSpeed")]
            private float DashSpeedModifier;
            [SerializeField,Tooltip("How long should you be able to Dash")] private float DashTime;
            
            [SerializeField,Tooltip("How long before able to Dash Again")] private float DashLockoutPeriod;
            public float currentLockoutTime { get; private set; }
            
            private bool lockout;

            private void OnValidate()
            {

                if (DashLockoutPeriod <0 )
                {
                    Debug.LogWarning("lockout period needs to be higher than 0");
                    DashLockoutPeriod = 0.1f;
                }
            }
            #region Unity Functions

            private void Start()
            {
                playerRigidBody = GetComponent<Rigidbody>();
                currentSpeed = BaseMoveSpeed;
                currentLockoutTime = 0;
            }

            private void Update()
            {
                DashCompletion();
            }

            void FixedUpdate()
            {
                SmoothInputMovement();
                TurnPlayer();
                MovePlayer();
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
                if (rawInputDirection == Vector3.zero)
                {
                    rawInputDirection = Vector3.zero;
                }
                movement = Time.fixedDeltaTime * currentSpeed * rawInputDirection;
                playerRigidBody.AddForce(movement,ForceMode.VelocityChange);
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
            
        }

        public void Dash(bool dash)
        {
            if (dash)
            {
                StartCoroutine(IsDashing());
                currentLockoutTime = DashLockoutPeriod;
            }
        }

        private IEnumerator IsDashing()
        {
            currentSpeed = BaseMoveSpeed + DashSpeedModifier;
            yield return new WaitForSeconds(DashTime);
            currentSpeed = BaseMoveSpeed;
            lockout = true;
        }
        private void DashCompletion()
        {
            if (lockout)
            {
                currentLockoutTime -=Time.deltaTime;
            }

            if (currentLockoutTime <= 0)
            {
                lockout = false;
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
