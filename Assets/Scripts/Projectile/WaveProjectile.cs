// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-26
// Author: b22alesj
// Description: Wave projectile script
// --------------------------------
// ------------------------------*/

using System;
using Game.Audio;
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

            [Header("Audio")] 
            [SerializeField] private PlayerAudio playerAudio;
            
            private int damage;
            private PlayerData playerData;
            private UnityEvent<bool> onWaveKill;
            private float currentAliveTime;

#region Unity Functions

            private void Awake()
            {
                try
                {
                    playerAudio.SoulFireAudio(gameObject);
                }
                catch (Exception e)
                {
                    Debug.LogError("[{PlayerAttackBehaviour}]: Error Exception " + e);
                }
            }

// Destroy bullet after specified time
            private void Update()
            {
                currentAliveTime += Time.deltaTime;
                
                if (currentAliveTime > aliveTime)
                {
                    Destroy(gameObject);
                }
            }

            private void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.TryGetComponent(out IDamageable _hit))
                {
                    MonoBehaviour _hitMonoBehaviour = _hit as MonoBehaviour;
                    if (_hitMonoBehaviour)
                    {
                        if (!_hitMonoBehaviour.CompareTag("Enemy"))
                        {
                            return;
                        }
                    }
                    
                    bool killed = _hit.Damage(damage, transform.position, knockbackForce);
                    
                    currentAliveTime -= aliveTimePerHit;
                    
                    if (killed)
                    {
                        playerData.kills += 1;
                        playerData.killsThisLevel += 1;
                        onWaveKill.Invoke(false); // Doesn't actually check if enemy is stunned since gorgon doesn't have a ranged attack
                    }
                }
            }

            private void OnCollisionEnter(Collision other)
            {
                Destroy(gameObject);
            }
#endregion

#region Public Functions
            public void Setup(int _damage, PlayerData _playerData, UnityEvent<bool> _onWaveKill)
            {
                damage = _damage;
                playerData = _playerData;
                onWaveKill = _onWaveKill;
                
                GetComponent<Rigidbody>().velocity = transform.forward * speed;
            }
#endregion
        }
    }
}