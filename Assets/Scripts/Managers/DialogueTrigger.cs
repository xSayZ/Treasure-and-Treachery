// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-26
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Dialogue {
        public class DialogueTrigger : MonoBehaviour
        {
            [Header("Ink Story")]
            [SerializeField] private CurrentDialogueSO currentDialogueSO;
            
            private DialogueManager dialogueManager;

            private void Start()
            {
                dialogueManager = FindObjectsByType<DialogueManager>(FindObjectsSortMode.None)[0];
            }

            private void OnTriggerEnter(Collider other) 
            {
                if(other.gameObject.CompareTag("Carriage"))
                {
                    if (currentDialogueSO.HasBeenRead) return;
                    TriggerDialogue();
                }
            }

            private void TriggerDialogue()
            {
                if (currentDialogueSO.StoryJSON == null)
                {
                    Debug.LogError("No story JSON file assigned to the current dialogue SO.");
                    return;
                }
                
                Debug.Log("UI open");
                dialogueManager.StartDialogue(currentDialogueSO.StoryJSON, currentDialogueSO.TypingSpeed);
            }
        }
    }
}