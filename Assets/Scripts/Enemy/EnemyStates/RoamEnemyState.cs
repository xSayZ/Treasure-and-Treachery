// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Enemy roam state
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


namespace Game {
    namespace Enemy {
        [System.Serializable]
        public class RoamEnemyState : EnemyState
        {
            [SerializeField] private float minRoamRange;
            [SerializeField] private float maxRoamRange;
            [Range(0, 180)]
            [SerializeField] private float minRoamAngle;
            [Range(0, 180)]
            [SerializeField] private float maxRoamAngle;
            
#region State Machine Functions
            protected override void SetUp()
            {
                Name = "Roam";
            }

            //public override void Enter(){}

            public override void FixedUpdate()
            {
                if (!enemyController.NavMeshAgent.hasPath)
                {
                    float _randomAngle = Random.Range(minRoamAngle, maxRoamAngle) * (Random.Range(0, 2) * 2 - 1);
                    Vector3 _randomDirection = Quaternion.AngleAxis(_randomAngle, Vector3.up) * enemyController.transform.forward;
                    Vector3 _randomPoint = enemyController.transform.position + _randomDirection * Random.Range(minRoamRange, maxRoamRange);
                    NavMeshHit _navHit;
                    NavMesh.SamplePosition(_randomPoint, out _navHit, float.MaxValue, -1);
                    enemyController.NavMeshAgent.destination = _navHit.position;
                }
                
                if (enemyController.targetsInVisionRange.Count + enemyController.targetsInHearingRange.Count > 0)
                {
                    enemyController.ChangeState(enemyController.AlertEnemyState);
                }
            }
            
            //public override void Exit(){}
 #endregion

 #region Public Functions
             public Tuple<float, float, float, float> GetRoamValues()
             {
                 return new Tuple<float, float, float, float>(minRoamRange, maxRoamRange, minRoamAngle, maxRoamAngle);
             }
 #endregion
        }
    }
}