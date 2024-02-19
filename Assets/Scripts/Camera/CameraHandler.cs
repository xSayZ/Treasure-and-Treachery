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
using UnityEngine.Serialization;


namespace Game {
    namespace Camera {
        public class CameraHandler : MonoBehaviour {
            [Range(10f, 70f)]
            [SerializeField] private float maxZoomOut;
            
            [SerializeField] private UnityEngine.Camera uiCamera;
            [SerializeField] private CinemachineVirtualCamera virtualCamera;
            private Dictionary<int, PlayerController> targets = new Dictionary<int, PlayerController>();
            private CinemachineTargetGroup targetGroup;

            [SerializeField]
            private int weight;
            [SerializeField]
            private int radius;
            
            private bool tempBool = true;
        

            #region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                // Get the active player controllers
                targets = GameManager.Instance.activePlayerControllers;
                if (tempBool) 
                    SetTargetGroupCamera();
            }

            private void Update() {
                
                if (UnityEngine.Camera.main != null)
                    uiCamera.fieldOfView = UnityEngine.Camera.main.fieldOfView;

                UpdateCameraZoom();
            }
    
            #endregion

            #region Private Functions

            private void SetupTargetGroup() {
                targetGroup = FindObjectOfType<CinemachineTargetGroup>();
                SetTargetGroupCamera();
            }
            
            private void SetTargetGroupCamera()
            {
                // Create a new array of targets
                CinemachineTargetGroup.Target[] _targetsArray = new CinemachineTargetGroup.Target[targets.Count];

                // Loop through the players and add them to the target group
                for (int i = 0; i < targets.Count; i++)
                {
                    _targetsArray[i] = new CinemachineTargetGroup.Target
                    {
                        target = targets[i].transform,
                        weight = weight,
                        radius = radius,
                    };
                }

                targetGroup.m_Targets = _targetsArray;
            }

            private void UpdateCameraZoom() {
                int _targetCount = targets.Count;

                // If there are more than 1 player, calculate the average distance between the players
                if (_targetCount > 1) {
                    float _totalDistance = 0f;

                    // Loop through the players and calculate the distance between them
                    for (int i = 0; i < Mathf.Min(_targetCount, 4); i += 2) {
                        // If there are more than 2 players, calculate the distance between the players
                        if (i + 1 < _targetCount) {
                            _totalDistance += CalculateAverageDistance(targets, i, i + 1);
                        }
                    }
                    
                    float CalculateAverageDistance(Dictionary<int, PlayerController> _playerControllers, int _index1, int _index2) {
                        if (!_playerControllers.ContainsKey(_index1) || !_playerControllers.ContainsKey(_index2)) {
                            return 0f;
                        }

                        // Get the transform of the players
                        Transform _transform1 = _playerControllers[_index1].gameObject.transform;
                        Transform _transform2 = _playerControllers[_index2].gameObject.transform;

                        // Calculate the distance between the players
                        return Vector3.Distance(_transform1.position, _transform2.position);
                    }

                    // Calculate the average distance between the players
                    float _averageDistance = _totalDistance / Mathf.Max(1, _targetCount / 2);

                    // Set the camera distance to the average distance between the players
                    virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = Mathf.Clamp(_averageDistance, 10, maxZoomOut);
                }
            }
            #endregion
        }
    }
}
