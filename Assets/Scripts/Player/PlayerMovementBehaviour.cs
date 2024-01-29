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
            
            
            [SerializeField] private float movementSpeed;
            [SerializeField] private float turnSpeed;
            [SerializeField]private Rigidbody playerRigidBody;
            private Vector3 direction;

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
            Vector3 movement = direction * Time.deltaTime * movementSpeed;
            playerRigidBody.MovePosition(transform.position+movement);
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
