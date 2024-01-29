// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Enemy state machine controller
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Enemy {
        public class EnemyController : MonoBehaviour
        {
            [Header("States")]
            public RoamEnemyState RoamEnemyState;
            public AlertEnemyState AlertEnemyState;
            public GrowlEnemyState GrowlEnemyState;
            public ChaseEnemyState ChaseEnemyState;
            
            [Header("Layer Masks")]
            public LayerMask PlayerLayerMask;
            public LayerMask ObstacleLayerMask;

            [Header("Vision Cone")]
            public float VisionRange;
            public float VisionFov;
            
            private EnemyState currentState;
            
            
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
            }
            
            private void FixedUpdate()
            {
                currentState.FixedUpdate();
            }

            private void OnDrawGizmosSelected()
            {
                Gizmos.color = Color.green;
                Utility.Gizmos.GizmoSemiCircle.DrawWireArc(transform.position, transform.forward, VisionFov, VisionRange);
            }
#endregion

#region Public Functions
            public void ChangeState(EnemyState newState)
            {
                currentState = newState;
                currentState.Enter();
            }
#endregion
        }
    }
}