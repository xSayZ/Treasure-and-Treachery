// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Enemy state machine controller
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using FMOD.Studio;
using Game.Core;
using Game.Audio;
using Game.Backend;
using UnityEngine;
using UnityEngine.AI;


namespace Game {
    namespace Enemy {
        public class EnemyController : MonoBehaviour, IDamageable
        {
            [Header("States")]
            public RoamEnemyState RoamEnemyState;
            public AlertEnemyState AlertEnemyState;
            public GrowlEnemyState GrowlEnemyState;
            public ChaseEnemyState ChaseEnemyState;
            public StunnedEnemyState StunnedEnemyState;
            
            [Header("Setup")]
            public NavMeshAgent NavMeshAgent;
            [SerializeField] private SphereCollider visionSphere;
            [SerializeField] private SphereCollider hearingSphere;
            [SerializeField] private LayerMask obstacleLayerMask;
            [SerializeField] private EnemyAnimationBehaviour enemyAnimationBehaviour;
            [SerializeField] private EnemyAttackBehaviour enemyAttackBehaviour;
            
            [Header("Pathfinding")]
            [SerializeField] public float maxPathfindingDeviationDistance;
            [SerializeField] public int maxPathfindingRetries;
            
            [field:Header("Health")]
            [field:SerializeField] public int Health { get; set; }
            public bool Invincible { get; set; }
            
            [Header("Vision and Hearing")]
            [SerializeField] private Transform headOrigin;
            [SerializeField] private float visionRange;
            [Range(0, 360)]
            [SerializeField] private float visionFov;
            [SerializeField] private float hearingRange;
            
            [Header("Nav Mesh Agent Update")]
            public float MaxDeviationAngle;
            public float UpdateDistance;
            
            [Header("Audio")] 
            [SerializeField] public EnemyAudio enemyAudio;
            public EventInstance spiritAudioEventInstance;
            
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
                StunnedEnemyState.Awake(this);
                
                currentState = RoamEnemyState;
            }

            private void Start()
            {
                // Setup behaviours
                enemyAnimationBehaviour.SetupBehaviour();
                enemyAttackBehaviour.SetupBehaviour(NavMeshAgent, enemyAnimationBehaviour);
                
                currentState.Enter();
                
                targetsInVisionRangeUpdate = new List<Transform>();
                
                // Setup vision and hearing colliders
                visionSphere.radius = visionRange;
                visionSphere.transform.position = headOrigin.position;
                hearingSphere.radius = hearingRange;
                hearingSphere.transform.position = headOrigin.position;
                
                try  
                {
                    spiritAudioEventInstance = enemyAudio.SpiritStateAudioUpdate(gameObject, spiritAudioEventInstance, 5);
                } 
                catch (Exception e)
                {
                    Debug.LogError("[{EnemyController}]: Error Exception " + e);
                }
            }

            private void FixedUpdate()
            {
                // Update animation
                enemyAnimationBehaviour.UpdateMovementAnimation(NavMeshAgent.velocity.magnitude);
                
                // Update targets in vision range
                for (int i = targetsInVisionRangeUpdate.Count - 1; i >= 0; i--)
                {
                    if (!targetsInVisionRangeUpdate[i]) // Null check
                    {
                        if (targetsInVisionRange.Contains(targetsInVisionRangeUpdate[i]))
                        {
                            targetsInVisionRange.Remove(targetsInVisionRangeUpdate[i]);
                        }
                        
                        targetsInVisionRangeUpdate.Remove(targetsInVisionRangeUpdate[i]);
                        
                        return;
                    }
                    
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
                
                // Remove null references from hearing range
                for (int i = targetsInHearingRange.Count - 1; i >= 0; i--)
                {
                    if (!targetsInHearingRange[i])
                    {
                        targetsInHearingRange.Remove(targetsInHearingRange[i]);
                    }
                }
                
                currentState.FixedUpdate();
            }

            private void OnDrawGizmosSelected()
            {
                // Vision range
                Gizmos.color = Color.green;
                Utility.Gizmos.GizmosExtra.DrawSemiCircle(transform.position, transform.forward, visionFov, visionRange);
                
                // Hearing range
                Gizmos.color = Color.blue;
                Utility.Gizmos.GizmosExtra.DrawCircle(transform.position, hearingRange);
                
                // Values for roam
                Tuple<float, float, float, float> _roamValues = RoamEnemyState.GetRoamValues();
                float _roamAngleRange = (_roamValues.Item4 - _roamValues.Item3 / 2);
                Vector3 _roamDirectionRight = Quaternion.AngleAxis(_roamAngleRange / 2 + _roamValues.Item3 / 2, Vector3.up) * transform.forward;
                Vector3 _roamDirectionLeft = Quaternion.AngleAxis(-(_roamAngleRange / 2 + _roamValues.Item3 / 2), Vector3.up) * transform.forward;
                
                // Roam range
                Gizmos.color = Color.magenta;
                Utility.Gizmos.GizmosExtra.DrawHollowSemiCircle(transform.position, _roamDirectionRight, _roamAngleRange, _roamValues.Item1, _roamValues.Item2);
                Utility.Gizmos.GizmosExtra.DrawHollowSemiCircle(transform.position, _roamDirectionLeft, _roamAngleRange, _roamValues.Item1, _roamValues.Item2);
            }
#endregion

#region Public Functions
            public void ChangeState(EnemyState _newState)
            {
                currentState.Exit();
                NavMeshAgent.ResetPath();
                currentState = _newState;
                currentState.Enter();
            }

            public EnemyState GetCurrentState()
            {
                return currentState;
            }

            public EnemyAttackBehaviour GetEnemyAttackBehaviour()
            {
                return enemyAttackBehaviour;
            }

            public void Death()
            {
                EnemyManager.OnEnemyDeath.Invoke(this);
                
                try
                {
                    enemyAudio.SpiritStateAudioUpdate(gameObject, spiritAudioEventInstance, 3);
                } 
                catch (Exception e)
                {
                    Debug.LogError("[{EnemyController}]: Error Exception " + e);
                }
                
                Destroy(gameObject);
            }

            public void DamageTaken()
            {
                // Enemy has taken damage
            }

            public void VisionRangeEntered(Transform _targetTransform)
            {
                if ((_targetTransform.gameObject.CompareTag("Player")  || _targetTransform.gameObject.CompareTag("Carriage")) && !targetsInVisionRangeUpdate.Contains(_targetTransform))
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