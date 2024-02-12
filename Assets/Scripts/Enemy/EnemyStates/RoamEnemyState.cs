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
using Game.Audio;

namespace Game {
    namespace Enemy {
        [System.Serializable]
        public class RoamEnemyState : EnemyState
        {
            [SerializeField] private float moveSpeed;
            [SerializeField] private float minRoamRange;
            [SerializeField] private float maxRoamRange;
            [Range(0, 180)]
            [SerializeField] private float minRoamAngle;
            [Range(0, 180)]
            [SerializeField] private float maxRoamAngle;
            [SerializeField] private float minMoveDistance;
            [SerializeField] private int maxMoveFrames;

            
            private Vector3 positionLastUpdate;
            private int currentStuckCount;
            
#region State Machine Functions
            protected override void SetUp()
            {
                Name = "Roam";
            }

            public override void Enter()
            {
                enemyController.NavMeshAgent.speed = moveSpeed;
                currentStuckCount = 0;
                try  
                {
                    enemyController.enemyAudio.SpiritStateAudioUpdate(enemyController.gameObject, enemyController.spiritAudioEventInstance, 0);
                } 
                catch (Exception e)
                {
                    Debug.LogError("[{RoamEnemyState}]: Error Exception " + e);
                }
            }

            public override void FixedUpdate()
            {
                // Has no path yet
                if (!enemyController.NavMeshAgent.hasPath)
                {
                    enemyController.NavMeshAgent.destination = GetNewRoamPosition();
                }
                else
                {
                    // Has path but is stuck
                    if (IsStuck(positionLastUpdate, enemyController.transform.position, minMoveDistance))
                    {
                        currentStuckCount += 1;
                        // Has been stuck for a while
                        if (currentStuckCount >= maxMoveFrames)
                        {
                            enemyController.NavMeshAgent.destination = GetNewRoamPosition();
                            currentStuckCount = 0;
                        }
                    }
                    // Has path and is not stuck
                    else
                    {
                        currentStuckCount = 0;
                    }
                }
                
                positionLastUpdate = enemyController.transform.position;
                
                // Seen or heard target
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
 
#region Private Functions
            private Vector3 GetNewRoamPosition()
            {
                float _randomAngle = Random.Range(minRoamAngle, maxRoamAngle) * (Random.Range(0, 2) * 2 - 1);
                Vector3 _randomDirection = Quaternion.AngleAxis(_randomAngle, Vector3.up) * enemyController.transform.forward;
                Vector3 _randomPoint = enemyController.transform.position + _randomDirection * Random.Range(minRoamRange, maxRoamRange);
                return GetClosestPointOnNavmesh(_randomPoint);
            }
 #endregion
        }
    }
}