// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/12
// Author: herman
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

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

#region Unity Functions

#endregion

#region Public Functions

public void GoldPickupAudio(GameObject goldObj)
{
    EventInstance goldPickupInstance = RuntimeManager.CreateInstance(goldPickupSound);
    RuntimeManager.AttachInstanceToGameObject(goldPickupInstance, goldObj.transform);
    goldPickupInstance.start();
    goldPickupInstance.release();
}

#endregion

#region Private Functions

#endregion
        }
    }
}
