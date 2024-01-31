// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Abstract enemy state class, states inherit from this class
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Game {
    namespace Enemy {
        public abstract class EnemyState
        {
            [HideInInspector] public string Name = "NameNotSet";
            protected EnemyController enemyController;
            
#region State Machine Functions
            public void Awake(EnemyController e)
            {
                enemyController = e;
                SetUp();
            }
            
            protected virtual void SetUp(){}
            
            public virtual void Enter(){}
            
            public virtual void FixedUpdate(){}
            
            public virtual void Exit(){}
#endregion

#region Protected Functions
            protected Transform GetClosestTarget(List<Transform> _targets)
            {
                Transform _closestTarget = _targets[0];
                float _closestDistance = Vector3.Distance(enemyController.transform.position, _targets[0].position);
                             
                for (int i = 1; i < _targets.Count; i++)
                {
                    float _distance = Vector3.Distance(enemyController.transform.position, _targets[i].position);
                                 
                    if (_distance < _closestDistance)
                    {
                        _closestTarget = _targets[i];
                        _closestDistance = _distance;
                    }
                }
                             
                return _closestTarget;
            }
            
            protected void NavmeshUpdateCheck(Vector3 _lastTargetPosition)
            {
                // Update nav mesh agent destination if angle is to much
                Vector3 _directionToTarget = (enemyController.NavMeshAgent.destination - enemyController.transform.position).normalized;
                Vector3 _directionToLastSeenPosition = (_lastTargetPosition - enemyController.transform.position).normalized;
                if (Vector3.Angle(_directionToTarget, _directionToLastSeenPosition) > enemyController.MaxDeviationAngle)
                {
                    enemyController.NavMeshAgent.destination = _lastTargetPosition;
                    //Debug.Log("Updated nav mesh agent destination (angle)");
                }
                // Update nav mesh agent destination if player is further away and destination has almost been reached
                else if (enemyController.NavMeshAgent.remainingDistance < enemyController.UpdateDistance && (enemyController.NavMeshAgent.destination.x != _lastTargetPosition.x || enemyController.NavMeshAgent.destination.z != _lastTargetPosition.z) && enemyController.NavMeshAgent.hasPath)
                {
                    enemyController.NavMeshAgent.destination = _lastTargetPosition;
                    //Debug.Log("Updated nav mesh agent destination (distance)");
                }
            }
#endregion
        }
    }
}