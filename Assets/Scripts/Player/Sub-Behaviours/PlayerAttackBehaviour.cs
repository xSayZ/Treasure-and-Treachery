// /*------------------------------
// --------------------------------
// Creation Date: 2024/01/30
// Author: Fredrik
// Description: This script is responsible for the attack behaviour of the player.
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Quest;
using UnityEngine;
using UnityEngine.Serialization;


namespace Game
{
    namespace Player
    {
        public class PlayerAttackBehaviour : MonoBehaviour {
            [Header("Component References")]
            /* DELETE THIS AFTER PLAYTEST 1 !!! */ [SerializeField] private PlayerMovementBehaviour playerMovementBehaviour; /* DELETE THIS AFTER PLAYTEST 1 !!! */ 
            [SerializeField] private CapsuleCollider weaponCollider;
            [SerializeField] private GameObject projectile;

            [Header("Melee Attack Settings")]
            [SerializeField] private bool hasMeleeWeapon;
            [SerializeField] private int meleeAttackDamage;
            [SerializeField] public float baseMeleeAttackCooldown;

            [Header("Ranged Attack Settings")]
            [SerializeField] private bool hasRangedWeapon;
            [SerializeField] private int rangedAttackDamage;
            [SerializeField] public float baseFireRateRanged;
            [SerializeField] private float projectileSpeed;
            
            private List<Transform> enemyTransforms = new List<Transform>();

            //private bool isAttacking;
            [HideInInspector] public float currentFireRate;
            [HideInInspector] public float currentMeleeCooldown;
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
            
            public void MeleeAttack()
            {
                if (currentMeleeCooldown > 0 || !hasMeleeWeapon)
                {
                    return;
                }
                
                for (int i = 0; i < enemyTransforms?.Count; i++) {
                    if (!enemyTransforms[i].TryGetComponent(out IDamageable _hit))
                        continue;
                    _hit.Damage(meleeAttackDamage);
                    enemyTransforms = enemyTransforms.Where(_collider => _collider != null).ToList();
                }
                currentMeleeCooldown = baseMeleeAttackCooldown;
            }

            public void RangedAttack()
            {
                if (!hasRangedWeapon)
                {
                    return;
                }
                
                GameObject _projectile = Instantiate(projectile, transform.position, Quaternion.identity);
                Projectile _playerProjectile = _projectile.GetComponent<Projectile>();
                _playerProjectile.SetValues(transform.forward, rangedAttackDamage, projectileSpeed);
                
                currentFireRate = baseFireRateRanged;
                playerMovementBehaviour.SetMovementActiveState(true, true);
            }
#endregion
            
#region Private Functions
            private void ActivateMeleeWeapon(int _playerIndex)
            {
                hasMeleeWeapon = true;
            }
            
            private void ActivateRangedWeapon(int _playerIndex)
            {
                hasRangedWeapon = true;
            }
#endregion
        }
    }
}