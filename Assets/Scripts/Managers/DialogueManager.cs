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
using UnityEngine.InputSystem;


namespace Game {
    namespace Dialogue {
        public class DialogueManager : MonoBehaviour
        {
        
            [Header("Dialogue UI")]
            [SerializeField] private GameObject dialoguePanel;
            [SerializeField] private TextMeshProUGUI dialogueText;

            [SerializeField] PlayerInput playerInput;
            [SerializeField] private TextAsset inkJSON;

            private Story currentStory;
            private bool dialogueIsPlaying;
            private bool submitPressed;

#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                dialogueIsPlaying = false;
                dialoguePanel.SetActive(false);
                playerInput.SwitchCurrentActionMap("Events");
            }

            void Update(){

                if(!dialogueIsPlaying) {
                    EnterDialogueMode(inkJSON);
                }

                if (GetSubmitPressed()){
                    ContinueStory();
                }
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

            public void SubmitPressed(InputAction.CallbackContext value) {
                if (value.performed)
                {
                    submitPressed = true;
                } else if (value.canceled) {
                    submitPressed = false;
                }
            }

            public bool GetSubmitPressed() 
            {
                bool result = submitPressed;
                submitPressed = false;
                return result;
            }
#endregion

#region Private Functions
            private void ExitDialogueMode(){
                dialogueIsPlaying = false;
                dialoguePanel.SetActive(false);
                dialogueText.text = "";
            }

            private void ContinueStory() {
                if (currentStory.canContinue) {
                    dialogueText.text = currentStory.Continue();
                } else {
                    ExitDialogueMode();
                }
                
            }
            
#endregion
        }
    }
}
