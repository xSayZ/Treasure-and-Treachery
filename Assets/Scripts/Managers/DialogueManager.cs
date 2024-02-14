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
            [SerializeField] private PlayerInput playerInput;
            
            [Header("Ink Story")]
            public TextAsset storyJSON;
            private Story story;
            
            // Internal Bools
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
#endregion

#region Public Functions

            public void SubmitPressed(InputAction.CallbackContext value) {
                if (value.started)
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
                Debug.Log(result);
                return result;
            }
            
            public void ChooseChoiceIndex(int choiceIndex) {
                story.ChooseChoiceIndex(choiceIndex);
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
                    while (story.canContinue)
                    {
                        if (!typing) {
                            StartCoroutine(DisplayLine(story.Continue().Trim()));
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
                        Debug.Log("Add button to leave");
                        yield return new WaitForSeconds(2);
                        if (GetSubmitPressed())
                        {
                            // Switch Scenes
                        }
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

                yield return new WaitForSeconds(0.1f);
                // display each letter one at a time
                foreach (char letter in line) {
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
