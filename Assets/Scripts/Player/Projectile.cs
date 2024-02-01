// /*------------------------------
// --------------------------------
// Creation Date: 2024/01/30
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using Game.Core;
using UnityEngine;


namespace Game {
    namespace Player {
        public class Projectile : MonoBehaviour
        {
            public float projectileSpeed;
            public Vector3 direction;
            public Rigidbody rb;
            public float BulletAliveTime;
            public int ProjectileDamage;
#region Unity Functions
            // Start is called before the first frame update
            private void Awake()
            {
                rb = GetComponent<Rigidbody>();
            }

            void Start()
            {
                
            }
    
            // Update is called once per frame
            private void FixedUpdate()
            { 
                transform.Translate( direction* projectileSpeed * Time.fixedDeltaTime);
                

            }

            private void LateUpdate()
            {
              
            }

            private void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.layer== 8) 
                {
                    if (  other.gameObject.TryGetComponent(out IDamageable hit))
                    {
                        hit.Damage(ProjectileDamage);
                        Destroy(this.gameObject);
                    }

                }
                else
                {
                    Destroy(gameObject,BulletAliveTime);
                }
            }

            #endregion

#region Public Functions

public void SetDirection(Vector3 _direction)
{
    direction = _direction;
}
#endregion

#region Private Functions

#endregion
        }
    }
}
