// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Abstract enemy state class, states inherit from this class
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;


namespace Game {
    namespace Enemy {
        public abstract class EnemyState
        {
            protected EnemyController enemyController;
            
#region Public Functions
            public void Awake(EnemyController e)
            {
                enemyController = e;
            }

            public virtual void Enter(){}
            
            public virtual void FixedUpdate(){}
#endregion


#region Protcted Functions
            protected List<Vector3> LookForTarget(Vector3 lookPosition, Vector3 lookDirection, float lookDistance, float lookFov, LayerMask playerLayerMask, LayerMask obstacleLayerMask)
            {
                List<Vector3> visibleTargetPositions = new List<Vector3>();
                Collider[] targetsInViewRadius = Physics.OverlapSphere(lookPosition, lookDistance, playerLayerMask);
                
                for (int i = 0; i < targetsInViewRadius.Length; i++)
                {
                    Vector3 directionToTarget = (targetsInViewRadius[i].transform.position - lookPosition).normalized;
                    if (Vector3.Angle(lookDirection, directionToTarget) < lookFov / 2)
                    {
                        float distanceToTarget = Vector3.Distance(lookPosition, targetsInViewRadius[i].transform.position);
                        if (!Physics.Raycast(lookPosition, directionToTarget, distanceToTarget, obstacleLayerMask))
                        {
                            visibleTargetPositions.Add(targetsInViewRadius[i].transform.position);
                        }
                    }
                }
                
                return visibleTargetPositions;
            }
#endregion
        }
    }
}