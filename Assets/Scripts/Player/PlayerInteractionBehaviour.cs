// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-08
// Author: b22alesj
// Description: Player interaction behaviour
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Core;
using Game.Quest;
using UnityEngine;
using UnityEngine.UI;


namespace Game {
    namespace Player {
        public class PlayerInteractionBehaviour : MonoBehaviour
        {
            [Header("UI")]
            [SerializeField] private GameObject itemImage;
            
            private PlayerController playerController;
            private IInteractable closestInteraction;
            private List<IInteractable> inInteractRange = new List<IInteractable>();

#region Unity Functions
            private void OnEnable()
            {
                // Subscribe to events
                QuestManager.OnItemPickedUp.AddListener(PickUpItem);
                QuestManager.OnItemDropped.AddListener(DropItem);
                QuestManager.OnGoldPickedUp.AddListener(PickUpGold);
            }
            
            private void OnDisable()
            {
                // Unsubscribe from events
                QuestManager.OnItemPickedUp.AddListener(PickUpItem);
                QuestManager.OnItemDropped.AddListener(DropItem);
                QuestManager.OnGoldPickedUp.AddListener(PickUpGold);
            }
            
            private void Start()
            {
                playerController = GetComponent<PlayerController>();
            }
            
            private void Update()
            {
                List<IInteractable> canInteractWith = new List<IInteractable>();
                
                // Null check
                for (int i = inInteractRange.Count - 1; i >= 0; i--)
                {
                    if (!(inInteractRange[i] as Object)) // Fancy null check because a normal null check dose not work for some reason
                    {
                        inInteractRange.Remove(inInteractRange[i]);
                    }
                }
                
                // Turn of interaction UI and check if player can interact with interaction
                for (int i = 0; i < inInteractRange.Count; i++)
                {
                    inInteractRange[i].ToggleInteractionUI(playerController.PlayerIndex, false);
                    
                    if (inInteractRange[i].CanInteractWith[playerController.PlayerIndex])
                    {
                        canInteractWith.Add(inInteractRange[i]);
                    }
                }
                
                // Set closest interaction
                if (canInteractWith.Count > 0)
                {
                    closestInteraction = canInteractWith[0];
                    float closestDistance = Vector3.Distance(transform.position, canInteractWith[0].InteractionTransform.position);
                    
                    for (int i = 1; i < canInteractWith.Count; i++)
                    {
                        float _distance = Vector3.Distance(transform.position, canInteractWith[i].InteractionTransform.position);
                        if (_distance < closestDistance)
                        {
                            closestDistance = _distance;
                            closestInteraction = canInteractWith[i];
                        }
                    }
                    
                    closestInteraction.ToggleInteractionUI(playerController.PlayerIndex, true);
                }
                else
                {
                    closestInteraction = null;
                }
            }
#endregion

#region Public Functions
            public void InteractRangeEntered(Transform _transform)
            {
                if (!_transform.TryGetComponent(out IInteractable _interactable))
                {
                    return;
                }
                
                inInteractRange.Add(_interactable);
            }
            
            public void InteractRangeExited(Transform _transform)
            {
                if (_transform.TryGetComponent(out IInteractable _interactable))
                {
                    inInteractRange.Remove(_interactable);
                    _interactable.ToggleInteractionUI(playerController.PlayerIndex, false);
                }
            }

            public void OnInteract(bool _pressed)
            {
                // Drop item
                if (inInteractRange.Count <= 0 && playerController.PlayerData.currentItem != null && _pressed)
                {
                    DropItem(playerController.PlayerIndex, playerController.PlayerData.currentItem, false);
                }
                
                // Interact with closest interaction
                if (closestInteraction != null)
                {
                    closestInteraction.Interact(playerController.PlayerIndex, _pressed);
                }
            }

            public void OnDeath()
            {
                if (playerController.PlayerData.currentItem != null)
                {
                    DropItem(playerController.PlayerIndex, playerController.PlayerData.currentItem, false);
                }
                
                closestInteraction.ToggleInteractionUI(playerController.PlayerIndex, false);
            }
#endregion

#region Private Functions
            private void PickUpItem(int _playerId, Item _item)
            {
                if (playerController.PlayerIndex != _playerId)
                {
                    return;
                }
                
                if (playerController.PlayerData.currentItem != null)
                {
                    DropItem(_playerId, playerController.PlayerData.currentItem, false);
                }
                
                _item.Pickup.SetActive(false);
                InteractRangeExited(_item.Pickup.transform);
                playerController.PlayerData.currentItem = _item;
                
                itemImage.GetComponent<Image>().sprite = _item.Sprite;
                itemImage.SetActive(true);
            }
            
            private void DropItem(int _playerId, Item _item, bool _destroy)
            {
                if (playerController.PlayerIndex != _playerId)
                {
                    return;
                }
                
                if (playerController.PlayerData.currentItem == _item)
                {
                    playerController.PlayerData.currentItem = null;
                    itemImage.SetActive(false);
                    
                    if (_destroy)
                    {
                        return;
                    }
                    
                    _item.Pickup.SetActive(true);
                    _item.Pickup.transform.position = transform.position;
                }
                else
                {
                    Debug.LogWarning("Cant remove item from player inventory since player does not have that item in their inventory"); 
                }
            }
            
            private void PickUpGold(int _playerId, int pickUpGold)
            {
                if (playerController.PlayerIndex == _playerId)
                {
                    playerController.PlayerData.currency += pickUpGold;
                }
            }
#endregion
        }
    }
}
