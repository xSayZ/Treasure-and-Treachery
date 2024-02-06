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
        private void Update()
        {
            transform.LookAt(UnityEngine.Camera.main.transform.position, Vector3.up);
        }
    }
}
