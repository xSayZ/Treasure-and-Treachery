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
            public string Name = "NameNotSet";
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
#endregion
        }
    }
}