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
using System.Threading.Tasks;
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
            [field:SerializeField,Tooltip("How long should you be able to Dash")] public float DashTime { get; private set; }
            [field:SerializeField,Tooltip("Dash Cooldown how long before able to Dash again after Dashing")]
            public float DashCooldown { get; private set; }
            [field:SerializeField]public float currentLockoutTime { get; private set; }
            
            private bool lockout;

            private void OnValidate()
            {

                if (DashTime <0 )
                {
                    Debug.LogWarning("lockout period needs to be higher than 0");
                    DashTime = 0;
                }
            }
            #region Unity Functions
            private void Start()
            {
                currentSpeed = BaseMoveSpeed;
                playerRigidBody = GetComponent<Rigidbody>();
                currentLockoutTime = 0;
            }

            private void Update()
            {
                SmoothInputMovement();
                TurnPlayer();
                DashCompletion();
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
                    playerRigidBody.velocity = Vector3.zero;
                }
                movement = Time.deltaTime * currentSpeed * rawInputDirection;
                playerRigidBody.AddForce(movement,ForceMode.VelocityChange);
        }
        public void TurnPlayer()
        {
            if(smoothMovementDirection.sqrMagnitude > 0.01f)
            {
    
                Quaternion rotation = Quaternion.Slerp(playerRigidBody.rotation,
                    Quaternion.LookRotation (smoothMovementDirection),
                    turnSpeed);

                playerRigidBody.rotation = rotation;
            }
            
        }

        public void Dash(bool dash)
        {
            if (dash && DashCooldown <=0)
            {
                StartCoroutine(IsDashing());
                currentLockoutTime = DashCooldown;
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
                Debug.Log(currentLockoutTime);
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
        #endregion

        }
    }
}
