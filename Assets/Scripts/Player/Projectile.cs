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

            private int _projectileDamage;

            private Vector3 mPrevPos;
#region Unity Functions
            // Start is called before the first frame update
            private void Awake()
            {
                rb = GetComponent<Rigidbody>();
            }

            void Start()
            {
                mPrevPos = transform.position;
            }

            
            // Update is called once per frame
            private void FixedUpdate()
            { 
                transform.Translate( direction* projectileSpeed * Time.fixedDeltaTime);

                RaycastHit[] hits = Physics.RaycastAll(new Ray(mPrevPos, (transform.position - mPrevPos).normalized),
                    (transform.position - mPrevPos).magnitude);
                for (int i = 0; i < hits.Length; i++)
                {
                    Debug.Log(hits[i].collider.gameObject.name);
                }

            }


            private void OnTriggerEnter(Collider other)
            {
                
                if (other.gameObject.layer== 8) 
                {
                    if (  other.gameObject.TryGetComponent(out IDamageable hit))
                    {
                        hit.Damage(_projectileDamage);
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

public void SetProjectileDamage(int _damage)
{
    _projectileDamage = _damage;
}
public void SetProjectileSpeed(float _speed)
{
    projectileSpeed = _speed;
}
#endregion

#region Private Functions

#endregion
        }
    }
}
