// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-30
// Author: johan
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using FMODUnity;
using UnityEngine;


namespace Game {
    namespace Audio {

        public enum EventsToBePlayed
        {
            HubMusic,
            GamePlayMusic,
            MenuMusic,
            
        }
        public class AudioMananger : MonoBehaviour
        {
            public static AudioMananger Instance { get; private set; }
            
            //tom emitter som får ett värde beroende på vad "get event" metoden skickar från switch case
            public StudioEventEmitter musicEmitter;

            [Header("Music Emitters")]
            [SerializeField] private StudioEventEmitter hubMusic;
            
           
            
            
            
    #region Unity Functions
         private void Awake()
         { //Om instansen inte är denna instans av detta script så ska spelobjektet som instansen 
             if (Instance != null && Instance != this) 
             {
                 Destroy(this); 
             }
             else
             { 
                 Instance = this;
             } 
             DontDestroyOnLoad(this);
         }       
         
            // Update is called once per frame
            void Update()
            {
                
            }
    #endregion

    #region Public Functions

    public void GetEvent(EventsToBePlayed eventsToBePlayed)
    {
        switch (eventsToBePlayed)
        {
            case EventsToBePlayed.HubMusic:
                musicEmitter = hubMusic;
                break;
            
            case EventsToBePlayed.MenuMusic: break;
            
            case EventsToBePlayed.GamePlayMusic: break;
            
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

    public void StopMusic(EventsToBePlayed eventsToBePlayed)
    {//hämtar vilket event vi valt i switchen och stoppar music emittern (musiken)
        GetEvent(eventsToBePlayed);
        musicEmitter.Stop();
    }


    #endregion

    #region Private Functions

    #endregion
        }
    }
}
