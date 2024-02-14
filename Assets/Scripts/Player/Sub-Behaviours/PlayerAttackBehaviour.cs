// /*------------------------------
// --------------------------------
// Creation Date: 2024/01/30
// Author: Fredrik
// Description: This script is responsible for the attack behaviour of the player.
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Backend;
using Game.Core;
using Game.Quest;
using UnityEngine;


namespace Game
{
    namespace Player
    {
        public class PlayerAttackBehaviour : MonoBehaviour {
            [Header("Component References")]
            /* DELETE THIS AFTER PLAYTEST 1 !!! */ [SerializeField] private PlayerMovementBehaviour playerMovementBehaviour; /* DELETE THIS AFTER PLAYTEST 1 !!! */
            /* DELETE THIS AFTER PLAYTEST 1 !!! */ [SerializeField] private PlayerAnimationBehaviour playerAnimationBehaviour; /* DELETE THIS AFTER PLAYTEST 1 !!! */ 
            [SerializeField] private CapsuleCollider weaponCollider;
            [SerializeField] private GameObject projectile;

            [Header("Melee Attack Settings")]
            [SerializeField] private int meleeAttackDamage;
            [SerializeField] public float baseMeleeAttackCooldown;

            [Header("Ranged Attack Settings")]
            [SerializeField] private int rangedAttackDamage;
            [SerializeField] public float baseFireRateRanged;
            [SerializeField] private float projectileSpeed;
            
            private List<Transform> enemyTransforms = new List<Transform>();

            //private bool isAttacking;
            [HideInInspector] public float currentFireRate;
            [HideInInspector] public float currentMeleeCooldown;
            private PlayerController playerController;
            private LayerMask enemyLayer;
            
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
                playerController = GetComponent<PlayerController>();
                enemyLayer = LayerMask.NameToLayer("Enemy");
                
                // Set default values
                currentFireRate = 0;
                currentMeleeCooldown = 0;
            }
            
            private void Update()
            {
                currentFireRate -= Time.deltaTime;
                currentMeleeCooldown -= Time.deltaTime;
            }
#endregion
            
#region Public Functions
            public void OnAttackRangeEnter(Transform _transform) {
                if (_transform.gameObject.layer != enemyLayer)
                    return;
                
                if (weaponCollider.isTrigger)
                {
                    enemyTransforms.Add(_transform);
                }
            }
            
            public void OnAttackRangeExit(Transform _transform)
            {
                if (_transform.gameObject.layer != enemyLayer)
                {
                    enemyTransforms?.Remove(_transform);
                }
            }
            
            public bool MeleeAttack()
            {
                if (currentMeleeCooldown > 0 || !playerController.PlayerData.hasMeleeWeapon)
                {
                    return false;
                }
                
                for (int i = enemyTransforms.Count - 1; i >= 0; i--)
                {
                    if (enemyTransforms[i] == null)
                    {
                        enemyTransforms.Remove(enemyTransforms[i]);
                        continue;
                    }
                    
                    if (!enemyTransforms[i].TryGetComponent(out IDamageable _hit))
                        continue;
                    
                    bool killed = _hit.Damage(meleeAttackDamage);
                    if (killed)
                    {
                        playerController.PlayerData.kills += 1;
                        playerController.PlayerData.killsThisLevel += 1;
                        EnemyManager.OnEnemyDeathUI.Invoke();
                    }
                }
                currentMeleeCooldown = baseMeleeAttackCooldown;
                return true;
            }

            public void RangedAttack()
            {
                if (!playerController.PlayerData.hasRangedWeapon)
                {
                    return;
                }
                
                GameObject _projectile = Instantiate(projectile, transform.position, Quaternion.identity);
                Projectile _playerProjectile = _projectile.GetComponent<Projectile>();
                _playerProjectile.SetValues(transform.forward, rangedAttackDamage, projectileSpeed, playerController.PlayerData);
                
                currentFireRate = baseFireRateRanged;
                playerMovementBehaviour.SetMovementActiveState(true, true);
            }
#endregion
            
#region Private Functions
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