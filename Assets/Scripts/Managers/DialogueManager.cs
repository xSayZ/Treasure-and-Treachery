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
            public bool dialogueIsPlaying { get; private set; }
            private bool submitPressed;
            
#region Unity Functions
            // Start is called before the first frame update
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
            
            public void MakeChoice(int _choiceIndex)
            {
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
                    dialogueText.text = currentStory.Continue();
                    DisplayChoices();
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
