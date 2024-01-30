// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Scene {
        public class Pickup : MonoBehaviour
        {

            public bool IsObjectivePickup;


#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
                
            }

            private void OnTriggerEnter(Collider other)
            {
                if (other.CompareTag("Player"))
                {
                    if (IsObjectivePickup)
                    {
                        // TODO: Tell game pickup has been added
                        //other.hasObjective = true;

                    }
                    Destroy(gameObject);
                }
            }
            #endregion

            #region Public Functions

            #endregion

            #region Private Functions

            #endregion
        }
    }
}
