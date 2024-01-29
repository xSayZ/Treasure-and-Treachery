// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Controls the movement of the player
// --------------------------------
// ------------------------------*/

using UnityEngine;
using UnityEngine.AI;


namespace Game {
    namespace Enemy {
        public class EnemyMovementBehaviour : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private NavMeshAgent navMeshAgent;
            
            [Header("Testing")]
            [SerializeField] private Transform goal;
            
#region Unity Functions
            // Start is called before the first frame update
            private void Start()
            {
                navMeshAgent.SetDestination(goal.position);
            }
    
            // Update is called once per frame
            private void Update()
            {
                
            }
#endregion

#region Public Functions

#endregion

#region Private Functions

#endregion
        }
    }
}
