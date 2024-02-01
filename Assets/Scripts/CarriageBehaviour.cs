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
            private bool availableToEndGame = false;
            [SerializeField] private int amountForObjective = 3;
            private int playerID;
            private int playerCounter;
            [SerializeField] GameObject carriageTeleport;
            
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
                    // When they use the shovel, decrease amountForObjective
                    // If amountForObjective is 0, stop them from being able to use shovel then make it available to leave
                    if (amountForObjective == 0) {
                        canDoObjective = false;
                        availableToEndGame = true;
                    }
                }

                if (other.CompareTag("Player") && availableToEndGame) {
                    var player = other.gameObject;
                    player.GetComponent<PlayerController>().SetInputActiveState(availableToEndGame);
                    player.transform.position = carriageTeleport.transform.position;
                    player.transform.localScale = new Vector3(0,0,0);
                }
            }


#endregion

#region Public Functions

            public void UpdateObjective() {
                amountForObjective--;
            }

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
