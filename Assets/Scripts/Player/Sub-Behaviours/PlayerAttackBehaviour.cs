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
using System.Linq;
using FMOD.Studio;
using Game.Audio;
using Game.Core;
using Game.Enemy;
using Game.Quest;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
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
            [SerializeField] private VisualEffect[] meleeVisualEffects;
            [SerializeField] private GameObject meleeImpactVFX;
            
            [Header("Attack Type")]
            [SerializeField] private AttackTypes attackType;
            
            [Header("Melee Attack Settings")]
            [Tooltip("The damage the melee attack does.")]
            [SerializeField] private int meleeAttackDamage;
            [Tooltip("The max number of targets that can be damaged in one melee attack.")]
            [SerializeField] private int meleeMaxAttackTargets;
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
            [SerializeField] private float rangedTurnSpeed = 0.5f;
            [SerializeField] private float rangedKnockbackSpeed;
            [SerializeField] private float rangedKnockbackTime;
            [SerializeField] private Transform projectileSpawnPoint;
            [SerializeField] private int rangedWaveHealthCost;
            
            [Header("Audio")]
            [SerializeField] private GameObject playerObj;
            [SerializeField] private PlayerAudio playerAudio;
            [SerializeField] private DialogueAudio dialogueAudio;
            private EventInstance dragonShootinstance;
            
            // Melee
            private List<IDamageable> damageableInRange;
            private float currentMeleeCooldown;
            private bool isMeleeAttacking;
            private bool meleeAttackStarted;
            private int currentMaxMeleeTargets;
            private int currentMeleeTargets;
            [HideInInspector] public float MeleeAttackCooldownMultiplier = 1f;
            [HideInInspector] public bool MeleeIsStunAttack;
            
            // Ranged
            private float currentRangedCooldown;
            private float currentAimAngle;
            private float turnSpeed;
            public bool IsAiming { get; private set; }
            
            private PlayerController playerController;
            private bool canAttack = true;
            
            // Events
            [HideInInspector] public UnityEvent<bool> OnKill = new UnityEvent<bool>();
            [HideInInspector] public UnityEvent<bool> OnWaveKill = new UnityEvent<bool>();

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

            public void SetupBehaviour(PlayerController _playerController, float _turnSpeed) {
                damageableInRange = new List<IDamageable>();
                currentMaxMeleeTargets = meleeMaxAttackTargets;
                
                playerController = _playerController;
                turnSpeed = _turnSpeed;
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
                    playerController.PlayerAnimationBehaviour.UpdateAttackChargeAnimation(currentAimAngle);
                    
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
                
                playerController.PlayerAnimationBehaviour.PlayAttackAnimation(); 
                
                StartCoroutine(MeleeAttack());
            }

            private void Aim(bool _aiming) {
                
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
                                
                    playerController.PlayerMovementBehaviour.CurrentTurnSpeed = rangedTurnSpeed;
                    playerController.PlayerMovementBehaviour.AimMoveLock = true;
                    
                    try
                    {
                        playerAudio.DragonShoot(playerObj, true, dragonShootinstance);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("[{PlayerAttackBehaviour}]: Error Exception " + e);
                    }
                }
                else if (IsAiming)
                {
                    IsAiming = false;
                                
                    aimLineLeft.SetActive(false);
                    aimLineRight.SetActive(false);
                                
                    currentRangedCooldown = rangedAttackCooldown;
                                
                    FireProjectile();
                    playerController.PlayerAnimationBehaviour.PlayAttackAnimation();
                    playerController.PlayerAnimationBehaviour.UpdateAttackChargeAnimation(0);
                    
                    playerController.PlayerMovementBehaviour.CurrentTurnSpeed = turnSpeed;
                    playerController.PlayerMovementBehaviour.AimMoveLock = false;
                    playerController.PlayerMovementBehaviour.ApplyForce(rangedKnockbackSpeed, -transform.forward, rangedKnockbackTime, true);

                    try
                    {
                        playerAudio.DragonShoot(playerObj, false, dragonShootinstance);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("[{PlayerAttackBehaviour}]: Error Exception " + e);
                    }
                }
            }

            private IEnumerator MeleeAttack()
            {
                currentMeleeTargets = 0;
                
                yield return new WaitForSeconds(meleeAttackDelay);
                
                // Play melee attack vfx
                foreach (VisualEffect _meleeVisualEffect in meleeVisualEffects)
                {
                    _meleeVisualEffect.Play();
                }
                
                // Apply charge force
                playerController.PlayerMovementBehaviour.ApplyForce(meleeChargeSpeed, transform.forward, meleeChargeTime);
                
                // Get all enemies in range as mono behaviours
                List<MonoBehaviour> _damageableMonoBehaviourInRange = new List<MonoBehaviour>();
                for (int i = damageableInRange.Count - 1; i >= 0; i--)
                {
                    MonoBehaviour _damageableMonoBehaviour = damageableInRange[i] as MonoBehaviour;
                    if (_damageableMonoBehaviour)
                    {
                        _damageableMonoBehaviourInRange.Add(_damageableMonoBehaviour);
                    }
                    else
                    {
                        damageableInRange.Remove(damageableInRange[i]);
                    }
                }
                
                // Sort mono behaviours after distance from player
                _damageableMonoBehaviourInRange = _damageableMonoBehaviourInRange.OrderBy(monoBehaviour => Vector3.Distance(transform.position, monoBehaviour.transform.position)).ToList();
                
                // Attack enemies
                foreach (MonoBehaviour _damageableMonoBehaviour in _damageableMonoBehaviourInRange)
                {
                    MeleeDamage(_damageableMonoBehaviour.GetComponent<IDamageable>());
                }
                
                // Play attack audio
                try
                {
                    playerAudio.MeleeAudioPlay(playerObj);
                }
                catch (Exception e)
                {
                    Debug.LogError("[{PlayerController}]: Error Exception " + e);
                }
                
                // Run melee attack for a while longer
                isMeleeAttacking = true;
                yield return new WaitForSeconds(meleeAttackDuration);
                isMeleeAttacking = false;
                
                // Reset melee attack
                currentMeleeCooldown = meleeAttackCooldown * MeleeAttackCooldownMultiplier;
                meleeAttackStarted = false;
                
                // Wait longer if attack duration is les than charge time
                if (meleeChargeTime > meleeAttackDuration)
                {
                    yield return new WaitForSeconds(meleeChargeTime - meleeAttackDuration);
                }
                
                // Melee stun
                playerController.PlayerMovementBehaviour.AttackStunMoveRotateLock = true;
                yield return new WaitForSeconds(meleeStunTime);
                playerController.PlayerMovementBehaviour.AttackStunMoveRotateLock = false;
            }

            private void MeleeDamage(IDamageable _damageable)
            {
                if (currentMeleeTargets >= currentMaxMeleeTargets)
                {
                    return;
                }
                
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
                            currentMeleeTargets++;
                        }
                    }
                }
                
                // Normal melee
                if (_doNormalMelee)
                {
                    bool killed = _damageable.Damage(meleeAttackDamage, transform.position, meleeKnockbackForce);
                    currentMeleeTargets++;
                    
                    if (meleeImpactVFX)
                    {
                        MonoBehaviour _damageableMonoBehaviour = _damageable as MonoBehaviour;
                        if (_damageableMonoBehaviour)
                        {
                            GameObject _spawnedImpactVFX = Instantiate(meleeImpactVFX, _damageableMonoBehaviour.transform.position, Quaternion.identity);
                            Destroy(_spawnedImpactVFX, 5);
                        }
                    }
                    
                    if (killed)
                    {
                        // Update player data
                        playerController.PlayerData.kills += 1;
                        playerController.PlayerData.killsThisLevel += 1;
                        
                        // Check if stun kill
                        bool _stunKill = false;
                        MonoBehaviour _damageableMonoBehaviour = _damageable as MonoBehaviour;
                        if (_damageableMonoBehaviour)
                        {
                            if (_damageableMonoBehaviour.TryGetComponent(out EnemyController _enemyController))
                            {
                                if (_enemyController.GetCurrentState() == _enemyController.StunnedEnemyState)
                                {
                                    _stunKill = true;
                                }
                            }
                        }
                        
                        OnKill.Invoke(_stunKill);
                        
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