// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using Game.Events;
using Game.Backend;
using System;

namespace Game {
    namespace Scenes {
        public class CarriageBehaviour : MonoBehaviour
        {
            private bool canDoObjective = false;
            
            private void Start() {
                EventManager.OnObjectivePickup.AddListener(BeginObjective);
            }

#region Unity Functions
            void OnTriggerEnter(Collider other)
            {
                if (other.CompareTag("Player") && canDoObjective)
                {
                    Debug.Log("Player inside");
                }
            }


            #endregion

            #region Public Functions

            #endregion

            #region Private Functions

            private void BeginObjective(bool arg0){
                Debug.Log("Pickup has been picked up?" +arg0);
                canDoObjective = arg0;
            }

            #endregion
        }
    }
}
