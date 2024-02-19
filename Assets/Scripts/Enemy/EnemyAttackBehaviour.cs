// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-19
// Author: b22alesj
// Description: Controls enemy attack
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Core;
using UnityEngine;
using UnityEngine.AI;


namespace Game {
    namespace Enemy {
        public class EnemyAttackBehaviour : MonoBehaviour
        {
            [Header("Settings")]
            [SerializeField] private int damage;
            [SerializeField] private float attackCooldown;
            [SerializeField] private float attackDelay;
            
            private NavMeshAgent navMeshAgent;
            private EnemyAnimationBehaviour enemyAnimationBehaviour;
            private List<IDamageable> targetsInAttackRange;
            private float currentAttackCooldown;
            private float currentAttackDelay;
            private bool isAttacking;

#region Unity Functions
            private void FixedUpdate()
            {
                // Remove null references from attack range
                for (int i = targetsInAttackRange.Count - 1; i >= 0; i--)
                {
                    if (!(targetsInAttackRange[i] as Object))
                    {
                        targetsInAttackRange.Remove(targetsInAttackRange[i]);
                    }
                }
                
                // Decrease attack cooldown and initialize attack
                if (currentAttackCooldown > 0)
                {
                    currentAttackCooldown -= Time.fixedDeltaTime;
                }
                else if (targetsInAttackRange.Count > 0 && !isAttacking)
                {
                    enemyAnimationBehaviour.PlayAttackAnimation();
                    
                    navMeshAgent.isStopped = true;
                    
                    currentAttackDelay = attackDelay;
                    isAttacking = true;
                }
                
                // Decrease attack delay and attack
                if (currentAttackDelay > 0)
                {
                    currentAttackDelay -= Time.fixedDeltaTime;
                }
                else if (isAttacking)
                {
                    isAttacking = false;
                    currentAttackCooldown = attackCooldown;
                    
                    navMeshAgent.isStopped = false;
                    
                    // Attack everyone in attack range
                    for (int i = 0; i < targetsInAttackRange.Count; i++)
                    {
                        targetsInAttackRange[i].Damage(damage);
                    }
                }
            }
#endregion

#region Public Functions
            public void SetupBehaviour(NavMeshAgent _navMeshAgent, EnemyAnimationBehaviour _enemyAnimationBehaviour)
            {
                navMeshAgent = _navMeshAgent;
                enemyAnimationBehaviour = _enemyAnimationBehaviour;
                
                targetsInAttackRange = new List<IDamageable>();
            }

            public int GetTargetsInAttackRangeCount()
            {
                return targetsInAttackRange.Count;
            }

            public void AttackRangeEntered(Transform _targetTransform)
            {
                if (_targetTransform.gameObject.CompareTag("Player") || _targetTransform.gameObject.CompareTag("Carriage"))
                {
                    if (_targetTransform.TryGetComponent(out IDamageable hit))
                    {
                        targetsInAttackRange.Add(hit);
                    }
                }
            }

            public void AttackRangeExited(Transform _targetTransform)
            {
                if (_targetTransform.TryGetComponent(out IDamageable hit))
                {
                    targetsInAttackRange.Remove(hit);
                }
            }
#endregion
        }
    }
}
