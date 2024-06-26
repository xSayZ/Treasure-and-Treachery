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
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game {
    namespace Audio {
        public class VolumeSliders : MonoBehaviour {
            public Slider MasterVolumeSlider;
            public Slider SFXVolumeSlider;
            public Slider VoiceVolumeSlider;
            public Slider MusicVolumeSlider;

            #region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
                UpdateVolumeSliders();
            }
#endregion

#region Public Functions

    public void UpdateVolumeSliders()
    {
        var sFXVolume = SFXVolumeSlider.value;
        string sfxvcaPath = "vca:/SFX_VCA";
        VCA sfxvca = RuntimeManager.GetVCA(sfxvcaPath);
        sfxvca.setVolume(sFXVolume);

        var VOXVolume = VoiceVolumeSlider.value;
        string voxvcaPath = "vca:/VOX_VCA";
        VCA voxvca = RuntimeManager.GetVCA(voxvcaPath);
        voxvca.setVolume(VOXVolume);
        
        var musicVolume = MusicVolumeSlider.value;
        string musicvcaPath = "vca:/MUSIC_VCA";
        VCA musicvca = RuntimeManager.GetVCA(musicvcaPath);
        musicvca.setVolume(musicVolume);
    }
    

#endregion

#region Private Functions

#endregion
        }
    }
}
