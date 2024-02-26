// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-26
// Author: b22alesj
// Description: Wave projectile script
// --------------------------------
// ------------------------------*/

using Game.Backend;
using Game.Core;
using UnityEngine;
using UnityEngine.Events;


namespace Game {
    namespace Player {
        public class WaveProjectile : MonoBehaviour
        {
            [Header("Settings")]
            [SerializeField] private float speed;
            [SerializeField] private float aliveTime;
            [SerializeField] private float aliveTimePerHit;
            
            private int damage;
            private PlayerData playerData;
            private UnityEvent onKill;
            private float currentAliveTime;

#region Unity Functions
            // Destroy bullet after specified time
            private void Update()
            {
                currentAliveTime += Time.deltaTime;
                
                if (currentAliveTime > aliveTime)
                {
                    Destroy(gameObject);
                }
            }

            private void OnCollisionEnter(Collision other)
            {
                if (other.gameObject.TryGetComponent(out IDamageable _hit))
                {
                    bool killed = _hit.Damage(damage);
                    
                    currentAliveTime -= aliveTimePerHit;
                    
                    if (killed)
                    {
                        playerData.kills += 1;
                        playerData.killsThisLevel += 1;
                        onKill.Invoke();
                        EnemyManager.OnEnemyDeathUI.Invoke();
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
#endregion

#region Public Functions
            public void Setup(int _damage, PlayerData _playerData, UnityEvent _onKill)
            {
                damage = _damage;
                playerData = _playerData;
                onKill = _onKill;
                
                GetComponent<Rigidbody>().velocity = transform.forward * speed;
            }
#endregion
        }
    }
}
