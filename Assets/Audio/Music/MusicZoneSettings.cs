// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-30
// Author: johan
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using FMODUnity;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


namespace Game {
    namespace Audio {
        public class MusicZoneSettings : MonoBehaviour
        {
        
            public enum Action
            {
                None,
                Play,
                Stop,
                SetParameter,
            }
            
            [System.Serializable]
            public struct ZoneSettings
            {
                public EventsToBePlayed eventsToBePlayed;
                public Action action;
                public string paramName;
                public float paramValue; 
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

                            case Action.Play:
                                AudioMananger.Instance.PlayMusic(i.eventsToBePlayed);
                                break;

                            case Action.Stop:
                                AudioMananger.Instance.StopMusic(i.eventsToBePlayed);
                                break;

                            case Action.SetParameter: break;

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
