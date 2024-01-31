// /*------------------------------
// --------------------------------
// Creation Date: 2024/01/31
// Author: herman
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.UI;

namespace Game {
    namespace Audio {
        public class VolumeSliders : MonoBehaviour
        {
            public Slider volumeSlider;

            #region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
                UpdateSfxVolume();
            }
#endregion

#region Public Functions

    public void UpdateSfxVolume()
    {
        
         var _sFXVolume = volumeSlider.value;   
        string vcaPath = "vca:/TestVCA";
        FMOD.Studio.VCA vca = FMODUnity.RuntimeManager.GetVCA(vcaPath); vca.setVolume(_sFXVolume); 
    }
    

#endregion

#region Private Functions

#endregion
        }
    }
}
