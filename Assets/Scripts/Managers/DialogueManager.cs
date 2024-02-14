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
using UnityEngine.Serialization;


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
            private TextMeshProUGUI[] choicesText;
            
            [Header("Player Input")]
            [SerializeField] PlayerInput playerInput;
            
            [Header("Ink Story")]
            public TextAsset storyJSON;
            private Story story;
            
            
            private bool dialogueIsPlaying;
            private bool submitPressed;
            private bool canContinueToNextLine = false;
            private bool typing = false;
            private bool hasMadeAChoice = false;

            private Coroutine displayLineCoroutine;
            
#region Unity Functions

            
            void Start()
            {
                if (storyJSON == null)
                {
                    Debug.LogWarning("Drag a valid story JSON file into the StoryReader component.");
                }
                
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

                story = new Story(storyJSON.text);
                StartCoroutine(OnAdvanceStory());
            }

            void Update() {
                
            }
#endregion

#region Public Functions
            public void EnterDialogueMode(TextAsset _inkJSON){
                story = new Story(_inkJSON.text);
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
            
            public void ChooseChoiceIndex(int _choiceIndex) {
                story.ChooseChoiceIndex(_choiceIndex);
                hasMadeAChoice = true;
                HideChoices();
                StartCoroutine(OnAdvanceStory());
            }
#endregion

#region Private Functions

            IEnumerator OnAdvanceStory() {
                dialoguePanel.SetActive(true);
                if (story.canContinue)
                {
                    Debug.Log("Story can continue");
                    while (story.canContinue)
                    {
                        StartCoroutine(DisplayLine(story.Continue()));
                        if (!story.canContinue)  {
                            if (story.currentChoices.Count > 0) {
                                // Set amount of choices available
                            } else  {
                                ExitDialogueMode();
                            }
                        }
                        while (typing)
                            yield return null;
                        if (story.canContinue)
                            yield return new WaitForSeconds(Mathf.Min(1.0f));
                    }
                    if (story.currentChoices.Count > 0) {
                        yield return new WaitForSeconds(1f);
                        DisplayChoices();
                        yield return new WaitForSeconds(0.5f);
                    } else {
                        // Add a button to continue
                        ExitDialogueMode();
                        yield return new WaitForSeconds(2);
                    }
                } else  {
                    yield return new WaitForSeconds(2);
                        ExitDialogueMode();
                }
            }

            private void ExitDialogueMode(){
                dialogueIsPlaying = false;
                dialoguePanel.SetActive(false);
                dialogueText.text = "";
                HideChoices();
            }

            private void ContinueStory() {
                if (story.canContinue) {
                    // stop the coroutine if it's running
                    if (displayLineCoroutine != null) {
                        StopCoroutine(displayLineCoroutine);
                    }
                    // display the next line
                    displayLineCoroutine = StartCoroutine(DisplayLine(story.Continue()));
                    // hide the choices
                    HideChoices();
                    // if there are choices, display them
                    if (story.currentChoices.Count > 0) {
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
                List<Choice> currentChoices = story.currentChoices;

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
                typing = true;
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

                typing = false;
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
