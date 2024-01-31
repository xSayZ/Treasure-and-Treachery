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
            GameManager gameManager;
            CinemachineTargetGroup targetGroup;

            int weight;
            int radius;

            #region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                gameManager = FindObjectOfType<GameManager>();
                targetGroup = FindObjectOfType<CinemachineTargetGroup>();
                SetCamera();
            }
    
            #endregion

            #region Public Functions

            #endregion

            #region Private Functions

            private void SetCamera()
            {
                GameObject[] _targets = gameManager.activePlayerControllers.ToArray();

                CinemachineTargetGroup.Target[] targetsArray = new CinemachineTargetGroup.Target[_targets.Length];

                for (int i = 0; i < _targets.Length; i++)
                {
                    targetsArray[i] = new CinemachineTargetGroup.Target
                    {
                        target = _targets[i].transform,
                        weight = 2,
                        radius = 1,
                    };

                    Debug.Log(_targets[i].transform);
                }

                targetGroup.m_Targets = targetsArray;
                Debug.Log(targetGroup.m_Targets);
            }

            #endregion
        }
    }
}
