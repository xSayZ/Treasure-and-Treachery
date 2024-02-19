// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-31
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Backend;
using Game.Player;


namespace Game {
    namespace Camera {
        public class CameraHandler : MonoBehaviour {
            [Range(10f, 70f)]
            [SerializeField] private float maxZoomOut;

            [SerializeField] private UnityEngine.Camera UICamera;
            [SerializeField] private CinemachineVirtualCamera virtualCamera;
            private Dictionary<int, PlayerController> _targets = new Dictionary<int, PlayerController>();
            private CinemachineTargetGroup targetGroup;

            [SerializeField]
            private int weight;
            [SerializeField]
            private int radius;
        

            #region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                _targets = GameManager.Instance.activePlayerControllers;
                
                targetGroup = FindObjectOfType<CinemachineTargetGroup>();
                SetCamera();
            }

            private void Update()
            {
                UICamera.fieldOfView = UnityEngine.Camera.main.fieldOfView;

                UpdateCameraZoom();
            }
    
            #endregion

            #region Private Functions

            private void SetCamera()
            {
                CinemachineTargetGroup.Target[] _targetsArray = new CinemachineTargetGroup.Target[_targets.Count];

                for (int i = 0; i < _targets.Count; i++)
                {
                    _targetsArray[i] = new CinemachineTargetGroup.Target
                    {
                        target = _targets[i].transform,
                        weight = weight,
                        radius = radius,
                    };
                }

                targetGroup.m_Targets = _targetsArray;
            }

            private void UpdateCameraZoom() {
                // Get the distance between the players
                float distance = Vector3.Distance(_targets[0].transform.position, _targets[1].transform.position);
                
                // Change the camera distance based on the distance between the players
                // Clamp the value so the change is not instant
                virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = Mathf.Clamp(distance, 10, maxZoomOut);
                
            }
            #endregion
        }
    }
}
