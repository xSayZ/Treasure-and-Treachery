// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-30
// Author: johan
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using FMODUnity;
using JetBrains.Annotations;
using UnityEngine;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;


namespace Game {
    namespace Audio {

        public enum EventsToBePlayed
        {
            HubMusic,
            GamePlayMusic,
            MenuMusic,
            Ambience,
        }

        public enum MusicAction
        {
            None,
            PlayMusic,
            StopMusic,
            SetMusicParam,
        }
        public class AudioMananger : Singleton<AudioMananger>
        {
            [Header("New Event references")]
            [SerializeField] private EventReference[] musicReferences = new EventReference [4];
            private EventInstance[] musicInstances = new EventInstance [4];
            
            
            
            //tom emitter som får ett värde beroende på vad "get event" metoden skickar från switch case
            public StudioEventEmitter musicEmitter;
            public StudioEventEmitter ambienceEmitter;

            [Header("Event references")]
            [SerializeField] private StudioEventEmitter hubMusic;
            [SerializeField] private StudioEventEmitter gamePlayMusic;
            [SerializeField] private StudioEventEmitter ambience;


            private void Start()
            {
               
            }

            public void PlayMusicEvent(EventsToBePlayed eventsToBePlayed)
            {
                //konvertar enum namn till ints (letar upp events)
                int num = Convert.ToInt32(eventsToBePlayed);


                bool isActive = CheckActiveState(musicInstances[num]);
                if (isActive == false)
                {
                    Debug.Log("event not active before. Activating");
                    //Låten som ska spelas (instansen) är = "num". Num är = "eventsToBePlayed" och "eventsToBePlayed" är det vi valt i vårt enum EventsToBePlayed (men utgår från plats i enumet (int istället för namn)
                    //ex plats 2 i enumet blir då event 2 i vår "musicReferences" array
                    musicInstances[num] = RuntimeManager.CreateInstance(musicReferences[num]);
                    musicInstances[num].start();
                                    
                    Debug.Log("event instans spelas");
                }
                
            }
            
            public void StopMusicEvent(EventsToBePlayed eventsToBePlayed, bool ignoreFadeOut)
            {
                //konvertar enum namn till ints (letar upp events)
                int num = Convert.ToInt32(eventsToBePlayed);
                
                if (ignoreFadeOut == true)
                {
                    musicInstances[num].stop(STOP_MODE.IMMEDIATE);
                }
                else
                {
                    
                    musicInstances[num].stop(STOP_MODE.ALLOWFADEOUT);
                }
                
                musicInstances[num].release();
                Debug.Log("event instans stoppas");
            }
            

            public void SetParameterMusicEvent(EventsToBePlayed eventsToBePlayed, string paramName, float paramValue, bool ignoreSeekSpeed)
            {
                //konvertar enum namn till ints (letar upp events)
                int num = Convert.ToInt32(eventsToBePlayed);

                musicInstances[num].setParameterByName(paramName, paramValue, ignoreSeekSpeed);


                Debug.Log("musicparam set on Instance to" + paramValue);

            }
            
            //Kallar på FMOD metod som kollar playbackstate på instansen
            private bool CheckActiveState(EventInstance eInstance)
            {
                bool isActive = true;
                
                //Skriver ut playback state i vår lokala variabel "state" som jämförs nedan i if satsen
                eInstance.getPlaybackState(out PLAYBACK_STATE state);
                Debug.Log("checking active state" + state);
                
                if (state == PLAYBACK_STATE.STOPPED || state == PLAYBACK_STATE.STOPPING)
                {
                    isActive = false;
                }
                //skickar ut värdet om isActive boolen är true eller false
                return isActive;
            }


            #region Public Functions

    public void GetEvent(EventsToBePlayed eventsToBePlayed)
    {
        switch (eventsToBePlayed)
        {
            case EventsToBePlayed.HubMusic:
                musicEmitter = hubMusic;
                break;
            
            case EventsToBePlayed.MenuMusic:
                break;
            
            case EventsToBePlayed.GamePlayMusic:
                musicEmitter = gamePlayMusic;
                break;
            
            case EventsToBePlayed.Ambience: 
                ambienceEmitter = ambience; 
                break;
            
        }
    }

    public void PlayMusic(EventsToBePlayed eventsToBePlayed)
    {
        GetEvent(eventsToBePlayed);
        
        if (!musicEmitter.IsActive)
        {
            musicEmitter.Play();
            Debug.Log("music emitter played" + " " + eventsToBePlayed);
        }
    }

    //hämtar vilket event vi valt i switchen och stoppar music emittern (musiken)
    public void StopMusic(EventsToBePlayed eventsToBePlayed)
    {
        GetEvent(eventsToBePlayed);
        musicEmitter.Stop();
        Debug.Log("musicEvent stopped" + " " + eventsToBePlayed);
    }
        
    //sätter parameter för musikevent i FMOD (ändras via "MusicZoneSettings" cs.
    public void SetMusicParam(string paramName, float paramValue, bool ignoreSeekSpeed)
    {
        musicEmitter.SetParameter(paramName,paramValue,ignoreSeekSpeed);
        Debug.Log("parameter set to" + " " + paramValue + " " + "on parameter" + " " + paramName + " "+ "ignore seek-speed was set to" + ignoreSeekSpeed);
        
    }
    
    

    public void PlayAmbience(EventsToBePlayed eventsToBePlayed)
    {
        GetEvent(eventsToBePlayed);
        ambienceEmitter.Play();
        Debug.Log("ambience emitter played");
    }

    public void StopAmbience(EventsToBePlayed eventsToBePlayed)
    {
        GetEvent(eventsToBePlayed);
        ambienceEmitter.Stop();
        Debug.Log("ambience emitter stopped");
    }
    
    public void SetAmbienceParam(string paramName, float paramValue, bool ignoreSeekSpeed)
    {
        ambienceEmitter.SetParameter(paramName,paramValue,ignoreSeekSpeed);
        Debug.Log("Ambience parameter set to" + " " + paramValue + " " + "on parameter" + " " + paramName + " "+ "ignore seek-speed was set to" + ignoreSeekSpeed);
    }


    #endregion

    #region Private Functions

    #endregion
        }
    }
}
