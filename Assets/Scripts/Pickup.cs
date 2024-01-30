// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using Game.Events;


namespace Game {
    namespace Scene {
        public class Pickup : MonoBehaviour
        {

            private bool IsObjectivePickup;
            private bool canBePickup = true;


#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                IsObjectivePickup = true;
            }
    
            // Update is called once per frame
            void Update()
            {
                
            }

            private void OnTriggerEnter(Collider other)
            {
                if (other.CompareTag("Player") && canBePickup)
                {
                    if (IsObjectivePickup)
                    {
                        // TODO: Tell game pickup has been added
                        //other.hasObjective = true;
                        EventManager.OnObjectivePickup.Invoke(true);
                    }

                    gameObject.SetActive(false);
                    canBePickup = false;
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
