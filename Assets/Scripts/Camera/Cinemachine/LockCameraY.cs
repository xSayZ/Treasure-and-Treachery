// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-21
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using Cinemachine;

namespace Game
{
    namespace Cinemachine
    {
        /// <summary>
        /// An add-on module for Cinemachine Virtual Camera that locks the camera's Y co-ordinate
        /// </summary>
        [ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
        public class LockCameraY : CinemachineExtension
        {
            [Tooltip("Lock the camera's Z position to this value")]
            public float m_YPosition = 10;
 
            protected override void PostPipelineStageCallback(
                CinemachineVirtualCameraBase vcam,
                CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
            {
                if (stage == CinemachineCore.Stage.Body)
                {
                    var pos = state.RawPosition;
                    pos.y = m_YPosition;
                    state.RawPosition = pos;
                }
            }
        }
    }
}