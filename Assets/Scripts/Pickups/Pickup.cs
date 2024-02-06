// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-02
// Author: alexa
// Description: Item that can be picked up
// --------------------------------
// ------------------------------*/

using Game.Core;
using Game.Quest;
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
            
            [Header("Setup")]
            [SerializeField] private GameObject interactionUI;
            
            [Header("Pickup Type")]
            public PickupTypes PickupType;
            
            // Item variables
            [HideInInspector] public int Weight;
            [HideInInspector] public float InteractionTime;
            [HideInInspector] public Sprite ItemSprite;
            
            // Gold variables
            [HideInInspector] public int Amount;

            private Item item;

#region Unity Functions
            private void Awake()
            {
                CreateItem();
            }
#endregion

#region Public Functions
            public void Interact(int _playerIndex, bool _start)
            {
                if (!_start)
                {
                    return;
                }
                
                switch (PickupType)
                {
                    case PickupTypes.QuestItem:
                        QuestManager.OnItemPickedUp.Invoke(_playerIndex, item);
                        break;
                        
                    case PickupTypes.Gold:
                        QuestManager.OnGoldPickedUp.Invoke(_playerIndex, Amount);
                        break;
                }
            }
            
            public void InInteractionRange(int _playerIndex, bool _inRange)
            {
                interactionUI.SetActive(_inRange);
            }

            public Item GetItem()
            {
                return item;
            }
#endregion

#region Private Functions
            private void CreateItem()
            {
                item = new Item(Weight, InteractionTime, gameObject, ItemSprite);
            }
#endregion
        }
    }
}
