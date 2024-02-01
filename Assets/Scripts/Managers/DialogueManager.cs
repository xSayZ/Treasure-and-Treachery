// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-30
// Author: Felix
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using Ink.Runtime;
using TMPro;


namespace Game {
    namespace Dialogue {
        public class DialogueManager : MonoBehaviour
        {
        
            [Header("Dialogue UI")]
            [SerializeField] private GameObject dialoguePanel;
            [SerializeField] private TextMeshProUGUI dialogueText;

            private Story currentStory;
            private bool dialogueIsPlaying;

#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                dialogueIsPlaying = false;
                dialoguePanel.SetActive(false);
            }

            void Update(){
                
            }    
#endregion

#region Public Functions
            public void EnterDialogueMode(TextAsset inkJSON){
                currentStory = new Story(inkJSON.text);
                dialogueIsPlaying = true;
                dialoguePanel.SetActive(true);

                if(currentStory.canContinue){
                    dialogueText.text = currentStory.Continue();
                }
                else {
                    ExitDialogueMode();
                }
            }
#endregion

#region Private Functions
            private void ExitDialogueMode(){
                dialogueIsPlaying = false;
                dialoguePanel.SetActive(false);
                dialogueText.text = "";
            }
#endregion
        }
    }
}
