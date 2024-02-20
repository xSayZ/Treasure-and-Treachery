// /*------------------------------
// --------------------------------
// Creation Date: 2024/01/30
// Author: Fredrik
// Description: This script is responsible for the attack behaviour of the player.
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using Game.Audio;
using Game.Backend;
using Game.Core;
using Game.Quest;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Game {
    namespace Player {
        public class PlayerAttackBehaviour : MonoBehaviour {
            [Header("Component References")]
            [SerializeField] private CapsuleCollider weaponCollider;
            [SerializeField] private GameObject projectile;
            
            [Header("Melee Attack Settings")]
            [SerializeField] private int meleeAttackDamage;
            [SerializeField] private float meleeAttackCooldown;
            [SerializeField] private float meleeAttackDuration;
            [SerializeField] private float meleeAttackDelay;
            [SerializeField] private float meleeChargeSpeed;
            [SerializeField] private float meleeChargeTime;
            
            [Header("Ranged Attack Settings")]
            [SerializeField] private int rangedAttackDamage;
            [SerializeField] private float rangedAttackCooldown;
            [SerializeField] private float projectileSpeed;
            
            [Header("Audio")]
            [SerializeField] private GameObject playerObj;
            [SerializeField] private PlayerAudio playerAudio;
            
            // Melee
            private List<IDamageable> damageableInRange;
            private float currentMeleeCooldown;
            private bool isMeleeAttacking;
            
            // Ranged
            private float currentRangedCooldown;
            
            private PlayerController playerController;

#region Unity Functions
            private void OnEnable()
            {
                QuestManager.OnMeleeWeaponPickedUp.AddListener(ActivateMeleeWeapon);
                QuestManager.OnRagedWeaponPickedUp.AddListener(ActivateRangedWeapon);
            }

            private void OnDisable()
            {
                QuestManager.OnMeleeWeaponPickedUp.RemoveListener(ActivateMeleeWeapon);
                QuestManager.OnRagedWeaponPickedUp.RemoveListener(ActivateRangedWeapon);
            }

            private void Awake()
            {
                damageableInRange = new List<IDamageable>();
                
                playerController = GetComponent<PlayerController>();
            }

            private void Update()
            {
                if (currentMeleeCooldown > 0)
                {
                    currentMeleeCooldown -= Time.deltaTime;
                }
                
                if (currentRangedCooldown > 0)
                {
                    currentRangedCooldown -= Time.deltaTime;
                }
            }
#endregion

#region Public Functions
            public void Melee(PlayerAnimationBehaviour _playerAnimationBehaviour, PlayerMovementBehaviour _playerMovementBehaviour)
            {
                if (playerController.PlayerData.currentItem != null || !playerController.PlayerData.hasMeleeWeapon || currentMeleeCooldown > 0)
                {
                    return;
                }
                
                currentMeleeCooldown = meleeAttackCooldown;
                
                _playerAnimationBehaviour.PlayMeleeAttackAnimation(); 
                
                StartCoroutine(MeleeAttack(_playerMovementBehaviour));
            }

            public void Aim(bool _aiming, PlayerMovementBehaviour _playerMovementBehaviour)
            {
                if (playerController.PlayerData.currentItem != null || !playerController.PlayerData.hasRangedWeapon || currentRangedCooldown > 0)
                {
                    return;
                }
                
                if (_aiming)
                {
                    _playerMovementBehaviour.TurnSpeed /= 2;
                    _playerMovementBehaviour.SetMovementActiveState(false, true);
                }
                else
                {
                    RangedAttack();
                    _playerMovementBehaviour.TurnSpeed *= 2;
                    _playerMovementBehaviour.SetMovementActiveState(true, true);
                    currentRangedCooldown = rangedAttackCooldown;
                }
            }

            public void OnAttackRangeEnter(Transform _transform)
            {
                if (_transform.gameObject.CompareTag("Enemy"))
                {
                    if (_transform.TryGetComponent(out IDamageable _hit))
                    {
                        damageableInRange.Add(_hit);

                        if (isMeleeAttacking)
                        {
                            _hit.Damage(meleeAttackDamage);
                            Debug.Log("Extended kill");
                        }
                    }
                }
            }

            public void OnAttackRangeExit(Transform _transform)
            {
                if (_transform.TryGetComponent(out IDamageable _hit))
                {
                    damageableInRange.Remove(_hit);
                }
            }
#endregion

#region Private Functions
            private IEnumerator MeleeAttack(PlayerMovementBehaviour _playerMovementBehaviour)
            {
                yield return new WaitForSeconds(meleeAttackDelay);
                
                _playerMovementBehaviour.ApplyForce(meleeChargeSpeed, transform.forward, meleeChargeTime);
                
                // Loop through all enemies in range
                for (int i = damageableInRange.Count - 1; i >= 0; i--)
                {
                    if (!(damageableInRange[i] as Object)) // Null check
                    {
                        damageableInRange.Remove(damageableInRange[i]);
                        continue;
                    }
                    
                    bool killed = damageableInRange[i].Damage(meleeAttackDamage);
                    
                    if (killed)
                    {
                        playerController.PlayerData.kills += 1;
                        playerController.PlayerData.killsThisLevel += 1;
                        EnemyManager.OnEnemyDeathUI.Invoke();
                    }
                }
                
                try
                {
                    playerAudio.MeleeAudioPlay(playerObj);
                }
                catch (Exception e)
                {
                    Debug.LogError("[{PlayerController}]: Error Exception " + e);
                }
                
                isMeleeAttacking = true;
                yield return new WaitForSeconds(meleeAttackDuration);
                isMeleeAttacking = false;
            }
            
            private void RangedAttack()
            {
                GameObject _projectile = Instantiate(projectile, transform.position, Quaternion.identity);
                Projectile _playerProjectile = _projectile.GetComponent<Projectile>();
                _playerProjectile.SetValues(transform.forward, rangedAttackDamage, projectileSpeed, playerController.PlayerData);
            }

            private void ActivateMeleeWeapon(int _playerIndex)
            {
                if (_playerIndex == playerController.PlayerIndex)
                {
                   playerController.PlayerData.hasMeleeWeapon = true; 
                }
            }

            private void ActivateRangedWeapon(int _playerIndex)
            {
                if (_playerIndex == playerController.PlayerIndex)
                {
                    playerController.PlayerData.hasRangedWeapon = true;
                }
            }
#endregion
        }
    }
}