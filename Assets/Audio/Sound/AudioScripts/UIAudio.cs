// /*------------------------------
// --------------------------------
// Creation Date: #DATETIME#
// Author: #DEVELOPER#
// Description: #PROJECTNAME#
// --------------------------------
// ------------------------------*/

using FMOD.Studio;
using FMODUnity;
using UnityEngine;


namespace Game {
    namespace Audio {
        
        [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Ui")]
        public class UIAudio : ScriptableObject
        {
            [SerializeField] private EventReference scrollOpen, scrollClose, uiPing;


            public void ScrollOpenAudio()
            {
                EventInstance scrollOpenInstance = RuntimeManager.CreateInstance(scrollOpen);
                scrollOpenInstance.start();
                scrollOpenInstance.release();
            }
            
            public void ScrollCloseAudio()
            {
                EventInstance scrollCloseInstance = RuntimeManager.CreateInstance(scrollClose);
                scrollCloseInstance.start();
                scrollCloseInstance.release();
            }
            
            public void UiPingAudio()
            {
                EventInstance uiPingInstance = RuntimeManager.CreateInstance(uiPing);
                uiPingInstance.start();
                uiPingInstance.release();
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
