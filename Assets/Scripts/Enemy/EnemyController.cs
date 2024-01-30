// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Enemy state machine controller
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Game {
    namespace Enemy {
        public class EnemyController : MonoBehaviour
        {
            [Header("States")]
            public RoamEnemyState RoamEnemyState;
            public AlertEnemyState AlertEnemyState;
            public GrowlEnemyState GrowlEnemyState;
            public ChaseEnemyState ChaseEnemyState;

            [Header("Setup")]
            [SerializeField] private NavMeshAgent navMeshAgent;
            [SerializeField] private SphereCollider visionSphere;
            [SerializeField] private SphereCollider hearingSphere;
            [SerializeField] private LayerMask obstacleLayerMask;

            [Header("Vision and Hearing")]
            [SerializeField] private Transform headOrigin;
            [SerializeField] private float visionRange;
            [SerializeField] private float visionFov;
            [SerializeField] private float hearingRange;
            
            [HideInInspector] public List<Transform> targetsInVisionRange;
            [HideInInspector] public List<Transform> targetsInHearingRange;
            
            private EnemyState currentState;
            private List<Transform> targetsInVisionRangeUpdate;
            
            
#region Unity Functions
            private void Awake()
            {
                RoamEnemyState.Awake(this);
                AlertEnemyState.Awake(this);
                GrowlEnemyState.Awake(this);
                ChaseEnemyState.Awake(this);
                            
                currentState = RoamEnemyState;
            }

            private void Start()
            {
                currentState.Enter();

                targetsInVisionRangeUpdate = new List<Transform>();
                
                // Setup vision and hearing colliders
                visionSphere.radius = visionRange;
                visionSphere.transform.position = headOrigin.position;
                hearingSphere.radius = hearingRange;
                hearingSphere.transform.position = headOrigin.position;
            }
            
            private void FixedUpdate()
            {
                // Update targets in vision range
                for (int i = 0; i < targetsInVisionRangeUpdate.Count; i++)
                {
                    if (IsVisible(targetsInVisionRangeUpdate[i]))
                    {
                        if (!targetsInVisionRange.Contains(targetsInVisionRangeUpdate[i]))
                        {
                            targetsInVisionRange.Add(targetsInVisionRangeUpdate[i]);
                        }
                    }
                    else
                    {
                        if (targetsInVisionRange.Contains(targetsInVisionRangeUpdate[i]))
                        {
                            targetsInVisionRange.Remove(targetsInVisionRangeUpdate[i]);
                        }
                    }
                }
                
                currentState.FixedUpdate();
            }

            private void OnDrawGizmosSelected()
            {
                Gizmos.color = Color.green;
                Utility.Gizmos.GizmoSemiCircle.DrawWireArc(transform.position, transform.forward, visionFov, visionRange);
                
                Gizmos.color = Color.blue;
                Utility.Gizmos.GizmoSemiCircle.DrawWireArc(transform.position, -transform.forward, 360, hearingRange);
            }
#endregion

#region Public Functions
            public void ChangeState(EnemyState _newState)
            {
                currentState.Exit();
                currentState = _newState;
                currentState.Enter();
            }
            
            public NavMeshAgent GetNavMeshAgent()
            {
                return navMeshAgent;
            }
            
            public void VisionRangeEntered(Transform _targetTransform)
            {
                if (_targetTransform.gameObject.CompareTag("Player") && !targetsInVisionRangeUpdate.Contains(_targetTransform))
                {
                    targetsInVisionRangeUpdate.Add(_targetTransform);
                    
                    if (IsVisible(_targetTransform))
                    {
                        targetsInVisionRange.Add(_targetTransform);
                    }
                }
            }

            public void VisionRangeExited(Transform _targetTransform)
            {
                if (targetsInVisionRangeUpdate.Contains(_targetTransform))
                {
                    targetsInVisionRangeUpdate.Remove(_targetTransform);
                }
                
                if (targetsInVisionRange.Contains(_targetTransform))
                {
                    targetsInVisionRange.Remove(_targetTransform);
                }
            }
            
            public void HearingRangeEntered(Transform _targetTransform)
            {
                if (_targetTransform.gameObject.CompareTag("Player") && !targetsInHearingRange.Contains(_targetTransform))
                {
                    targetsInHearingRange.Add(_targetTransform);
                }
            }

            public void HearingRangeExited(Transform _targetTransform)
            {
                if (targetsInHearingRange.Contains(_targetTransform))
                {
                    targetsInHearingRange.Remove(_targetTransform);
                }
            }
#endregion

#region Private Functions
            private bool IsVisible(Transform _targetTransform)
            {
                Vector3 _directionToTarget = new Vector3(_targetTransform.position.x - headOrigin.position.x, 0, _targetTransform.position.z - headOrigin.position.z).normalized;
                
                if (Vector3.Angle(transform.forward, _directionToTarget) < visionFov / 2)
                {
                    float _distanceToTarget = Vector3.Distance(transform.position, _targetTransform.position);
                    
                    if (!Physics.Raycast(headOrigin.position, _directionToTarget, _distanceToTarget, obstacleLayerMask))
                    {
                        return true;
                    }
                }
                
                return false;
            }
#endregion
        }
    }
}