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
            private float currentSpeed;
            private Vector3 movement;
            
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
           
            direction = _directionVector;

        }
        #endregion

        #region Private Functions
        //TODO:: Add Interpolation for moving
        private void MovePlayer()
        {
            
            movement = Time.deltaTime * MaxmovementSpeed * direction;
            playerRigidBody.MovePosition(movement + transform.localPosition);

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
