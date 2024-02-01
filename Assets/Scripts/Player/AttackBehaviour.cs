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
            [Header("References")] public CapsuleCollider WeaponCollider;
            public GameObject projectile;
            public int MeleeAttackDamage;
            private bool enemyInRange;
            public List<Collider> enemyColliders = new List<Collider>();

            #region Unity Functions

            // Start is called before the first frame update

            private void Awake()
            {
                WeaponCollider.GetComponentInChildren<Collider>();
            }
            // Update is called once per frame
            void Update()
            {
                enemyColliders = enemyColliders.Where(item =>item != null).ToList();

            }

            #endregion
            
            private void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.layer == 8 && !other.isTrigger)
                {
                    enemyInRange = true;
                    enemyColliders.Add(other);
                }
            }

            private void OnTriggerExit(Collider other)
            {
                if (!other.gameObject.CompareTag("Pickup"))
                {
                    enemyInRange = false;
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
                GameObject _projectile = Instantiate(projectile, transform.position, Quaternion.identity);

                Projectile playerProjectile = _projectile.GetComponent<Projectile>();
                playerProjectile.SetDirection(transform.forward);
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