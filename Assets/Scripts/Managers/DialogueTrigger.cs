// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-26
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using Game.Backend;
using UnityEngine;


namespace Game {
    namespace Dialogue {
        public class DialogueTrigger : MonoBehaviour
        {
            [Header("Ink Story")]
            [SerializeField] private CurrentDialogueSO currentDialogueSO;
            
            [SerializeField] private DialogueManager dialogueManager;

            public void OnTriggerEnter(Collider other) {
                Debug.Log("OnTriggerEnter");
                if(other.gameObject.CompareTag("Carriage")) {
                    if (currentDialogueSO.HasBeenRead) return;
                    TriggerDialogue();
                }
            }
            private void TriggerDialogue() {
                if (currentDialogueSO.StoryJSON == null) {
                    Debug.LogError("No story JSON file assigned to the current dialogue SO.");
                    return;
                }
                
                Debug.Log("Triggering dialogue");
                dialogueManager.StartDialogue(currentDialogueSO.StoryJSON, currentDialogueSO.TypingSpeed);
            }
        }
    }
}
