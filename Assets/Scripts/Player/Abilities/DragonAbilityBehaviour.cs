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
            [SerializeField] private float healInterval;
            [SerializeField] private int healAmount;
            
            private bool started;
            private float currentHealInterval;

            protected override void Setup()
            {
                started = true;
                
                playerController.PlayerOverheadUIBehaviour.UpdatePersonalObjective(playerController.PlayerData.personalObjective.ToString());
                
                QuestManager.OnGoldPickedUp.AddListener(OnGoldPickedUp);
            }

            protected override void OnDisable()
            {
                base.OnDisable();
                
                if (playerController)
                {
                    QuestManager.OnGoldPickedUp.RemoveListener(OnGoldPickedUp);
                }
            }

            protected override void OnDashKill(bool _stunned)
            {
                QuestManager.OnGoldPickedUp.Invoke(playerController.PlayerIndex, goldOnDashKill);
            }

            private void OnGoldPickedUp(int _playerIndex, int _amount)
            {
                if (_playerIndex == playerController.PlayerData.playerIndex)
                {
                    playerController.PlayerData.personalObjective += _amount;
                    playerController.PlayerData.personalObjectiveThisLevel += _amount;
                    playerController.PlayerOverheadUIBehaviour.UpdatePersonalObjective(playerController.PlayerData.personalObjective.ToString());
                }
            }

            private void Update()
            {
                if (currentHealInterval >= healInterval)
                {
                    currentHealInterval = 0;
                    playerController.Heal(healAmount);
                }
                else
                {
                    currentHealInterval += Time.deltaTime;
                }
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