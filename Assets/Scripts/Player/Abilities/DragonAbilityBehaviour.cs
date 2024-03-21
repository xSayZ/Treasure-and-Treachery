// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-21
// Author: b22alesj
// Description: The dragons ability
// --------------------------------
// ------------------------------*/

using System;
using Game.Audio;
using Game.Core;
using Game.Enemy;
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

            [Header("Audio")] 
            [SerializeField] private PlayerAudio playerAudio;
            
            private bool started;
            private float currentHealInterval;

            protected override void Setup()
            {
                started = true;
                
                playerController.PlayerOverheadUIBehaviour.UpdatePersonalObjective(playerController.PlayerData.personalObjective, 0);
                
                playerController.PlayerMovementBehaviour.OnDashThroughEnemy.AddListener(OnDashThroughEnemy);
            }

            protected override void OnDisable()
            {
                base.OnDisable();
                
                if (playerController)
                {
                    playerController.PlayerMovementBehaviour.OnDashThroughEnemy.AddListener(OnDashThroughEnemy);
                }
            }

            private void OnDashThroughEnemy(EnemyController _enemyController)
            {
                if (_enemyController.TryStealGold())
                {
                    OnPersonalGoldPickedUp(goldOnDashKill);
                    try
                    {
                        playerAudio.DragonPickPocket(gameObject);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning("[{DragonAbilityBehaviour}]: Error Exception " + e);
                    }
                }
            }

            private void OnPersonalGoldPickedUp(int _amount)
            {
                playerController.PlayerData.personalObjective += _amount;
                playerController.PlayerData.personalObjectiveThisLevel += _amount;
                playerController.PlayerOverheadUIBehaviour.UpdatePersonalObjective(playerController.PlayerData.personalObjective, _amount);
                QuestManager.PersonalObjectiveScoreUpdated(playerController.PlayerIndex, playerController.PlayerData.personalObjective);
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
                            OnPersonalGoldPickedUp(_pickUp.Amount);
                            Destroy(_pickUp.gameObject);

                            try
                            {
                                playerAudio.DragonPickPocket(gameObject);
                            }
                            catch (Exception e)
                            {
                                Debug.LogWarning("[{DragonAbilityBehaviour}]: Error Exception " + e);
                            }
                        }
                    }
                }
            }
        }
    }
}