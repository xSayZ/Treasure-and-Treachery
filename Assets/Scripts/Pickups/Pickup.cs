// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-02
// Author: alexa
// Description: Item that can be picked up
// --------------------------------
// ------------------------------*/

using Game.Core;
using Game.Quest;
using Game.Player;
using UnityEngine;


namespace Game {
    namespace Scene {
        public class Pickup : MonoBehaviour, IInteractable
        {
            public enum PickupTypes
            {
                QuestItem,
                Gold
            }
            
            [Header("Pickup Type")]
            public PickupTypes PickupType;
            
            // Quest item variables
            [Header("Item Settings")]
            public Item Item;
            
            // Gold variables
            [Header("Gold Settings")]
            public int Amount;

#region Public Functions
            public void Interact(int _playerIndex)
            {
                switch (PickupType)
                {
                    case PickupTypes.QuestItem:
                        QuestManager.OnItemPickedUp.Invoke(_playerIndex, Item);
                        break;
                        
                    case PickupTypes.Gold:
                        QuestManager.OnGoldPickedUp.Invoke(_playerIndex, Amount);
                        break;
                }
                
                Destroy(gameObject);
            }
#endregion
        }
    }
}
