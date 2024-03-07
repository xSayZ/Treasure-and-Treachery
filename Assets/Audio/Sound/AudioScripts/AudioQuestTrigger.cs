// /*------------------------------
// --------------------------------
// Creation Date: 2024/03/04
// Author: herman
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using UnityEngine.Events;


namespace Game {
    namespace Audio {
        public class AudioQuestTrigger : MonoBehaviour
        {
            public DialogueAudioWrapper dialogueAudioWrapper;
            [Header("Speaking order")]
            public int setSpeakerAfterWolf;
            public int setSpeakerAfterDragon;
            public int setSpeakerAfterWitch;
            public int setSpeakerAfterGorgon;
            public int setSpeakerAfterBoss;
            
            [Header("Set Dialogue Progression")]
            public int setdialogueProgresionWolf;
            public int setdialogueProgresionDragon;
            public int setdialogueProgresionWitch;
            public int setdialogueProgresionGorgon;
            public int setdialogueProgresionBoss;
            
            
            [Header("Settings")]
            [SerializeField] private bool activateOnIsTrigger;
            
            [Header("Events")]
            [SerializeField] private UnityEvent<Transform> triggerEnter;
            [SerializeField] private UnityEvent<Transform> triggerExit;

            private void OnTriggerEnter(Collider other)
            {
                if (!other.isTrigger || activateOnIsTrigger)
                {
                    dialogueAudioWrapper.speakerAfterWolf = setSpeakerAfterWolf;
                    dialogueAudioWrapper.speakerAfterDragon = setSpeakerAfterDragon;
                    dialogueAudioWrapper.speakerAfterWitch = setSpeakerAfterWitch;
                    dialogueAudioWrapper.speakerAfterGorgon = setSpeakerAfterGorgon;
                    dialogueAudioWrapper.speakerAfterBoss = setSpeakerAfterBoss;
                    dialogueAudioWrapper.dialogueProgressionWolf = setdialogueProgresionWolf;
                    dialogueAudioWrapper.dialogueProgressionDragon = setdialogueProgresionDragon;
                    dialogueAudioWrapper.dialogueProgressionWitch = setdialogueProgresionWitch;
                    dialogueAudioWrapper.dialogueProgressionGorgon = setdialogueProgresionGorgon;
                    dialogueAudioWrapper.dialogueProgressionBoss = setdialogueProgresionBoss;
                    triggerEnter.Invoke(other.transform);
                }
            }

            private void OnTriggerExit(Collider other)
            {
                if (!other.isTrigger || activateOnIsTrigger)
                {
                    triggerExit.Invoke(other.transform);
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
