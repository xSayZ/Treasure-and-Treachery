// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-21
// Author: b22alesj
// Description: The dragons ability
// --------------------------------
// ------------------------------*/

using Game.Core;
using Game.Quest;
using Game.Scene;
using UnityEngine;


namespace Game {
    namespace Player {
        public class DragonAbilityBehaviour : PlayerAbilityBehaviour
        {
            [Header("Settings")]
            [SerializeField] private int goldOnDashKill;
            
            private bool started;

            protected override void Setup()
            {
                started = true;
            }

            protected override void OnDashKill()
            {
                QuestManager.OnGoldPickedUp.Invoke(playerController.PlayerIndex, goldOnDashKill);
            }

            private void OnTriggerStay(Collider _other)
            {
                if (!started)
                {
                    return;
                }
                
                if (playerController.PlayerMovementBehaviour.IsDashing)
                {
                    if (_other.TryGetComponent(out Pickup _pickUp))
                    {
                        if (_pickUp.PickupType == Pickup.PickupTypes.Gold)
                        {
                            if (_other.TryGetComponent(out IInteractable _interactable))
                            {
                                _interactable.Interact(playerController.PlayerData.playerIndex, true);
                            }
                        }
                    }
                }
            }
        }
    }
}