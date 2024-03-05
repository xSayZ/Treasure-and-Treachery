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
using Game.Managers;
using Game.Racer;
using UnityEngine.EventSystems;


namespace Game {
    namespace Dialogue {
        public class DialogueManager : MonoBehaviour {
            [Header("Params")]
            [SerializeField] private float typingSpeed;
            
            [Header("Dialogue UI")]
            [SerializeField] private GameObject dialoguePanel;
            [SerializeField] private TextMeshProUGUI dialogueText;

            [Header("Choices UI")]
            [SerializeField] private GameObject[] choices;
            private TextMeshProUGUI[] choicesText;
            
            [Header("Ink Story")]
            private Story story;
            
            // Internal Bools
            private bool dialogueIsPlaying;
            private bool submitPressed;
            private bool canContinueToNextLine = true;
            private bool typing = false;
            private bool hasMadeAChoice = false;
            private bool isPaused;

            private Coroutine displayLineCoroutine;

            private CarriageRacer carriageRacer;
            
#region Unity Functions

            
            void Start()
            {
                carriageRacer = GameObject.FindGameObjectWithTag("Carriage").GetComponent<CarriageRacer>();
                dialogueIsPlaying = false;
                dialoguePanel.SetActive(false);
            }

            private void Update()
            {
                if(!canContinueToNextLine && !typing && GetSubmitPressed())
                {
                    canContinueToNextLine = true;
                } 
            }
            #endregion

            #region Public Functions

            public void StartDialogue(TextAsset storyJSON, float _typingSpeed) {
                carriageRacer.SetCarriageActive(false);
                
                typingSpeed = _typingSpeed;
                
                choicesText = new TextMeshProUGUI[choices.Length];
                int index = 0;
                foreach (GameObject choice in choices)
                {
                    choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                    index++;
                }
                
                if (dialogueIsPlaying) {
                    Debug.LogWarning("Dialogue is already playing.");
                    return;
                }

                TogglePauseState();
                
                dialogueIsPlaying = true;
                story = new Story(storyJSON.text);
                
                story.BindExternalFunction("giveGold", (int amount) => {
                    Debug.Log("Player received " + amount + " gold.");
                });
                story.BindExternalFunction("giveItem", (string itemName) => {
                    Debug.Log("Player received " + itemName + ".");
                });
                
                StartCoroutine(OnAdvanceStory());
            }
            
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
                        if (!typing && canContinueToNextLine) {
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
                        Debug.Log("Change scene");
                        yield return new WaitForSeconds(5);
                        ExitDialogueMode();
                    }
                } else  {
                    yield return new WaitForSeconds(2);
                        ExitDialogueMode();
                }
            }

            private void ExitDialogueMode(){
                carriageRacer.SetCarriageActive(true);
                dialogueIsPlaying = false;
                dialoguePanel.SetActive(false);
                dialogueText.text = "";
                TogglePauseState();
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
            }
            private IEnumerator SelectFirstChoice() 
            {
                // Event System requires we clear it first, then wait
                // for at least one frame before we set the current selected object.
                EventSystem.current.SetSelectedGameObject(null);
                yield return new WaitForEndOfFrame();
                EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
            }

            private void TogglePauseState() {
                float _newTimeScale = isPaused switch {
                    true => 0f,
                    false => 1f
                };
                
                Time.timeScale = _newTimeScale;
            }
            
            private void ToggleInputState() {
                
            }
            
#endregion
        }
    }
}
