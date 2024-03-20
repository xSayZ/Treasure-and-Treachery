// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-26
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using UnityEngine.Serialization;


namespace Game {
    namespace Dialogue {
        public class DialogueTrigger : MonoBehaviour
        {
            [FormerlySerializedAs("currentDialogueSO")]
            [Header("Ink Story")]
            [SerializeField] public CurrentDialogueSO CurrentDialogueSO;
            
            private DialogueManager dialogueManager;

            private void Start()
            {
                dialogueManager = FindObjectsByType<DialogueManager>(FindObjectsSortMode.None)[0];
            }

            private void OnTriggerEnter(Collider other) 
            {
                if(other.gameObject.CompareTag("Carriage"))
                {
                    if (CurrentDialogueSO.HasBeenRead) return;
                    TriggerDialogue();
                }
            }

            private void TriggerDialogue()
            {
                if (CurrentDialogueSO.StoryJSON == null)
                {
                    Debug.LogError("No story JSON file assigned to the current dialogue SO.");
                    return;
                }
                
                dialogueManager.StartDialogue(CurrentDialogueSO.StoryJSON, CurrentDialogueSO.TypingSpeed, CurrentDialogueSO.EventImage, this);
            }
        }
    }
}