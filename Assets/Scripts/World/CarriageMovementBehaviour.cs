// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-19
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Racer {
        public class CarriageMovementBehaviour : MonoBehaviour
        {
            [Tooltip("Base Movement Speed of the player. This is the speed the player moves at when not dashing.")]
            [SerializeField] public float movementSpeed = 3f;
            
            [Tooltip("How fast the player turns")]
            [SerializeField] private float turnSpeed;

            // References
            private Rigidbody carriageRigidBody;
            
            // Stored Values
            private Vector3 movementDirection;
            public float currentSpeed { get; private set; }
            
            public bool canMove { get; private set; } = true;
            private bool canRotate = true;
            
            public void SetupBehaviour()
            {
                currentSpeed = movementSpeed;

                carriageRigidBody = GetComponent<Rigidbody>();
            }
            
#region Unity Functions
            
            private void FixedUpdate()
            {
                MoveCarriage();
                TurnCarriage();
            }
#endregion

#region Public Functions
                
                public void UpdateMovementData(Vector3 _newMovementDirection)
                {
                    movementDirection = _newMovementDirection;
                }
#endregion

#region Private Functions

                private void MoveCarriage()
                {
                    Vector3 _movement = Time.deltaTime * currentSpeed * movementDirection;
                    Debug.Log(_movement);
                    carriageRigidBody.AddForce(_movement, ForceMode.VelocityChange);
                }

                private void TurnCarriage()
                {
                    if (movementDirection != Vector3.zero && canRotate)
                    {
                        if (movementDirection.sqrMagnitude > 0.01f && movementDirection != Vector3.zero) {
                            var _rotation = Quaternion.Slerp(carriageRigidBody.rotation, Quaternion.LookRotation(movementDirection), turnSpeed);
                            carriageRigidBody.rotation = _rotation;
                        }
                    }
                }
#endregion
        }
    }
}
