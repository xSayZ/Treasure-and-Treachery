// /*------------------------------
// --------------------------------
// Creation Date: 2024/01/30
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using Game.Enemy;
using UnityEngine;


namespace Game {
    namespace Player {
        public class AttackBehaviour : MonoBehaviour
        {
            
            [Header("References")]
            public CapsuleCollider WeaponCollider;
            public GameObject projectile;
            
            private bool enemyInRange;
            private List<Collider> enemyColliders = new List<Collider>();

            #region Unity Functions
            // Start is called before the first frame update

            private void Awake()
            {
                WeaponCollider.GetComponentInChildren<Collider>();
            }

            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
               
            }
#endregion


private void OnTriggerEnter(Collider other)
{

    if (other.gameObject.layer == 8)
    {
        Debug.Log("enter");
        enemyInRange = true;
        enemyColliders.Add(other);
        
    }
}

private void OnTriggerExit(Collider other)
{
    if (!other.gameObject.CompareTag("Pickup")) 
    {
        Debug.Log("exit");
        enemyInRange = false;
        enemyColliders?.Remove(other);
    }
  
}

#region Public Functions


    public void MeleeAttack()
    {
        if (enemyInRange)
        {
            for (int i = 0; i < enemyColliders?.Count; i++)
            {
                //TODO:: Damage EnemyHealth;
            }
        }
    }

    public void RangedAttack()
    {
        //TODO FixedRangeAttack
        GameObject _projectile = Instantiate(projectile, transform.position, Quaternion.identity);

        Projectile playerProjectile = _projectile.GetComponent<Projectile>();
        playerProjectile.SetDirection(transform.forward);

    }


    private void OnDrawGizmos()
    {
        Utility.Gizmos.GizmoSemiCircle.DrawWireArc(transform.position,transform.forward,60,45,2);
        
    }

    #endregion

#region Private Functions

#endregion
        }
    }
}
