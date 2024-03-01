// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-30
// Author: johan
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using FMOD;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;


namespace Game {
    namespace Audio {
        public class AudioZoneSettings : MonoBehaviour
        {
            private Bus musicBus;
            private bool isMuted;
            
            [System.Serializable]
            public struct AudioSettings
            {
                public EventsToBePlayed eventsToBePlayed;
                public Action action;
                public bool ignoreFadeOut;
                public string paramName;
                public float paramValue;
                public bool ignoreseekspeed;
                public bool paramIsGlobal;

            }
            
            //skapar arrays av variabeln som innehåller "AudioZSettings" 
            public AudioSettings [] audioZoneSettings;
            
        #region Unity Functions
            // Start is called before the first frame update

            private void Start()
            {
                musicBus = RuntimeManager.GetBus("Bus:/PreMaster/Music Bus");
                /*musicBus.getPaused(out bool state);
                Debug.Log("Music Bus state is" + state); */
                
                RunAudioSettings();
            }

            private void OnDisable()
            {
               musicBus.stopAllEvents(STOP_MODE.ALLOWFADEOUT);
               Debug.Log("Unity Scene changed-Music Bus stopped");
               /*musicBus.getPaused(out bool state);
               Debug.Log("Music Bus state is" + state); 
               */
            }

            public void RunAudioSettings()
            {
                foreach (AudioSettings i in audioZoneSettings)
                {
                    if (i.action == Action.None)
                    {
                        Debug.Log(
                            "You have unfinished fields in AudioZoneSettings, action set to -None or -ParameterName was missing");
                    }
                    else
                    {
                        switch (i.action)
                        {
                            case Action.None: 
                                Debug.Log("Action set to None in MusicZoneSettings");
                                break;
                  
                            case Action.PlayMusic: 
                                AudioMananger.Instance.PlayMusicEvent(i.eventsToBePlayed);
                  
                                break;
                    
                            case Action.StopMusic: 
                                AudioMananger.Instance.StopMusicEvent(i.eventsToBePlayed, i.ignoreFadeOut);
                    
                                break;
                  
                            case Action.SetMusicParam:
                                AudioMananger.Instance.SetParameterMusicEvent(i.eventsToBePlayed,i.paramName,i.paramValue,i.ignoreseekspeed, i.paramIsGlobal);
                    
                                break;

                        }
                    }
                }
            }

            #endregion

#region Public Functions

#endregion

#region Private Functions

#endregion
        }
    }
        
}
