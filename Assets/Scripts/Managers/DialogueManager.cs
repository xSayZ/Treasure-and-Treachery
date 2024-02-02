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
using System.Collections.Generic;


namespace Game {
    namespace Dialogue {
        public class DialogueManager : MonoBehaviour
        {
        
            [Header("Dialogue UI")]
            [SerializeField] private GameObject dialoguePanel;
            [SerializeField] private TextMeshProUGUI dialogueText;

            [Header("Choices UI")]
            [SerializeField] private GameObject[] choices;
            private TextMeshProUGUI[] choicesText;

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

                choicesText = new TextMeshProUGUI[choices.Length];
                int index = 0;
                foreach (GameObject choice in choices)
                {
                    choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                    index++;
                }
                EnterDialogueMode(inkJSON);

            }

            void Update(){

                if(!dialogueIsPlaying) {
                    
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

            private void DisplayChoices(){
                List<Choice> currentChoices = currentStory.currentChoices;

                if(currentChoices.Count > choices.Length) {
                    Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
                }
            }
            
#endregion
        }
    }
}
