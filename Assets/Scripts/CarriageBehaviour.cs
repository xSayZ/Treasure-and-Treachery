// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using Game.Events;
using Game.Player;

namespace Game {
    namespace Scenes {
        public class CarriageBehaviour : MonoBehaviour
        {
            private bool canDoObjective = false;
            private int playerID;
            
            private void Start() {
                EventManager.OnObjectivePickup.AddListener(ObjectiveCheck);
            }

#region Unity Functions
            void OnTriggerEnter(Collider other)
            {
                if (other.CompareTag("Player") && 
                    canDoObjective && 
                    other.gameObject.transform.GetComponent<PlayerController>().PlayerData.playerIndex == playerID)
                {
                    //TODO: Send to player that they can use pickup
                }
            }


#endregion

#region Public Functions

#endregion

#region Private Functions

            private void ObjectiveCheck(bool arg0, int playerIndex){
                canDoObjective = arg0;
                playerID = playerIndex;
            }
#endregion

        }
    }
}
