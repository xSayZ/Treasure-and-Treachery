// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-28
// Author: b22alesj
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Utility {
    namespace EditorHelper {
        [ExecuteAlways]
        public class TransformLock : MonoBehaviour
        {
            [Header("Position")]
            [SerializeField] private bool lockPosition;
            [SerializeField] private Vector3 position;
            
            [Header("Rotation")]
            [SerializeField] private bool lockRotation;
            [SerializeField] private Vector3 rotation;
            
            [Header("Scale")]
            [SerializeField] private bool lockScale;
            [SerializeField] private Vector3 scale = new Vector3(1, 1, 1);

            #if (UNITY_EDITOR)
            private void Update()
            {
                if (transform.hasChanged)
                {
                    transform.hasChanged = false;
                    
                    if (lockPosition)
                    {
                        transform.localPosition = position;
                    }
                    
                    if (lockRotation)
                    {
                        transform.localRotation = Quaternion.Euler(rotation);
                    }
                    
                    if (lockScale)
                    {
                        transform.localScale = scale;
                    }
                }
            }
            #endif
        }
    }
}