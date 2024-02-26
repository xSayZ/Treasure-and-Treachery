// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-06
// Author: b22alesj
// Description: Script that turns object to face camera
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Utility {
    public class TurnTowardsCamera : MonoBehaviour
    {
        [SerializeField] private bool runContinuously;
        [SerializeField] private bool freezeXAxis;

        private Transform _cameraTransform;
        
        private void Start()
        {
            _cameraTransform = UnityEngine.Camera.main.transform;
            UpdateRotation();
        }

        private void Update()
        {
            if (runContinuously)
            {
                UpdateRotation();
            }
        }

        private void UpdateRotation()
        {
            Vector3 _rotation = _cameraTransform.rotation.eulerAngles;
            
            if (freezeXAxis)
            {
                _rotation = new Vector3(transform.rotation.eulerAngles.x, _rotation.y, _rotation.z);
            }
            
            transform.rotation = Quaternion.Euler(_rotation);
        }
    }
}