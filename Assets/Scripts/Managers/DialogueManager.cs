// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-30
// Author: Felix
// Description: Dialogue Manager for handling dialogue in the game.
// --------------------------------
// ------------------------------*/

using System.Collections;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;


namespace Game {
    namespace Dialogue {
        public class DialogueManager : MonoBehaviour
        {
            [Header("Params")]
            [SerializeField] private float typingSpeed = 0.05f;
            
            [Header("Dialogue UI")]
            [SerializeField] private GameObject dialoguePanel;
            [SerializeField] private TextMeshProUGUI dialogueText;

            [Header("Choices UI")]
            [SerializeField] private GameObject[] choices;
            [SerializeField] PlayerInput playerInput;
            [SerializeField] private TextAsset inkJSON;
            
            private TextMeshProUGUI[] choicesText;
            
            private bool dialogueIsPlaying;
            private bool submitPressed;
            private bool canContinueToNextLine = false;
            
            private Story currentStory;
            private Coroutine displayLineCoroutine;
            
#region Unity Functions
            void Start()
            {
                dialogueIsPlaying = false;
                dialoguePanel.SetActive(false);
                playerInput.SwitchCurrentActionMap("Menu");

                choicesText = new TextMeshProUGUI[choices.Length];
                int index = 0;
                foreach (GameObject choice in choices)
                {
                    choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                    index++;
                }
                EnterDialogueMode(inkJSON);
            }
            void Update() {

                if (!dialogueIsPlaying)
                    return;
                
                if (canContinueToNextLine 
                    && currentStory.currentChoices.Count > 0 
                    && GetSubmitPressed() ) {
                    ContinueStory();
                }
            }
#endregion

#region Public Functions
            public void EnterDialogueMode(TextAsset _inkJSON){
                currentStory = new Story(_inkJSON.text);
                dialogueIsPlaying = true;
                dialoguePanel.SetActive(true);
                
                ContinueStory();
            }

            public void SubmitPressed(InputAction.CallbackContext value) {
                Debug.Log("Submit Pressed");
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
            
            public void MakeChoice(int _choiceIndex) {
                if (!canContinueToNextLine)
                    return;
                
                currentStory.ChooseChoiceIndex(_choiceIndex);
                GetSubmitPressed(); 
                ContinueStory();
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
                    // stop the coroutine if it's running
                    if (displayLineCoroutine != null) {
                        StopCoroutine(displayLineCoroutine);
                    }
                    // display the next line
                    displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
                    // hide the choices
                    HideChoices();
                    // if there are choices, display them
                    if (currentStory.currentChoices.Count > 0) {
                        DisplayChoices();
                    }
                } else {
                    ExitDialogueMode();
                }
                
            }
            
            private void HideChoices() 
            {
                foreach (GameObject choiceButton in choices) 
                {
                    choiceButton.SetActive(false);
                }
            }

            private void DisplayChoices(){
                List<Choice> currentChoices = currentStory.currentChoices;

                if(currentChoices.Count > choices.Length) {
                    Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
                }

                int index = 0;

                foreach (Choice choice in currentChoices)
                {
                    choices[index].gameObject.SetActive(true);
                    choicesText[index].text = choice.text;
                    index++;
                }

                for (int i = index; i < choices.Length; i++)
                {
                    choices[i].gameObject.SetActive(false);
                }

                StartCoroutine(SelectFirstChoice());
            }

            private IEnumerator DisplayLine(string line) {
                // empty the dialogue text
                dialogueText.text = "";
                // set the flag to false so the player can't continue to the next line
                canContinueToNextLine = false;
                
                bool isAddingRichTextTag = false;
                
                // display each letter one at a time
                foreach (char letter in line.ToCharArray()) {
                    // if the player presses the submit button, skip to the end of the line
                    if (GetSubmitPressed()) {
                        dialogueText.text = line;
                        break;
                    }

                    if (letter == '<' || isAddingRichTextTag) {
                        isAddingRichTextTag = true;
                        dialogueText.text += letter;
                        if (letter == '>') {
                            isAddingRichTextTag = false;
                        }
                    } else {
                        dialogueText.text += letter;
                        yield return new WaitForSeconds(typingSpeed);    
                    }
                }
                // set the flag to true so the player can continue to the next line
                canContinueToNextLine = true;
            }
            private IEnumerator SelectFirstChoice() 
            {
                // Event System requires we clear it first, then wait
                // for at least one frame before we set the current selected object.
                EventSystem.current.SetSelectedGameObject(null);
                yield return new WaitForEndOfFrame();
                EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
            }
            
#endregion
        }
    }
}
