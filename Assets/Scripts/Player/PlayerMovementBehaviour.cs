// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Player
    {
        public class PlayerMovementBehaviour : MonoBehaviour
        {
            
            
            [SerializeField] private float MaxmovementSpeed;
            [SerializeField] private float turnSpeed;
            [SerializeField]private Rigidbody playerRigidBody;
            public Vector3 direction { get; private set; }
           
            public float AccelerationFactor;

            [Range(0, 1)] public float InputDamp;

            private float currentSpeed;
            private Vector3 currentInputVector;
            private Vector3 movement;
            private Vector3 smoothVelocity;
            #region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void FixedUpdate()
            {
                MovePlayer();
                TurnPlayer();
            }
        #endregion

        #region Public Functions

        public void MovementData(Vector3 _directionVector)
        {
            currentSpeed += AccelerationFactor * Time.deltaTime;
            Debug.Log(currentSpeed);
            direction = _directionVector;

        }
        #endregion

        #region Private Functions
        //TODO:: Add Interpolation for moving
        private void MovePlayer()
        {
            
            currentSpeed +=  AccelerationFactor * Time.deltaTime;
            if (currentSpeed>= MaxmovementSpeed) currentSpeed = MaxmovementSpeed;


            currentInputVector = Vector3.SmoothDamp(currentInputVector, direction+transform.position,ref smoothVelocity,InputDamp);
            
            playerRigidBody.MovePosition(currentInputVector);

        }
        //TODO:: Add interpolation for turning
        private void TurnPlayer()
        {
            transform.LookAt(direction+transform.position);
        }


        
        #endregion
        }
    }
}
