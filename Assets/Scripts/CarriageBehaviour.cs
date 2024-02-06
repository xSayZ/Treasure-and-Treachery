// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Carriage for leaving level
// --------------------------------
// ------------------------------*/

using Game.Backend;
using Game.Core;
using UnityEngine;
using Game.Quest;
using Game.Player;

namespace Game {
    namespace Scenes {
        public class CarriageBehaviour : MonoBehaviour, IInteractable
        {
            [SerializeField] GameObject playerTeleportPosition;
            
            private bool canLeave = true;
            private int playersInCarriage;

#region Unity Functions
            private void OnEnable()
            {
                QuestManager.OnRequiredQuestRegistered.AddListener(RequiredQuestRegistered);
                QuestManager.OnAllRequiredQuestsCompleted.AddListener(AllRequiredQuestsCompleted);
            }
            
            private void OnDisable()
            {
                QuestManager.OnRequiredQuestRegistered.RemoveListener(RequiredQuestRegistered);
                QuestManager.OnAllRequiredQuestsCompleted.RemoveListener(AllRequiredQuestsCompleted);
            }
#endregion

#region Public Functions
            public void Interact(int _playerIndex, bool _start)
            {
                if (_start && canLeave)
                {
                    GameObject player = GameManager.Instance.activePlayerControllers[_playerIndex];
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
#endregion

#region Private Functions
            private void RequiredQuestRegistered()
            {
                canLeave = false;
            }
            
            private void AllRequiredQuestsCompleted()
            {
                canLeave = true;
            }
#endregion
        }
    }
}
