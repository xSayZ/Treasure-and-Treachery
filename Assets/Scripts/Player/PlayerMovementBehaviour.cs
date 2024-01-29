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

            private Rigidbody playerRigidBody;
            private Vector3 direction;

        #region Unity Functions
            // Start is called before the first frame update

            private void Awake()
            {
                playerRigidBody = GetComponent<Rigidbody>();

            }
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
                
            }
        #endregion

        #region Public Functions

        public void MovePlayer(Vector3 directionVector)
        {
            playerRigidBody.MovePosition((transform.position+directionVector)*Time.deltaTime*movementSpeed);
        }
        #endregion

        #region Private Functions

#endregion
        }
    }
}
