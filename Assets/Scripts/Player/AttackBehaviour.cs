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
using UnityEngine;
using UnityEngine.Serialization;


namespace Game
{
    namespace Player
    {
        public class AttackBehaviour : MonoBehaviour
        {
            [Header("Component References")] 
            [SerializeField] private CapsuleCollider weaponCollider;
            [SerializeField] private GameObject projectile;

            [Header("Melee Attack Settings")] 
            [SerializeField] private int meleeAttackDamage;
            [SerializeField] private float baseMeleeAttackCooldown;

            [Header("Ranged Attack Settings")] 
            [SerializeField] private int rangedAttackDamage;
            [SerializeField] private float baseFireRateRanged;
            [SerializeField] private float projectileSpeed;
            
            private List<Transform> enemyTransforms = new List<Transform>();

            //private bool isAttacking;
            private float currentFireRate;
            private float currentMeleeCooldown;
            private LayerMask enemyLayer;
            
            #region Unity Functions
            private void Awake()
            {
                // Get references
                weaponCollider.GetComponentInChildren<Collider>();
                enemyLayer = LayerMask.NameToLayer("Enemy");
                
                // Set default values
                currentFireRate = 0;
                currentMeleeCooldown = 0;
            }

            // Update is called once per frame
            private void Update()
            {
                currentFireRate -= Time.deltaTime;
                currentMeleeCooldown -= Time.deltaTime;
                if (currentMeleeCooldown <=0 || currentFireRate <= 0)
                {
                    //isAttacking = false;
                }
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
            
            public void MeleeAttack() {
                if (!(currentMeleeCooldown <= 0))
                    return;
                
                for (int i = 0; i < enemyTransforms?.Count; i++) {
                    if (!enemyTransforms[i].TryGetComponent(out IDamageable _hit))
                        continue;
                    _hit.Damage(meleeAttackDamage);
                    enemyTransforms = enemyTransforms.Where(_collider => _collider != null).ToList();
                }
                currentMeleeCooldown = baseMeleeAttackCooldown;
            }

            public void RangedAttack() {
                if (!(currentFireRate <= 0))
                    return;
                
                GameObject _projectile = Instantiate(projectile, transform.position, Quaternion.identity);
                Projectile _playerProjectile = _projectile.GetComponent<Projectile>();
                _playerProjectile.SetValues(transform.forward, rangedAttackDamage, projectileSpeed);
                
                currentFireRate = baseFireRateRanged;
            }
#endregion
        }
    }
}