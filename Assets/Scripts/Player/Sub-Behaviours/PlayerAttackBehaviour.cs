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
using Game.Enemy;
using Game.Quest;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using WaitForSeconds = UnityEngine.WaitForSeconds;


namespace Game {
    namespace Player {
        public class PlayerAttackBehaviour : MonoBehaviour {
            private enum AttackTypes
            {
                Melee,
                Ranged
            }
            
            [Header("Component References")]
            [Tooltip("Assign the weapon collider here.")]
            [SerializeField] private CapsuleCollider weaponCollider;
            [Tooltip("Assign the projectiles here.")]
            [SerializeField] private GameObject normalProjectile;
            [SerializeField] private GameObject waveProjectile;
            [SerializeField] private GameObject aimLineLeft;
            [SerializeField] private GameObject aimLineRight;
            
            [Header("Attack Type")]
            [SerializeField] private AttackTypes attackType;
            
            [Header("Melee Attack Settings")]
            [Tooltip("The damage the melee attack does.")]
            [SerializeField] private int meleeAttackDamage;
            [Tooltip("The cooldown of the melee attack.")]
            [SerializeField] private float meleeAttackCooldown;
            [Tooltip("The duration of the melee attack.")]
            [SerializeField] private float meleeAttackDuration;
            [Tooltip("The delay of the melee attack. The delay before the attack starts.")]
            [SerializeField] private float meleeAttackDelay;
            [Tooltip("The speed of the charge that occurs during the melee attack.")]
            [SerializeField] private float meleeChargeSpeed;
            [Tooltip("The time the charge lasts.")]
            [SerializeField] private float meleeChargeTime;
            [SerializeField] private float meleeStunTime;
            [Range(0, 2500)]
            [Tooltip("The force of the knockback of the melee attack.")]
            [SerializeField] private float meleeKnockbackForce;
            
            [Header("Ranged Attack Settings")]
            [SerializeField] private int rangedAttackDamage;
            [SerializeField] private float rangedAttackCooldown;
            [Range(0f, 180f)]
            [SerializeField] private float rangedAimMinAngle;
            [Range(0f, 180f)]
            [SerializeField] private float rangedAimMaxAngle;
            [SerializeField] private bool rangedAimShrink;
            [SerializeField] private float rangedAimSpeed;
            [SerializeField] private float rangedKnockbackSpeed;
            [SerializeField] private float rangedKnockbackTime;
            [SerializeField] private Transform projectileSpawnPoint;
            [SerializeField] private int rangedWaveHealthCost;
            
            [Header("Audio")]
            [SerializeField] private GameObject playerObj;
            [SerializeField] private PlayerAudio playerAudio;
            [SerializeField] private DialogueAudio dialogueAudio;
            
            // Melee
            private List<IDamageable> damageableInRange;
            private float currentMeleeCooldown;
            private bool isMeleeAttacking;
            private bool meleeAttackStarted;
            [HideInInspector] public float MeleeAttackCooldownMultiplier = 1f;
            [HideInInspector] public bool MeleeIsStunAttack;
            
            // Ranged
            private float currentRangedCooldown;
            private float currentAimAngle;
            public bool IsAiming { get; private set; }
            
            private PlayerController playerController;
            private bool canAttack = true;
            
            // Events
            [HideInInspector] public UnityEvent OnKill = new UnityEvent();
            [HideInInspector] public UnityEvent OnWaveKill = new UnityEvent();

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
                
                if (IsAiming)
                {
                    if (rangedAimShrink)
                    {
                        currentAimAngle -= Time.deltaTime * rangedAimSpeed;
                    }
                    else
                    {
                        currentAimAngle += Time.deltaTime * rangedAimSpeed;
                    }
                    
                    currentAimAngle = Mathf.Clamp(currentAimAngle, rangedAimMinAngle, rangedAimMaxAngle);
                    
                    Vector3 _leftPosition = Quaternion.AngleAxis(-currentAimAngle, Vector3.up) * new Vector3(0, 0, 1.6f);
                    Quaternion _leftRotation = Quaternion.Euler(aimLineLeft.transform.localRotation.eulerAngles.x, -currentAimAngle, aimLineLeft.transform.localRotation.eulerAngles.z);
                    aimLineLeft.transform.localPosition = _leftPosition;
                    aimLineLeft.transform.localRotation = _leftRotation;
                    
                    Vector3 _rightPosition = Quaternion.AngleAxis(currentAimAngle, Vector3.up) * new Vector3(0, 0, 1.6f);
                    Quaternion _rightRotation = Quaternion.Euler(aimLineLeft.transform.localRotation.eulerAngles.x, currentAimAngle, aimLineLeft.transform.localRotation.eulerAngles.z);
                    aimLineRight.transform.localPosition = _rightPosition;
                    aimLineRight.transform.localRotation = _rightRotation;
                }
            }
#endregion

#region Public Functions
            public void Attack(bool _started)
            {
                if (attackType == AttackTypes.Melee)
                {
                    if (_started)
                    {
                        Melee();
                    }
                }
                else if (attackType == AttackTypes.Ranged)
                {
                    Aim(_started);
                }
            }

            public void SetAttackActiveState(bool _active)
            {
                canAttack = _active;
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
                            MeleeDamage(_hit);
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
            private void Melee()
            {
                if (playerController.PlayerData.currentItem != null || !playerController.PlayerData.hasMeleeWeapon || currentMeleeCooldown > 0 || !canAttack || meleeAttackStarted)
                {
                    return;
                }
                            
                meleeAttackStarted = true;
                            
                playerController.PlayerAnimationBehaviour.PlayMeleeAttackAnimation(); 
                            
                StartCoroutine(MeleeAttack());
            }

            private void Aim(bool _aiming)
            {
                if (playerController.PlayerData.currentItem != null || !playerController.PlayerData.hasRangedWeapon || currentRangedCooldown > 0 || !canAttack)
                {
                    return;
                }
                            
                if (_aiming)
                {
                    IsAiming = true;
                                
                    if (rangedAimShrink)
                    {
                        currentAimAngle = rangedAimMaxAngle;
                    }
                    else
                    {
                        currentAimAngle = rangedAimMinAngle;
                    }
                                
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
                    
                    MeleeDamage(damageableInRange[i]);
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
                
                currentMeleeCooldown = meleeAttackCooldown * MeleeAttackCooldownMultiplier;
                meleeAttackStarted = false;
                
                playerController.PlayerMovementBehaviour.SetMovementActiveState(false, false);
                yield return new WaitForSeconds(meleeStunTime);
                playerController.PlayerMovementBehaviour.SetMovementActiveState(true, true);
            }

            private void MeleeDamage(IDamageable _damageable)
            {
                bool _doNormalMelee = true;
                
                // Stun melee
                if (MeleeIsStunAttack)
                {
                    MonoBehaviour _damageableMonoBehaviour = _damageable as MonoBehaviour;
                    if (!_damageableMonoBehaviour)
                    {
                        return; 
                    }
                    
                    if (_damageableMonoBehaviour.TryGetComponent(out EnemyController _enemyController))
                    {
                        if (_enemyController.GetCurrentState() != _enemyController.StunnedEnemyState)
                        {
                            _doNormalMelee = false;
                            _enemyController.ChangeState(_enemyController.StunnedEnemyState);
                        }
                    }
                }
                
                // Normal melee
                if (_doNormalMelee)
                {
                    bool killed = _damageable.Damage(meleeAttackDamage, transform.position, meleeKnockbackForce);
                    
                    if (killed)
                    {
                        playerController.PlayerData.kills += 1;
                        playerController.PlayerData.killsThisLevel += 1;
                        OnKill.Invoke();
                        
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
            }

            private void FireProjectile()
            {
                if (rangedAimShrink)
                {
                    Quaternion _launchRotation = Quaternion.AngleAxis(Random.Range(0f, currentAimAngle * (Random.Range(0, 2) * 2 - 1)), Vector3.up);
                    
                    GameObject _projectile = Instantiate(normalProjectile, projectileSpawnPoint.position, Quaternion.LookRotation(_launchRotation * transform.forward));
                    _projectile.GetComponent<Projectile>().Setup(rangedAttackDamage, playerController.PlayerData, OnKill);
                }
                else
                {
                    if (currentAimAngle < rangedAimMaxAngle || playerController.Health <= rangedWaveHealthCost)
                    {
                        GameObject _projectile = Instantiate(normalProjectile, projectileSpawnPoint.position, Quaternion.LookRotation(Quaternion.identity * transform.forward));
                        _projectile.GetComponent<Projectile>().Setup(rangedAttackDamage, playerController.PlayerData, OnKill);
                    }
                    else
                    {
                        GameObject _projectile = Instantiate(waveProjectile, projectileSpawnPoint.position, Quaternion.LookRotation(Quaternion.identity * transform.forward));
                        _projectile.GetComponent<WaveProjectile>().Setup(rangedAttackDamage, playerController.PlayerData, OnWaveKill);
                        (playerController as IDamageable).Damage(rangedWaveHealthCost, new Vector3(), 0);
                    }
                }
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