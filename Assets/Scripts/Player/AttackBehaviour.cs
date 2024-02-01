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


namespace Game
{
    namespace Player
    {
        
        public class AttackBehaviour : MonoBehaviour
        {
            [Header("Component References")]
            
            public CapsuleCollider WeaponCollider;
            public GameObject projectile;
            [Header("Modifiables")]
            public int MeleeAttackDamage;
            public int ProjectileAttackDamage;
            public float BaseFireRate;
            
            [HideInInspector]
            public List<Collider> enemyColliders = new List<Collider>();


            private float currentFireRate;
            #region Unity Functions

            // Start is called before the first frame update

            private void Awake()
            {
                WeaponCollider.GetComponentInChildren<Collider>();
                currentFireRate = 0;
            }
            // Update is called once per frame
            void Update()
            {
                enemyColliders = enemyColliders.Where(item =>item != null).ToList();
                currentFireRate -= Time.deltaTime;
            }

            #endregion
            
            private void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.layer == 8 && !other.isTrigger)
                {
                    enemyColliders.Add(other);
                }
            }

            private void OnTriggerExit(Collider other)
            {
                if (!other.gameObject.CompareTag("Pickup"))
                {
                    enemyColliders?.Remove(other);
                }
            }

            #region Public Functions

            public void MeleeAttack()
            {
                for (int i = 0; i < enemyColliders?.Count; i++)
                {
                    if (enemyColliders[i].TryGetComponent(out IDamageable hit))
                    {
                        hit.Damage(MeleeAttackDamage);
                    }
                }
                
            }

            public void RangedAttack()
            {
                //TODO FixedRangeAttack
                if (currentFireRate <=0 )
                {
                    GameObject _projectile = Instantiate(projectile, transform.position, Quaternion.identity);

                    Projectile playerProjectile = _projectile.GetComponent<Projectile>();
                    playerProjectile.SetProjectileDamage(ProjectileAttackDamage);
                    playerProjectile.SetDirection(transform.forward);

                    currentFireRate = BaseFireRate;
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