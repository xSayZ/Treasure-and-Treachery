// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-21
// Author: b22alesj
// Description: The dragons ability
// --------------------------------
// ------------------------------*/

using Game.Core;
using Game.Scene;
using UnityEngine;


namespace Game {
    namespace Player {
        public class DragonAbilityBehaviour : PlayerAbilityBehaviour
        {
            private bool started;

            protected override void Setup()
            {
                started = true;
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

            private void EnemyDied()
            {
                
            }
        }
    }
}