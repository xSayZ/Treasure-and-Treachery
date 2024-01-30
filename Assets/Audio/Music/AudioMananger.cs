// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-30
// Author: johan
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
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

    

    #endregion

    #region Private Functions

    #endregion
        }
    }
}
