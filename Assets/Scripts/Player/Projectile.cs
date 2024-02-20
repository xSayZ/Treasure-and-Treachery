// /*------------------------------
// --------------------------------
// Creation Date: 2024/01/30
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using Game.Core;
using Game.Audio;
using Game.Backend;
using UnityEngine;


namespace Game
{
    namespace Player
    {
        public class Projectile : MonoBehaviour
        {
            [Header("Settings")]
            [SerializeField] private float aliveTime;
            
            [Header("Audio")]
            [SerializeField] private GameObject projectileObj;
            [SerializeField] private PlayerAudio playerAudio;
            
            private int damage;
            private PlayerData playerData;
            private float currentAliveTime;

#region Unity Functions
            private void Start()
            {
                try
                {
                    playerAudio.PlayerRangedAudio(projectileObj);
                }
                catch (Exception e)
                {
                    Debug.LogError("[{Projectile}]: Error Exception " + e);
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

            private void OnCollisionEnter(Collision other)
            {
                if (other.gameObject.TryGetComponent(out IDamageable _hit))
                {
                    bool killed = _hit.Damage(damage);
                    
                    if (killed)
                    {
                        playerData.kills += 1;
                        playerData.killsThisLevel += 1;
                        EnemyManager.OnEnemyDeathUI.Invoke();
                        
                        try
                        {
                            playerAudio.ProjectileHitAudio(gameObject);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("[{Projectile}]: Error Exception " + e);
                        }
                    }
                }
                
                Destroy(gameObject);
            }
#endregion

#region Public Functions
            public void Setup(int _damage, float _speed, PlayerData _playerData)
            {
                damage = _damage;
                playerData = _playerData;
                
                GetComponent<Rigidbody>().velocity = transform.forward * _speed;
            }
#endregion
        }
    }
}