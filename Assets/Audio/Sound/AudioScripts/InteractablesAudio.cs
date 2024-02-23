// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/12
// Author: herman
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace Game {
    namespace Audio {
        [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Interactables")]
        public class InteractablesAudio : ScriptableObject
        {
            [Header("Interactables")] 
            [SerializeField]
            private EventReference goldPickupSound;
            
            [SerializeField] 
            private EventReference carriageHit;

#region Public Functions
            public void GoldPickupAudio(GameObject goldObj)
            {
                try
                {
                    EventInstance goldPickupInstance = RuntimeManager.CreateInstance(goldPickupSound);
                    RuntimeManager.AttachInstanceToGameObject(goldPickupInstance, goldObj.transform);
                    goldPickupInstance.start();
                    goldPickupInstance.release();
                }
                catch (Exception e)
                {
                    Debug.LogError("[{InteractablesAudio}]: Error Exception " + e);
                }
            }

            public void CarriageHitAudio(GameObject goldObj)
            {
                try
                {
                    EventInstance carriageHitInstance = RuntimeManager.CreateInstance(carriageHit);
                    RuntimeManager.AttachInstanceToGameObject(carriageHitInstance, goldObj.transform);
                    carriageHitInstance.start();
                    carriageHitInstance.release();
                }
                catch (Exception e)
                {
                    Debug.LogError("[{InteractablesAudio}]: Error Exception " + e);
                }
            }
#endregion
        }
    }
}
