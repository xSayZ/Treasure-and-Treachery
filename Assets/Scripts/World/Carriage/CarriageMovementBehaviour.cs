// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-19
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;


namespace Game {
    namespace Racer {
        public class CarriageMovementBehaviour : MonoBehaviour
        {
            [SerializeField] private float movementSpeed = 10.0f;
            [SerializeField] private float steerSpeed = 2.0f;
            
            private Rigidbody rb;
            private Vector3 movementDirection;
            
            public void SetupBehaviour()
            { 
                rb = GetComponent<Rigidbody>();
            }
            
#region Unity Functions
            private void FixedUpdate()
            {
                Move();
                Steer();
            }

            private void OnDisable()
            {
                rb.velocity = Vector3.zero;
            }
#endregion

#region Public Functions
                
            public void UpdateMovementData(Vector3 _newMovementDirection) { 
                movementDirection = _newMovementDirection;
            }
#endregion

#region Private Functions
            
             private void Move()
             {
                 Vector3 _movement = Time.deltaTime * movementSpeed * movementDirection;
                 rb.AddForce(_movement,ForceMode.VelocityChange);
             }

             private void Steer()
             {
                 if (movementDirection.sqrMagnitude > 0.01f) {
                     var _rotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(movementDirection), steerSpeed * Time.deltaTime);
                     rb.rotation = _rotation;
                 }
             }
            
#endregion
        }
    }
}
