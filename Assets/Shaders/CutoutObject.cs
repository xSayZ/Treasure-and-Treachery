// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-12
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game
{
    namespace NAME
    {
        public class CutoutObject : MonoBehaviour
        {
            [SerializeField] private Transform targetObject;

            [SerializeField]private LayerMask wallMask;

            [SerializeField]private UnityEngine.Camera mainCamera;
            
            [Header("Debugs")]
            public Material[] materials;

            public Vector2 cutOutPos;
            public Vector3 offset;
            #region Unity Functions

            // Start is called before the first frame update
            void Start()
            {
                mainCamera = GetComponent<UnityEngine.Camera>();
            }

            // Update is called once per frame
            void Update()
            {
                FindTarget();
            }

            #endregion


            #region Public Functions

            #endregion

            #region Private Functions

            private void FindTarget()
            {
                cutOutPos = mainCamera.WorldToViewportPoint(targetObject.position);
               offset = (targetObject.position-transform.position);
               

                RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset,offset.magnitude,wallMask);
                for (int i = 0; i < hitObjects.Length; ++i)
                {
                    materials= hitObjects[i].transform.GetComponent<Renderer>().materials;

                    for (int m = 0; m < materials.Length; ++m)
                    {
                        
                        materials[m].SetVector("CutoutPosition",cutOutPos);
                        materials[m].SetFloat("_CutoutSize",0.2f);
                        materials[m].SetFloat("_FalloffSize",0.05f);
                    }
                }
            }

            #endregion
            
          
        }
    }
}