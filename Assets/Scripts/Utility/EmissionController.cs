// /*------------------------------
// --------------------------------
// Creation Date: 2024/03/03
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utility {
    [ExecuteInEditMode] 
    public class EmissionController : MonoBehaviour
    {
        public List<Material> emissiveGameObjects = new List<Material>();
        //public List<Material> emissiveMaterials = new List<Material>();
        
        [SerializeField] private Color _emissionColorValue;
        [Tooltip("Starting emissionMap color")]
        private List<Color> emissionColor = new List<Color>();
        [Range(0.1f,100f)]
        public float intensity;

        [SerializeField] private bool clearGameObjects;

        public void Update()
        {
            //emissiveMaterials.Clear();
            EmissionChanger();
            RemoveAllGameObjects();
            
        }

        #region Private

        private void EmissionChanger()
        {
            for (int i = 0; i < emissiveGameObjects.Count; i++)
            {
                //emissiveMaterials.Add(emissiveGameObjects[i].GetComponentInChildren<Renderer>().sharedMaterial);
                emissiveGameObjects[i].SetVector("_EmissionColor", _emissionColorValue*intensity);
                
            }
        }
        
        private void RemoveAllGameObjects()
        {
            if (clearGameObjects)
            {
                emissiveGameObjects.Clear();
                clearGameObjects = false;
            }
        }

        #endregion
       
    }
}