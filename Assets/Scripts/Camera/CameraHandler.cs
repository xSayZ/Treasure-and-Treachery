// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-31
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using Cinemachine;
using Game.Backend;


namespace Game {
    namespace Camera {
        public class CameraHandler : MonoBehaviour
        {
            [SerializeField] CinemachineVirtualCamera vCam;
            [SerializeField] UnityEngine.Camera UICamera;
            CinemachineTargetGroup targetGroup;

            [SerializeField] int weight;
            [SerializeField] int radius;
        

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
                //vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 10;
            }
    
            #endregion

            #region Public Functions

            #endregion

            #region Private Functions

            private void SetCamera()
            {
                GameObject[] _targets = GameManager.Instance.activePlayerControllers.ToArray();

                CinemachineTargetGroup.Target[] targetsArray = new CinemachineTargetGroup.Target[_targets.Length];

                for (int i = 0; i < _targets.Length; i++)
                {
                    targetsArray[i] = new CinemachineTargetGroup.Target
                    {
                        target = _targets[i].transform,
                        weight = weight,
                        radius = radius,
                    };
                }

                targetGroup.m_Targets = targetsArray;
            }

            #endregion
        }
    }
}
