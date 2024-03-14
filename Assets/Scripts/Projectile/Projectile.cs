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
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;


namespace Game {
    namespace Player {
        public class Projectile : MonoBehaviour
        {
            [Header("Settings")]
            [SerializeField] private float speed;
            [Range(0, 2500)]
            [SerializeField] private float knockbackForce;
            [SerializeField] private float aliveTime;
            [SerializeField] private GameObject impactVFX;
            
            [Header("Audio")]
            [SerializeField] private GameObject projectileObj;
            [SerializeField] private PlayerAudio playerAudio;
            
            [SerializeField] private CharacterType characterType;
            private enum CharacterType
            {
                Witch,
                Dragon
            }
            
            private int damage;
            private int maxHitAmount;
            private PlayerData playerData;
            private UnityEvent<bool> onKill;
            private float currentAliveTime;
            private float currentHitAmount;

#region Unity Functions
            private void Start()
            {
                try
                {
                    if (characterType == CharacterType.Witch)
                    {
                       playerAudio.PlayerRangedAudio(projectileObj); 
                    }
                    if (characterType == CharacterType.Dragon)
                    {
                        playerAudio.DragonArrowAudio(projectileObj);
                    }
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

            private void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.TryGetComponent(out IDamageable _hit))
                {
                    MonoBehaviour _hitMonoBehaviour = _hit as MonoBehaviour;
                    if (_hitMonoBehaviour)
                    {
                        if (_hitMonoBehaviour.CompareTag("Enemy"))
                        {
                            bool killed = _hit.Damage(damage, transform.position, knockbackForce);
                            currentHitAmount++;
                            PlayImpactVFX();
                            
                            if (killed)
                            {
                                playerData.kills += 1;
                                playerData.killsThisLevel += 1;
                                onKill.Invoke(false); // Doesn't actually check if enemy is stunned since gorgon doesn't have a ranged attack
                                
                                try
                                {
                                    if (characterType == CharacterType.Witch)
                                    {
                                        playerAudio.ProjectileHitAudio(gameObject, 0); 
                                    }
                                    if (characterType == CharacterType.Dragon)
                                    {
                                        playerAudio.ProjectileHitAudio(gameObject, 1);
                                    }
                                }
                                catch (Exception e)
                                {
                                    Debug.LogError("[{Projectile}]: Error Exception " + e);
                                }
                            }
                        }
                    }
                }
                
                if (currentHitAmount >= maxHitAmount)
                {
                    DestroyBullet();
                }
            }

            private void OnCollisionEnter(Collision other)
            {
                if (!other.collider.isTrigger)
                {
                    DestroyBullet();
                }
            }
#endregion

#region Public Functions
            private void DestroyBullet()
            {
                PlayImpactVFX();
                
                Destroy(gameObject);
            }

            private void PlayImpactVFX()
            {
                if (impactVFX)
                {
                    GameObject _spawnedImpactVFX = Instantiate(impactVFX, transform.position, quaternion.identity);
                    Destroy(_spawnedImpactVFX, 5);
                }
            }
#endregion

#region Public Functions
            public void Setup(int _damage, int _maxHitAmount, PlayerData _playerData, UnityEvent<bool> _onKill)
            {
                damage = _damage;
                maxHitAmount = _maxHitAmount;
                playerData = _playerData;
                onKill = _onKill;
                
                GetComponent<Rigidbody>().velocity = transform.forward * speed;
            }
#endregion
        }
    }
}