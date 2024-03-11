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
            [Range(0, 2500)]
            [SerializeField] private float knockbackForce;
            [SerializeField] private float aliveTime;
            [SerializeField] private float aliveTimePerHit;
            
            private int damage;
            private PlayerData playerData;
            private UnityEvent onWaveKill;
            private float currentAliveTime;
            private Rigidbody rigidbody;

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
                rigidbody.velocity = transform.forward * speed;
                
                if (other.gameObject.TryGetComponent(out IDamageable _hit))
                {
                    MonoBehaviour _hitMonoBehaviour = _hit as MonoBehaviour;
                    if (_hitMonoBehaviour)
                    {
                        if (!_hitMonoBehaviour.CompareTag("Enemy"))
                        {
                            Destroy(gameObject);
                        }
                    }
                    
                    bool killed = _hit.Damage(damage, transform.position, knockbackForce);
                    
                    currentAliveTime -= aliveTimePerHit;
                    
                    if (killed)
                    {
                        playerData.kills += 1;
                        playerData.killsThisLevel += 1;
                        onWaveKill.Invoke();
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
#endregion

#region Public Functions
            public void Setup(int _damage, PlayerData _playerData, UnityEvent _onWaveKill)
            {
                damage = _damage;
                playerData = _playerData;
                onWaveKill = _onWaveKill;
                
                rigidbody = GetComponent<Rigidbody>();
                rigidbody.velocity = transform.forward * speed;
            }
#endregion
        }
    }
}