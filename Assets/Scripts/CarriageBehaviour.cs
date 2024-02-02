// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Carriage for leaving level
// --------------------------------
// ------------------------------*/

using Game.Backend;
using UnityEngine;
using Game.Quest;
using Game.Player;

namespace Game {
    namespace Scenes {
        public class CarriageBehaviour : MonoBehaviour
        {
            [SerializeField] GameObject playerTeleportPosition;
            
            private bool canLeave;
            private int playersInCarriage;

#region Unity Functions
            private void OnEnable()
            {
                QuestManager.OnAllRequiredQuestsCompleted.AddListener(AllRequiredQuestsCompleted);
            }
            
            private void OnDisable()
            {
                QuestManager.OnAllRequiredQuestsCompleted.RemoveListener(AllRequiredQuestsCompleted);
            }
            
            private void OnTriggerEnter(Collider other)
            {
                if (other.CompareTag("Player"))
                {
                    if (canLeave)
                    {
                        GameObject player = other.gameObject;
                        player.GetComponent<PlayerController>().SetInputActiveState(true);
                        player.transform.position = playerTeleportPosition.transform.position;
                        player.transform.localScale = new Vector3(0,0,0);

                        playersInCarriage++;
                        if (playersInCarriage >= GameManager.Instance.activePlayerControllers.Count)
                        {
                            // All players are in carriage, time to end level
                            Debug.Log("Level Done");
                        }
                    }
                }
            }
#endregion

#region Private Functions
            private void AllRequiredQuestsCompleted()
            {
                canLeave = true;
            }
#endregion
        }
    }
}
