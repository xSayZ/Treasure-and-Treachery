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
         public AudioSettings[] triggerExitAudioSettings;

        public void OnTriggerEnter(Collider other)
        {
          if (other.CompareTag("Player"))
          {
            
            //Foreach loopen, går igenom alla elements i vår struct "AudioSettings" när den går igenom hela vår array "triggerEnterAudioSettings" och
            //uppdaterar varibalers innehåll/info beroende på vad vi skrivit i Inspectorn. Detta sker vid varje "OnTriggerEnter" om "Other" har tag "Player"
            foreach (AudioSettings i in triggerEnterAudioSettings)
              
              //Om det inte skrivits in något i inspectorn på parameternamn eller om action är satt till none så skrivs debug, annars körs nedan kod i nästa scope nedan
              if (i.action == Action.None)
              {
                Debug.Log("action set to -None on AudioTrigger");
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
                  
                }
              }
            
            //När destroyOnTrigger är satt till true, så blir boxcolliderns trigger false
            if (destroyOnTrigger == true)
            {
              gameObject.SetActive(false);
              Debug.Log("boxcollider of gameobject set to false-DestroyOnTrigger is set to true");
            }
            
          }

        }
        
        
        public void OnTriggerExit(Collider other)
        {
          if (other.CompareTag("Player"))
          {
            
            //Foreach loopen, går igenom alla elements i vår struct "AudioSettings" när den går igenom hela vår array "triggerEnterAudioSettings" och
            //uppdaterar varibalers innehåll/info beroende på vad vi skrivit i Inspectorn. Detta sker vid varje "OnTriggerEnter" om "Other" har tag "Player"
            foreach (AudioSettings i in triggerExitAudioSettings)
              
              //Om det inte skrivits in något i inspectorn på parameternamn eller om action är satt till none så skrivs debug, annars körs nedan kod i nästa scope nedan
              if (i.action == Action.None)
              {
                Debug.Log("action set to -None on AudioTrigger");
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
                  
                }
              }
            
            //När destroyOnTrigger är satt till true, så blir boxcolliderns trigger false
            if (destroyOnTrigger == true)
            {
              gameObject.SetActive(false);
              Debug.Log("boxcollider of gameobject set to false-DestroyOnTrigger is set to true");
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
