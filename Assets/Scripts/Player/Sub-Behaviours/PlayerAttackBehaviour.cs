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
using Random = UnityEngine.Random;


namespace Game {
    namespace Player {
        public class PlayerAttackBehaviour : MonoBehaviour {
            [Header("Component References")]
            [SerializeField] private CapsuleCollider weaponCollider;
            [SerializeField] private GameObject projectile;
            [SerializeField] private GameObject aimLineLeft;
            [SerializeField] private GameObject aimLineRight;
            
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
            [Range(0f, 180f)]
            [SerializeField] private float rangedAimStartAngle;
            [SerializeField] private float rangedAimSpeed;
            [SerializeField] private float rangedKnockbackSpeed;
            [SerializeField] private float rangedKnockbackTime;
            [SerializeField] private Transform projectileSpawnPoint;
            [SerializeField] private float projectileSpeed;
            
            [Header("Audio")]
            [SerializeField] private GameObject playerObj;
            [SerializeField] private PlayerAudio playerAudio;
            [SerializeField] private DialogueAudio dialogueAudio;
            
            // Melee
            private List<IDamageable> damageableInRange;
            private float currentMeleeCooldown;
            private bool isMeleeAttacking;
            
            // Ranged
            private float currentRangedCooldown;
            private float currentAimAngle;
            public bool IsAiming { get; private set; }
            
            private PlayerController playerController;

#region Validation
            private void OnValidate()
            {
                if(meleeAttackCooldown < meleeAttackDelay + meleeAttackDuration)
                {
                    Debug.LogWarning("Melee Attack Cooldown needs to be higher than Melee Attack Delay and Melee Attack Duration combined");
                    meleeAttackCooldown = meleeAttackDelay + meleeAttackDuration + 0.01f;
                }
            }
#endregion

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
                
                if (IsAiming && currentAimAngle > 0)
                {
                    currentAimAngle -= Time.deltaTime * rangedAimSpeed;
                    currentAimAngle = Mathf.Max(0, currentAimAngle);
                    
                    Vector3 _leftPosition = Quaternion.AngleAxis(-currentAimAngle, Vector3.up) * new Vector3(0, 0, 1);
                    Quaternion _leftRotation = Quaternion.Euler(aimLineLeft.transform.localRotation.eulerAngles.x, -currentAimAngle, aimLineLeft.transform.localRotation.eulerAngles.z);
                    aimLineLeft.transform.localPosition = _leftPosition;
                    aimLineLeft.transform.localRotation = _leftRotation;
                    
                    Vector3 _rightPosition = Quaternion.AngleAxis(currentAimAngle, Vector3.up) * new Vector3(0, 0, 1);
                    Quaternion _rightRotation = Quaternion.Euler(aimLineLeft.transform.localRotation.eulerAngles.x, currentAimAngle, aimLineLeft.transform.localRotation.eulerAngles.z);
                    aimLineRight.transform.localPosition = _rightPosition;
                    aimLineRight.transform.localRotation = _rightRotation;
                }
            }
#endregion

#region Public Functions
            public void Melee()
            {
                if (playerController.PlayerData.currentItem != null || !playerController.PlayerData.hasMeleeWeapon || currentMeleeCooldown > 0)
                {
                    return;
                }
                
                currentMeleeCooldown = meleeAttackCooldown;
                
                playerController.PlayerAnimationBehaviour.PlayMeleeAttackAnimation(); 
                
                StartCoroutine(MeleeAttack());
            }

            public void Aim(bool _aiming)
            {
                if (playerController.PlayerData.currentItem != null || !playerController.PlayerData.hasRangedWeapon || currentRangedCooldown > 0)
                {
                    return;
                }
                
                if (_aiming)
                {
                    IsAiming = true;
                    currentAimAngle = rangedAimStartAngle;
                    
                    aimLineLeft.SetActive(true);
                    aimLineRight.SetActive(true);
                    
                    playerController.PlayerMovementBehaviour.TurnSpeed /= 2;
                    playerController.PlayerMovementBehaviour.SetMovementActiveState(false, true);
                }
                else if (IsAiming)
                {
                    IsAiming = false;
                    
                    aimLineLeft.SetActive(false);
                    aimLineRight.SetActive(false);
                    
                    currentRangedCooldown = rangedAttackCooldown;
                    
                    FireProjectile();
                    
                    playerController.PlayerMovementBehaviour.TurnSpeed *= 2;
                    playerController.PlayerMovementBehaviour.SetMovementActiveState(true, true);
                    playerController.PlayerMovementBehaviour.ApplyForce(rangedKnockbackSpeed, -transform.forward, rangedKnockbackTime, true);
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
            private IEnumerator MeleeAttack()
            {
                yield return new WaitForSeconds(meleeAttackDelay);
                
                playerController.PlayerMovementBehaviour.ApplyForce(meleeChargeSpeed, transform.forward, meleeChargeTime);
                
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
                        try
                        {
                            dialogueAudio.PlayerAttackAudio(playerController.PlayerIndex);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("[{PlayerAttackBehaviour}]: Error Exception " + e);
                        }
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

            private void FireProjectile()
            {
                Quaternion _launchRotation = Quaternion.AngleAxis(Random.Range(0f, currentAimAngle * (Random.Range(0, 2) * 2 - 1)), Vector3.up);
                
                GameObject _projectile = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.LookRotation(_launchRotation * transform.forward));
                _projectile.GetComponent<Projectile>().Setup(rangedAttackDamage, projectileSpeed, playerController.PlayerData);
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