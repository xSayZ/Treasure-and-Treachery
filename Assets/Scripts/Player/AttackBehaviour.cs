// /*------------------------------
// --------------------------------
// Creation Date: 2024/01/30
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Enemy;
using UnityEngine;
using UnityEngine.Serialization;


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
            [SerializeField] public int RangedAttackDamage;
            [SerializeField] public float BaseFireRateRanged;

             public List<Collider> enemyColliders = new List<Collider>();
             
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
                enemyColliders = enemyColliders.Where(item => item != null).ToList();
                currentFireRate -= Time.deltaTime;
                currentMeleeCooldown -= Time.deltaTime;
                if (currentMeleeCooldown <=0 || currentFireRate <= 0)
                {
                    isAttacking = false;
                }
            }
            private void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.layer == enemyLayer && !other.isTrigger)
                {
                    enemyColliders.Add(other);
                }
            }

            private void OnTriggerExit(Collider other)
            {
                if (other.gameObject.layer != enemyLayer)
                {
                    enemyColliders?.Remove(other);
                }
            }

            #endregion

           
            #region Public Functions

            public void MeleeAttack()
            {
                if (currentMeleeCooldown <= 0)
                {
                    isAttacking = true;
                    for (int i = 0; i < enemyColliders?.Count; i++)
                    {
                        if (enemyColliders[i].TryGetComponent(out IDamageable hit))
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
                    playerProjectile.SetDirection(transform.forward);

                    currentFireRate = BaseFireRateRanged;
                }
            }


            private void OnDrawGizmos()
            {
                Utility.Gizmos.GizmoSemiCircle.DrawWireArc(transform.position, transform.forward, 60, 45, 2);
            }

            #endregion

            #region Private Functions

            #endregion
        }
    }
}