// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-31
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Enemy;
using Game.Player;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;


namespace Game {
    namespace Camera {
        public class CameraHandler : MonoBehaviour
        {
            [Header("Objective Camera Settings")]
            [SerializeField] private bool useObjectiveCameraOnStart;
            [SerializeField] private float initialWaitTime;
            [SerializeField] private ObjectiveStages[] objectiveStages;
            private ObjectiveTransform[] objectiveTransforms;
            
            [Header("Camera Behaviour Settings")]
            [Range(10f, 20f)]
            [SerializeField] public float maxZoomOut;
            [Range(1, 5)]
            [SerializeField] private int playerWeight;
            [Range(1, 20)]
            [SerializeField] private int playerRadius;
            
            [Header("Camera References")]
            [SerializeField] private UnityEngine.Camera uiCamera;
            [SerializeField] private CinemachineVirtualCamera virtualCamera;
            [SerializeField] private GameObject targetGroupPrefab;
            
            // Private Variables
            private Dictionary<int, PlayerController> targets = new Dictionary<int, PlayerController>();
            private CinemachineTargetGroup playerTargetGroup;
            private ObjectiveTransform objectiveTransform;
            private bool isMaxZoom;
            private Transform[] dummyTransforms;
            private Transform objectiveFollowTransform;
            private List<int> targetKeys = new List<int>();
            private CinemachineFramingTransposer framingTransposer;
            
            private bool canZoom = false;
            
            [Serializable]
            private class ObjectiveStages {
                public ObjectiveTransform[] objectiveTransforms;
                
                [Header("Camera Events")]
                public UnityEvent cameraZoomStartEvent;
                public UnityEvent cameraZoomEndEvent;
                
                public ObjectiveStages(ObjectiveTransform[] _objectiveTransforms, UnityEvent _cameraZoomStartEvent, UnityEvent _cameraZoomEndEvent) {
                    objectiveTransforms = _objectiveTransforms;
                    cameraZoomStartEvent = _cameraZoomStartEvent;
                    cameraZoomEndEvent = _cameraZoomEndEvent;
                }
            }
            
            [Serializable]
            private class ObjectiveTransform {
                public Transform Transform;
                public int Stage;
                public int Zoom;
                public int CameraMoveSpeedToObjective;
                public int TimeUntilNextObjective;
                
                public ObjectiveTransform(Transform _transform, int _stage, int _zoom, int _cameraMoveSpeed, int _timeUntilNextObjective) {
                    Transform = _transform;
                    Stage = _stage;
                    Zoom = _zoom;
                    CameraMoveSpeedToObjective = _cameraMoveSpeed;
                    TimeUntilNextObjective = _timeUntilNextObjective;
                }
            }
            
#region Unity Functions
            private void Awake()
            {
                SetupCamera();
            }

            private void Start()
            {
                CameraZoomEvent();
            }

            private void Update()
            {
                if (UnityEngine.Camera.main != null)
                    uiCamera.fieldOfView = UnityEngine.Camera.main.fieldOfView;
                
                if(canZoom)
                    UpdateCameraZoom();
            }
#endregion

#region Public Functions
            public void CameraZoomEvent(int _stage = 0)
            {
                targets = Backend.GameManager.Instance.ActivePlayerControllers;
                
                if (objectiveStages.Length > 0) {
                    // Get the active player controllers
                    StartCoroutine(MoveCameraToObjectives(_stage));
                }
                else
                {
                    // Set the camera to zoom and update the player movement
                    canZoom = true;
                    SetTargetGroupCamera();
                }
            }

            public void SetCameraZoom(float _zoom)
            {
                maxZoomOut = _zoom;
            }

            public void SetEnemiesActiveState(bool _active)
            {
                var _enemies = Enemy.Systems.EnemyManager.Instance.enemies;
                
                foreach (EnemyController _enemy in _enemies)
                {
                    _enemy.enabled = _active;
                }
            }
#endregion

#region Private Functions
            private void SetupCamera()
            {
                // Get framing transposer
                framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
                
                // Get the active player controllers
                targets = Backend.GameManager.Instance.ActivePlayerControllers;
                transform.position = Backend.GameManager.Instance.spawnRingCenter.position;
                objectiveFollowTransform = new GameObject().transform;
                
                // Create dummy transforms
                dummyTransforms = new Transform[4]; // Hard coded to max 4 players
                for (int i = 0; i < dummyTransforms.Length; i++)
                {
                    Transform _dummyTransform = new GameObject().transform;
                    dummyTransforms[i] = _dummyTransform;
                    _dummyTransform.name = "DummyTransform" + i;
                }
                
                // Create target group
                playerTargetGroup = Instantiate(targetGroupPrefab).GetComponent<CinemachineTargetGroup>();
                playerTargetGroup.transform.position = transform.position;
                virtualCamera.Follow = playerTargetGroup.transform;
            }

            private IEnumerator MoveCameraToObjectives(int _stage)
            {
                float _startingCameraDistance = framingTransposer.m_CameraDistance;
                
                SetPlayerActiveState(false);
                
                // Swap to objective follow transform
                objectiveFollowTransform.position = playerTargetGroup.transform.position;
                objectiveFollowTransform.rotation = playerTargetGroup.transform.rotation;
                virtualCamera.Follow = objectiveFollowTransform;
                
                objectiveStages[_stage].cameraZoomStartEvent.Invoke();
                
                yield return new WaitForSeconds(initialWaitTime);
                
                // Get the objective transforms for the current stage
                objectiveTransforms = objectiveStages[_stage].objectiveTransforms;
                // Loop through the objective transforms
                foreach (ObjectiveTransform _objective in objectiveTransforms)
                {
                    Vector3 _initialPosition = objectiveFollowTransform.position;
                    float _timeElapsed = 0;
                    
                    // Move the camera to the objective transform
                    while (_timeElapsed < _objective.CameraMoveSpeedToObjective)
                    {
                        framingTransposer.m_CameraDistance = Mathf.Lerp(framingTransposer.m_CameraDistance, _objective.Zoom * 1.5f, _timeElapsed / _objective.CameraMoveSpeedToObjective);
                        objectiveFollowTransform.position = Vector3.Lerp(_initialPosition, _objective.Transform.position, _timeElapsed / _objective.CameraMoveSpeedToObjective);
                        _timeElapsed += Time.deltaTime;
                        yield return null;
                    }
                    
                    yield return new WaitForSeconds(_objective.TimeUntilNextObjective);
                }
                
                // Set the camera to zoom and update the player movement
                canZoom = true;
                objectiveStages[_stage].cameraZoomEndEvent.Invoke();
                
                // Swap back to player target group
                virtualCamera.Follow = playerTargetGroup.transform;
                
                framingTransposer.m_CameraDistance = _startingCameraDistance;
                
                SetPlayerActiveState(true);
            }

            private void SetTargetGroupCamera()
            {
                if (playerTargetGroup == null)
                {
                    Debug.LogError("No target group found in the scene");
                    return;
                }
                
                // Create a new array of targets
                CinemachineTargetGroup.Target[] _targetsArray = new CinemachineTargetGroup.Target[targets.Count];
                var keyCollection = targets.Keys;
                foreach (var key in keyCollection)
                {
                    targetKeys.Add(key);
                }
                
                // Loop through the players and add them to the target group
                for (int i = 0; i < targets.Count; i++)
                {
                    _targetsArray[i] = new CinemachineTargetGroup.Target
                    {
                        target = targets[targetKeys[i]].transform,
                        weight = playerWeight,
                        radius = playerRadius,
                    };
                }
                
                playerTargetGroup.m_Targets = _targetsArray;
            }
            
            private void UpdateCameraZoom()
            {
                int _targetCount = targets.Count;
                
                // If there are more than 1 player, calculate the average distance between the players
                if (_targetCount > 1) {
                    float _totalDistance = 0f;

                    // Loop through the players and calculate the distance between them
                    for (int _i = 0; _i < Mathf.Min(_targetCount, 4); _i += 2)
                    {
                        // If there are more than 2 players, calculate the distance between the players
                        if (_i + 1 < _targetCount)
                        {
                            _totalDistance += CalculateAverageDistance(targets, _i, _i + 1);
                        }
                    }
                    
                    float CalculateAverageDistance(Dictionary<int, PlayerController> _playerControllers, int _index1, int _index2)
                    {
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
                    framingTransposer.m_CameraDistance = Mathf.Clamp(_averageDistance, 10, maxZoomOut);
                }
                
                // Update target group to stop players from dragging each other
                if (framingTransposer.m_CameraDistance == maxZoomOut)
                {
                    if (!isMaxZoom)
                    {
                        isMaxZoom = true;
                        CinemachineTargetGroup.Target[] _targetsArray = new CinemachineTargetGroup.Target[targets.Count];
                        var keyCollection = targets.Keys;
                        foreach (var key in keyCollection)
                        {
                            targetKeys.Add(key);
                        }
                        
                        // Loop through the players and add them to the target group
                        for (int i = 0; i < targets.Count; i++)
                        {
                            Transform _dummyTransform = dummyTransforms[i];
                            _dummyTransform.position = targets[targetKeys[i]].transform.position;
                            
                            _targetsArray[i] = new CinemachineTargetGroup.Target
                            {
                                target = _dummyTransform,
                                weight = playerWeight,
                                radius = playerRadius,
                            };
                        }
                        
                        playerTargetGroup.m_Targets = _targetsArray; ;
                    }
                }
                else
                {
                    isMaxZoom = false;
                    SetTargetGroupCamera();
                }
            }

            private void ClearTargetGroup()
            {
                playerTargetGroup.m_Targets = new CinemachineTargetGroup.Target[0];
            }

            private void SetPlayerActiveState(bool _active)
            {
                foreach (var player in targets)
                {
                    player.Value.GetComponent<PlayerMovementBehaviour>().CameraMoveRotateLock = !_active;
                    player.Value.GetComponent<PlayerAttackBehaviour>().SetAttackActiveState(_active);
                }
            }
#endregion
        }
    }
}