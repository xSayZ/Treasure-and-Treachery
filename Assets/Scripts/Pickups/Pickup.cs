// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-02
// Author: alexa
// Description: Item that can be picked up
// --------------------------------
// ------------------------------*/

using Game.Quest;
using Game.Player;
using UnityEngine;


namespace Game {
    namespace Scene {
        public class Pickup : MonoBehaviour
        {
            public enum PickupTypes
            {
                QuestItem,
                Gold
            }
            
            [Header("Pickup Type")]
            public PickupTypes PickupType;
            
            // Quest item variables
            [HideInInspector] public int Weight;
            
            // Gold variables
            [HideInInspector] public int Amount;

#region Unity Functions
            
            
            private void OnTriggerEnter(Collider other)
            {
                if (other.CompareTag("Player"))
                {
                    int _playerIndex = other.gameObject.GetComponent<PlayerController>().PlayerData.playerIndex;
                    
                    switch (PickupType)
                    {
                        case PickupTypes.QuestItem:
                            QuestManager.OnQuestItemPickedUp.Invoke(_playerIndex, Weight, this);
                            break;
                        
                        case PickupTypes.Gold:
                            QuestManager.OnGoldPickedUp.Invoke(_playerIndex, Amount);
                            break;
                    }
                    
                    Destroy(gameObject);
                }
            }
#endregion
        }
    }
}
