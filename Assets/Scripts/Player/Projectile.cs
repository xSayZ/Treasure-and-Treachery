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
            [SerializeField] private float bulletAliveTime;
            
            [Header("Audio")]
            [SerializeField] private GameObject projectileObj;
            [SerializeField] private PlayerAudio playerAudio;
            
            // Internal Variables
            private Vector3 direction;
            private float projectileSpeed;
            private int projectileDamage;
            private PlayerData playerData;
            
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
            
            private void FixedUpdate()
            {
                transform.Translate(direction * (projectileSpeed * Time.fixedDeltaTime));
            }
            
            private void OnCollisionEnter(Collision other)
            {
                if (other.gameObject.layer == 8)
                {
                    if (!other.gameObject.TryGetComponent(out IDamageable _hit))
                        return;
                    
                    bool killed = _hit.Damage(projectileDamage);
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
                    
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(gameObject, bulletAliveTime);
                }
            }
#endregion

#region Public Functions
            public void SetValues(Vector3 _direction, int _damage, float _projectileSpeed, PlayerData _playerData)
            {
                direction = _direction;
                projectileDamage = _damage;
                projectileSpeed = _projectileSpeed;
                playerData = _playerData;
            }
#endregion
        }
    }
}