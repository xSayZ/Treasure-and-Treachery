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
        public class CameraHandler : MonoBehaviour
        {

            [SerializeField] private UnityEngine.Camera UICamera;
            private CinemachineTargetGroup targetGroup;

            [SerializeField]
            private int weight;
            [SerializeField]
            private int radius;
        

            #region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                targetGroup = FindObjectOfType<CinemachineTargetGroup>();
                SetCamera();
            }

            private void Update()
            {
                UICamera.fieldOfView = UnityEngine.Camera.main.fieldOfView;
            }
    
            #endregion

            #region Private Functions

            private void SetCamera()
            {
                Dictionary<int, PlayerController> _targets = GameManager.Instance.activePlayerControllers;

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

            #endregion
        }
    }
}
