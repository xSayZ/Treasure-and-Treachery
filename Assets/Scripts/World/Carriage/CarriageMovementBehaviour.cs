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

            public enum Axel
            {
                Front,
                Rear
            }

            [Serializable]
            public struct Wheel
            {
                public GameObject wheelModel;
                public WheelCollider wheelCollider;
                public GameObject wheelEffectObj;
                public ParticleSystem particleSystem;
                public Axel axel;
            }
            
            public float maxAcceleration = 30.0f;
            public float brakeAcceleration = 50.0f;

            public float turnSensitivity = 1.0f;
            public float maxSteerAngle = 30.0f;

            public Vector3 centreOfMass;

            public List<Wheel> wheels;
            
            private float moveInput;
            private float steerInput;
            private bool breaking;

            private Rigidbody carriageRb;
            
            public void SetupBehaviour()
            { 
                carriageRb = GetComponent<Rigidbody>();
                carriageRb.centerOfMass = centreOfMass;
            }
            
#region Unity Functions

            private void FixedUpdate() {
                Move();
                Steer();
                Brake();
            }
#endregion

#region Public Functions
                
            public void UpdateMovementData(Vector2 _input) { 
                moveInput = _input.y; 
                steerInput = _input.x;
            }
#endregion

#region Private Functions
            
             private void Move()
             {
                 foreach (var wheel in wheels) {
                     wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration;
                 }
             }

             private void Steer()
             {
                 foreach (var wheel in wheels) {
                     if (wheel.axel == Axel.Front) {
                         var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                         wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
                     }
                     
                 }
             }

             private void Brake()
             {
                 if (moveInput == 0) {
                     foreach (var wheel in wheels) {
                         wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration;
                     }
                 }
                 else {
                     foreach (var wheel in wheels) {
                         wheel.wheelCollider.brakeTorque = 0;
                     }
                 }
             }
#endregion
        }
    }
}
