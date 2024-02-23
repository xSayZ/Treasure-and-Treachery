// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-05
// Author: johan
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using FMODUnity;
using UnityEngine;
using System;
using FMOD.Studio;
using Debug = UnityEngine.Debug;

namespace Game {
    namespace Audio {
      public class AudioTrigger : MonoBehaviour
      {
          
        public bool destroyOnTrigger = false;
        
  //Skapar Enum för att kunna ha valmöjlighet senare i structen "AudioSettings"
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
        
        //Inställningar som görs i inspectorn för vilket event och vad som ska ske, samt vilken parameter och parametervärde som ska justeras
        [Serializable]
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
        //skapar arrays av variabeln som innehåller "AudioSettings" 
         public AudioSettings [] triggerEnterAudioSettings;
        [NonReorderable] public AudioSettings [] triggerExitAudioSettings;

        public void OnTriggerEnter(Collider other)
        {
          if (other.CompareTag("Player"))
          {
            //Foreach loopen, går igenom alla elements i vår struct "AudioSettings" när den går igenom hela vår array "triggerEnterAudioSettings" och
            //uppdaterar varibalers innehåll/info beroende på vad vi skrivit i Inspectorn. Detta sker vid varje "OnTriggerEnter" om "Other" har tag "Player"
            foreach (AudioSettings i in triggerEnterAudioSettings)
              
              //Om det inte skrivits in något i inspectorn på parameternamn eller om action är satt till none så skrivs debug, annars körs nedan kod i nästa scope nedan
              if (i.paramName == "" || i.action == Action.None)
              {
                Debug.Log("You have unfinished fields in Audiotrigger-OnTriggerEnter, action set to -None or -ParameterName was missing");
              }
              
              else
              {
                //Jämför vilken action vi valt från vårt enum Action som finns i "AudioSettings"
                switch (i.action)
                {
                  case Action.None: 
                    Debug.Log("Action set to None on AudioTrigger - OnTriggerEnter");
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
                    
                  
                  case Action.PlayAmbience: AudioMananger.Instance.PlayAmbience(i.eventsToBePlayed);
                    break;
                    
                  case Action.StopAmbience: AudioMananger.Instance.StopAmbience(i.eventsToBePlayed);
                    break;
                  
                  case Action.SetAmbience: AudioMananger.Instance.SetAmbienceParam(i.paramName, i.paramValue, i.ignoreseekspeed);
                    break;
                }
              }
            
            //När destroyOnTrigger är satt till true, så blir boxcolliderns trigger false
            if (destroyOnTrigger == true)
            {
              gameObject.SetActive(false);
              Debug.Log("boxcollider of gameobject set to false, OnTriggerEnter and OnTriggerExit is NOT active on this gameobject");
            }
            
          }

        }
        
        
        //Samma metod/funktion som "OnTriggerEnter" ovan, men detta är när spelaren lämnar box collidern
        private void OnTriggerExit(Collider other)
        {
          if (other.CompareTag("Player"))
          {
            foreach (AudioSettings i in triggerExitAudioSettings)
              
              if (i.paramName == "" || i.action == Action.None)
              {
                Debug.Log("You have unfinished fields in Audiotrigger-OnTriggerExit, action set to -None or -ParameterName was missing");
              }
              
              else
              {
                switch (i.action)
                { 
                  case Action.None: 
                    Debug.Log("Action set to None on AudioTrigger - OnTriggerExit");
                    break;
                  
                  case Action.PlayMusic: AudioMananger.Instance.PlayMusic(i.eventsToBePlayed);
                    break;
                
                  case Action.StopMusic: AudioMananger.Instance.StopMusic(i.eventsToBePlayed);
                    break;
                  
                  //Dem nya världena som finns i variblarna från vår array "triggerExitAudioSettings" skickas med in i "SetParameter();" metoden i vår AudioMananger
                  case Action.SetMusicParam: 
                    AudioMananger.Instance.SetMusicParam(i.paramName, i.paramValue, i.ignoreseekspeed);
                    //Skriver ut de nya världena i konsollen. 
                    Debug.Log("parameter set to" + " " + i.paramValue + " " + "on parameter" + " " + i.paramName + " "+ "ignore seek-speed was set to" + i.ignoreseekspeed + "On Exit");
                    break;
                  
                  
                  case Action.PlayAmbience: AudioMananger.Instance.PlayAmbience(i.eventsToBePlayed);
                    break;
                  
                  case Action.StopAmbience: AudioMananger.Instance.StopAmbience(i.eventsToBePlayed);
                    break;
                  
                  case Action.SetAmbience: AudioMananger.Instance.SetAmbienceParam(i.paramName, i.paramValue, i.ignoreseekspeed);
                    break;
                }
              }
          }
        }
  
  
  
  
  

         




          #region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
                
            }
#endregion

#region Public Functions

#endregion

#region Private Functions

#endregion
        }
    }
}
