// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-30
// Author: johan
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Numerics;
using FMODUnity;
using JetBrains.Annotations;
using UnityEngine;
using FMOD.Studio;
using Ink.Parsed;
using STOP_MODE = FMOD.Studio.STOP_MODE;
using Vector3 = UnityEngine.Vector3;


namespace Game {
    namespace Audio {

        public enum EventsToBePlayed
        {
            WorldMap,
            GamePlayMusic,
            AmbienceTrees,
            AmbienceWind,
            TimedStinger,
        }
        
        public enum Action
        {
            None,
            PlayMusic,
            StopMusic,
            SetMusicParam,
        }
        
        public class AudioMananger : Singleton<AudioMananger>
        {
            [Header("New Event references")]
            [SerializeField] private EventReference[] musicReferences = new EventReference [5];
            private EventInstance[] musicInstances = new EventInstance [5];

            
           [SerializeField] private EventReference gameOverStinger;
            
            #region Public Functions
            public void PlayMusicEvent(EventsToBePlayed eventsToBePlayed)
            {
                //konvertar enum namn till ints (letar upp events)
                int num = Convert.ToInt32(eventsToBePlayed);

                //Om boolen isActive är "true" så kommer inte musiken startas igen, detta sker via CheckActiveState metoden
                bool isActive = CheckActiveState(musicInstances[num]);
                if (isActive == false)
                {
                    //Debug.Log("event not active before. Activating");
                    //Låten som ska spelas (instansen) är = "num". Num är = "eventsToBePlayed" och "eventsToBePlayed" är det vi valt i vårt enum "EventsToBePlayed" (men utgår från plats i enumet (int istället för namn)
                    //ex plats 2 i enumet blir då event 2 i vår "musicReferences" array
                    musicInstances[num] = RuntimeManager.CreateInstance(musicReferences[num]);
                    musicInstances[num].start();
                                    
                   // Debug.Log("played music event" + num);
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
               // Debug.Log("stopped music event" +" "+ num);
            }
            

            public void SetParameterMusicEvent(EventsToBePlayed eventsToBePlayed, string paramName, float paramValue, bool ignoreSeekSpeed, bool paramIsGlobal)
            {
                if (paramIsGlobal)
                {
                    RuntimeManager.StudioSystem.setParameterByName(paramName, paramValue, ignoreSeekSpeed);
                    //Debug.Log("global param set to" +" "+ paramValue);
                    
                }
                
                
                //konvertar enum namn till ints (letar upp events)
                int num = Convert.ToInt32(eventsToBePlayed);

                musicInstances[num].setParameterByName(paramName, paramValue, ignoreSeekSpeed);
                //Debug.Log("parameter set to " + paramName + paramValue);
            }
            
            
            
            //Kallar på FMOD metod som kollar playbackstate på instansen
            private bool CheckActiveState(EventInstance eInstance)
            {
                bool isActive = true;
                
                //Skriver ut playback state i vår lokala variabel "state" som jämförs nedan i if satsen
                eInstance.getPlaybackState(out PLAYBACK_STATE state);
                //Debug.Log("checking active state" + state);
                
                if (state == PLAYBACK_STATE.STOPPED || state == PLAYBACK_STATE.STOPPING)
                {
                    isActive = false;
                }
                //skickar ut värdet om isActive boolen är true eller false
                return isActive;
            }

            public void WaveStinger(EventsToBePlayed eventsToBePlayed)
            {
                //konvertar enum namn till ints (letar upp events)
                int num = Convert.ToInt32(eventsToBePlayed);
                
                musicInstances[num] = RuntimeManager.CreateInstance(musicReferences[num]);
                musicInstances[num].start();
                musicInstances[num].release();
                //Debug.Log("played music event" + num);
                
            }

            public void GameOverStinger()
            {
                EventInstance gameoverstingerInstance = RuntimeManager.CreateInstance(gameOverStinger);
                gameoverstingerInstance.start();
                gameoverstingerInstance.release();
                Debug.Log("played gameover");

            }

            public void ObjectiveStinger()
            {
                
            }


            #endregion

            #region Private Functions

            #endregion
        }
    }
    
}
