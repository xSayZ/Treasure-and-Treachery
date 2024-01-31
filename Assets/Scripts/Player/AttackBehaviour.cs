// /*------------------------------
// --------------------------------
// Creation Date: 2024/01/30
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Player {
        public class AttackBehaviour : MonoBehaviour
        {
            public GameObject projectile;

#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
                
            }
#endregion

#region Public Functions

    public void RangedAttack(Vector3 direction)
    {
        Debug.Log(direction);
        projectile.GetComponent<Projectile>().SetDirection(direction);
        Instantiate(projectile, transform.position, Quaternion.identity);
       
    }
#endregion

#region Private Functions

#endregion
        }
    }
}
