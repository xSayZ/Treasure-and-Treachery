// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-12
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using Cinemachine;
using Game.Player;
using UnityEngine;


namespace Game
{
    namespace Shader
    {
        public class CutoutObject : MonoBehaviour
        {
            [SerializeField] private Transform target;

            [SerializeField] private LayerMask wallMask;
            [Header("Debugs")] public Material[] materials;
            
            public UnityEngine.Camera cam;
            #region Unity Functions

            // Start is called before the first frame update
            void Start()
            {
                cam = UnityEngine.Camera.main;
                target = transform;
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
                RaycastHit hit;
                
                if (Physics.Raycast(cam.transform.position,target.transform.position-cam.transform.position,
                        out hit,Vector3.Distance(target.position,cam.transform.position),wallMask))
                    
                {
                    target.transform.localScale = new Vector3(2, 2, 2);
                }
                else
                {
                    target.transform.localScale = new Vector3(0, 0, 0);

                }
                

            }

            private void OnDrawGizmos(){
            
                Gizmos.color = Color.black;
                Gizmos.DrawRay(cam.transform.position,target.transform.position-cam.transform.position);
                
                
            }

           

            #endregion
        }
    }
}