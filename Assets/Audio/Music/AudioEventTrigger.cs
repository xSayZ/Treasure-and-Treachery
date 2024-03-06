// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-06
// Author: johan
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace Game {
    namespace Audio {
        public class AudioEventTrigger : MonoBehaviour
        {
            private Bus musicBus;
            public bool musicCrossOver = false;
            
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

        
            private void OnDisable()
            {
                if (musicCrossOver==true)
                {
                    Debug.Log("MusicContinued into next scene");
                    return;
                }
                musicBus.stopAllEvents(STOP_MODE.ALLOWFADEOUT);
                Debug.Log("Unity Scene changed-Music Bus stopped");
                /*musicBus.getPaused(out bool state);
                Debug.Log("Music Bus state is" + state);
                */
              
            }

#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                musicBus = RuntimeManager.GetBus("Bus:/PreMaster/Music Bus");
                /*musicBus.getPaused(out bool state);
                Debug.Log("Music Bus state is" + state); */
                
            }
    
            // Update is called once per frame
            void Update()
            {
                
            }
#endregion

#region Public Functions

            public void RunEventAudio()
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

#region Private Functions

#endregion
        }
    }
}
