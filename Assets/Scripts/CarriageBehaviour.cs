// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Scenes {

        public class CarriageBehaviour : MonoBehaviour
        {
        
            

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
                    //if(Objectives == Done)
                    //End level
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
