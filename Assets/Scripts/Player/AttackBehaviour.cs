// /*------------------------------
// --------------------------------
// Creation Date: 2024/01/30
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using System.Linq;
using Game.Core;
using UnityEngine;


namespace Game
{
    namespace Player
    {
        public class AttackBehaviour : MonoBehaviour
        {
            [Header("Component References")] public CapsuleCollider WeaponCollider;
            public GameObject projectile;

            [Header("Melee Attack Settings")] 
            [SerializeField]public int MeleeAttackDamage;
            [SerializeField] public float BaseMeleeAttackCooldown;

            [Header("Ranged Attack Settings")] 
            [SerializeField] private int RangedAttackDamage;
            [SerializeField] private float BaseFireRateRanged;
            [SerializeField] private float projectileSpeed;
             public List<Transform> enemyTransforms = new List<Transform>();
             
            public bool isAttacking { get; private set; }
            private float currentFireRate;
            private float currentMeleeCooldown;
            private LayerMask enemyLayer;
            
            #region Unity Functions

            // Start is called before the first frame update

            private void Awake()
            {
                WeaponCollider.GetComponentInChildren<Collider>();
                currentFireRate = 0;
                currentMeleeCooldown = 0;
                enemyLayer = LayerMask.NameToLayer("Enemy");
            }

            // Update is called once per frame
            void Update()
            {
                enemyTransforms = enemyTransforms.Where(Collider => Collider != null).ToList();
                currentFireRate -= Time.deltaTime;
                currentMeleeCooldown -= Time.deltaTime;
                if (currentMeleeCooldown <=0 || currentFireRate <= 0)
                {
                    isAttacking = false;
                }
            }
            

            #endregion

           
            #region Public Functions

            public void OnAttackRangeEnter(Transform _transform)
            {
                if (_transform.gameObject.layer == enemyLayer)
                {
                    if (WeaponCollider.isTrigger)
                    {
                        enemyTransforms.Add(_transform);
                    }
                  
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
                if (currentMeleeCooldown <= 0)
                {
                    isAttacking = true;
                    for (int i = 0; i < enemyTransforms?.Count; i++)
                    {
                        if (enemyTransforms[i].TryGetComponent(out IDamageable hit))
                        {
                            hit.Damage(MeleeAttackDamage);
                        }
                    }

                    currentMeleeCooldown = BaseMeleeAttackCooldown;
                }
            }

            public void RangedAttack()
            {
                //TODO FixedRangeAttack
                if (currentFireRate <= 0)
                {
                   
                    GameObject _projectile = Instantiate(projectile, transform.position, Quaternion.identity);

                    Projectile playerProjectile = _projectile.GetComponent<Projectile>();
                    playerProjectile.SetProjectileDamage(RangedAttackDamage);
                    playerProjectile.SetProjectileSpeed(projectileSpeed);
                    playerProjectile.SetDirection(transform.forward);

                    currentFireRate = BaseFireRateRanged;
                }
            }
            #endregion

            #region Private Functions

            #endregion
        }
    }
}