// /*------------------------------
// --------------------------------
// Creation Date: 2024/01/30
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;


namespace Game {
    namespace Player {
        public class Projectile : MonoBehaviour
        {
            public float projectileSpeed;
            public Vector3 direction;
            public Rigidbody rb;
            private Vector3 currentBulletPosition;
            private Vector3 oldBulletPosition;
#region Unity Functions
            // Start is called before the first frame update
            private void Awake()
            {
                rb = GetComponent<Rigidbody>();
            }

            void Start()
            {
                
                Debug.Log(direction);
                
            }
    
            // Update is called once per frame
            private void FixedUpdate()
            { 
                transform.Translate( direction* projectileSpeed * Time.fixedDeltaTime);
                

            }

            private void LateUpdate()
            {
                oldBulletPosition = transform.position;
              
            }

            private void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.layer== 8) 
                {
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(gameObject,5);
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
