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
using UnityEngine;
using UnityEngine.Serialization;


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

            // References
            private Rigidbody rb;
            
            #region Unity Functions
            private void Awake() {
                rb = GetComponent<Rigidbody>();
            }
            void Start() {
                playerAudio.PlayerRangedAudio(projectileObj);
            }
            private void FixedUpdate() {
                transform.Translate(direction * (projectileSpeed * Time.fixedDeltaTime));
            }
            private void OnTriggerEnter(Collider other) {
                if (other.gameObject.layer == 8) {
                    if (!other.gameObject.TryGetComponent(out IDamageable _hit))
                        return;
                    
                    _hit.Damage(projectileDamage);
                    Destroy(this.gameObject);
                } else {
                    Destroy(gameObject, bulletAliveTime);
                }
            }

            #endregion

            #region Public Functions
            public void SetValues(Vector3 _direction, int _damage, float _projectileSpeed) {
                direction = _direction;
                projectileDamage = _damage;
                projectileSpeed = _projectileSpeed;
            }
            #endregion
        }
    }
}