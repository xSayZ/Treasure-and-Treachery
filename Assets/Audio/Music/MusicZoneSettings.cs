// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-30
// Author: johan
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using FMODUnity;
using UnityEngine;
using FMOD.Studio;


namespace Game {
    namespace Audio {
        public class MusicZoneSettings : MonoBehaviour
        {
        
            public enum Action
            {
                None,
                PlayMusic,
                StopMusic,
                SetMusicParam,
          
                PlayAmbience,
                StopAmbience,
                SetAmbience,
            }
            
            
            [System.Serializable]
            public struct ZoneSettings
            {
                public EventsToBePlayed eventsToBePlayed;
                public Action action;
                public string paramName;
                public float paramValue;
                public bool ignoreSeekSpeed;
            }
            
            //skapar arrays av variabeln som innehåller "AudioZSettings" 
            [NonReorderable] public ZoneSettings [] audioZoneSettingsArray;

            

        #region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                foreach (ZoneSettings i in audioZoneSettingsArray)
                {
                    if (i.paramName == "" || i.action == Action.None)
                    {
                        Debug.Log(
                            "You have unfinished fields in AudioZoneSettings, action set to -None or -ParameterName was missing");
                    }
                    else
                    {
                        switch (i.action)
                        {
                            case Action.None: break;

                            case Action.PlayMusic:
                                AudioMananger.Instance.PlayMusic(i.eventsToBePlayed);
                                break;

                            case Action.StopMusic:
                                AudioMananger.Instance.StopMusic(i.eventsToBePlayed);
                                break;

                            case Action.SetMusicParam: 
                                AudioMananger.Instance.SetMusicParam(i.paramName, i.paramValue, i.ignoreSeekSpeed);
                                break;
                            
                            
                            case Action.PlayAmbience: AudioMananger.Instance.PlayAmbience(i.eventsToBePlayed);
                                break;
                    
                            case Action.StopAmbience: AudioMananger.Instance.StopAmbience(i.eventsToBePlayed);
                                break;
                  
                            case Action.SetAmbience: AudioMananger.Instance.SetAmbienceParam(i.paramName, i.paramValue, i.ignoreSeekSpeed);
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
