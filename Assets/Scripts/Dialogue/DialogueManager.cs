// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-30
// Author: Felix
// Description: Dialogue Manager for handling dialogue in the game.
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Game.Backend;
using Game.CharacterSelection;
using Game.Managers;
using Game.Racer;
using Game.Scenes;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Game {
    namespace Dialogue {
        public class DialogueManager : MonoBehaviour {
            [Header("Params")]
            [SerializeField] private float typingSpeed;
            [SerializeField] private bool hasRacer = true;
            
            [Header("Ink Story")]
            private Story story;
            
            [Header("Dialogue UI")]
            [SerializeField] private GameObject dialoguePanel;
            [SerializeField] public Image eventImage;
            [SerializeField] public TextMeshProUGUI dialogueText;
            
            [Header("References")]
            [SerializeField] private Slider progress;
            
            // EventDialogueAudioManager
            [Header("Audio")]
            [SerializeField] private DialogueAudioWrapper dialogueAudio;

            [Header("Choices UI")]
            [SerializeField] private GameObject[] choices;
            private TextMeshProUGUI[] choicesText;
            
            [SerializeField] public List<PlayerData> playerDatas = new List<PlayerData>();
            
            [SerializeField] private UnityEvent OnDialogueEnd = new UnityEvent();
            
            public List<RacerPlayerInput> racerPlayerInputs;
            
            // Internal Bools
            private bool dialogueIsPlaying;
            private bool holdDownIsDone;
            private bool submitPressed;
            private bool canContinueToNextLine = true;
            private bool typing = false;
            private bool isPaused;
            private DialogueTrigger currentTrigger;
            private float holdTime = 0f;
            private float holdThreshold = 1f; // Set this to the desired hold time in seconds

            private Coroutine displayLineCoroutine;

            private CarriageRacer carriageRacer;
            
#region Unity Functions
            private void Start()
            {
                if (hasRacer)
                {
                    carriageRacer = GameObject.FindGameObjectWithTag("Carriage").GetComponent<CarriageRacer>();
                }
                
                if(dialoguePanel != null)
                    dialoguePanel.SetActive(false);
                
                if (TryGetComponent(out Ending _ending))
                {
                    dialogueIsPlaying = true;
                }
                else
                {
                    dialogueIsPlaying = false;
                }
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

            public void StartDialogue(TextAsset _storyJSON, float _typingSpeed, Sprite _eventImage, DialogueTrigger _trigger=null)
            {
                story = new Story(_storyJSON.text);
                
                // Set inputs to dialogue
                foreach (RacerPlayerInput _racerPlayerInput in racerPlayerInputs)
                {
                    _racerPlayerInput.dialogueActice = true;
                }
                
                if (hasRacer) {
                    carriageRacer.SetCarriageActive(false);
                }
                if (_eventImage != null) {
                    eventImage.sprite = _eventImage;
                }
                typingSpeed = _typingSpeed;
                currentTrigger = _trigger;
                
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

#region Ink External Functions
                // Changes the currency of the player.
                // How to use: changeCurrency(100, 0) - This will add 100 currency to the first player;
                story.BindExternalFunction("changeCurrency", (int _amount, int _playerIndex) => {
                    playerDatas[_playerIndex].currency += _amount;
                });
                
                // Changes the personal objective of the player.
                // How to use: changePersonalObjective(5, 0) - This will add 5 personal objective to the first player;
                story.BindExternalFunction("changePersonalObjective", (int _amount, int _playerIndex) => {
                    playerDatas[_playerIndex].personalObjective += _amount;
                });
                
                // Changes the global score of the player.
                // How to use: changeScore(100, 1) - This will add 100 score to the second player;
                story.BindExternalFunction("changeScore", (int _amount, int _playerIndex) => {
                    playerDatas[_playerIndex].points += _amount;
                });
                
                // Changes the kills of the player.
                // How to use: changeKills(1, 0) - This will add 1 kill to the first player;
                story.BindExternalFunction("changeKills", (int _amount, int _playerIndex) => {
                    playerDatas[_playerIndex].kills += _amount;
                });
                
                
                story.BindExternalFunction("PlayEventAudio", (int eventIndex) => {
                    // Play Sound
                    try
                    {
                        dialogueAudio.BossEventDialogue(eventIndex);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("[{DialogueManager}]: Error Exception " + e);
                    }
                    
                });
                
  #endregion
                
                StartCoroutine(OnAdvanceStory());
            }
            
            public void SubmitPressed(InputAction.CallbackContext value) {
                if (value.started)
                {
                    submitPressed = true;
                    if (dialogueIsPlaying)
                        StartCoroutine(IncrementHoldTime());
                } 
                else if (value.canceled) 
                {
                    submitPressed = false;
                    progress.value = 0f; // Reset progress bar
                    holdTime = 0f; // Reset hold time
                }
            }

            public bool GetSubmitPressed() 
            {
                if (typing) {
                    bool _result = holdDownIsDone;
                    holdDownIsDone = false;
                    return _result;
                } else {
                    bool _result = submitPressed;
                    submitPressed = false;
                    return _result;
                }
            }
            
            public void ChooseChoiceIndex(int choiceIndex) {
                story.ChooseChoiceIndex(choiceIndex);
                HideChoices();
                canContinueToNextLine = true;
                StartCoroutine(OnAdvanceStory());
            }

#endregion

#region Private Functions

            IEnumerator OnAdvanceStory() {
                if(dialoguePanel != null)
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
                            yield return new WaitForSeconds(1.0f);
                    }
                    if (story.currentChoices.Count > 0) {
                        yield return new WaitForSeconds(1f);
                        DisplayChoices();
                        yield return new WaitForSeconds(0.5f);
                    } else {
                        yield return new WaitForSeconds(2);
                        ExitDialogueMode();
                    }
                } else  {
                    yield return new WaitForSeconds(2);
                        ExitDialogueMode();
                }
            }

            private void ExitDialogueMode(){
                if(carriageRacer != null)
                    carriageRacer.SetCarriageActive(true);
                dialogueIsPlaying = false;
                if(dialoguePanel != null)
                    dialoguePanel.SetActive(false);
                dialogueText.text = "";
                if (currentTrigger != null)
                    currentTrigger.CurrentDialogueSO.HasBeenRead = true;
                
                // Set inputs to not dialogue
                foreach (RacerPlayerInput _racerPlayerInput in racerPlayerInputs)
                {
                    _racerPlayerInput.dialogueActice = false;
                }
                
                OnDialogueEnd.Invoke();
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
                canContinueToNextLine = false;
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

                canContinueToNextLine = true;
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
            
            private IEnumerator IncrementHoldTime()
            {
                while (submitPressed)
                {
                    holdTime += Time.deltaTime;
                    progress.value = holdTime / holdThreshold;
                    if (holdTime >= holdThreshold)
                    {
                        holdDownIsDone = true;
                        break;
                    }
                    yield return null;
                }
            }
            
#endregion
        }
    }
}
