using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using Ink.Runtime;

public class DialogueSystem : MonoBehaviour
{
    // Inspector fields
    [Tooltip("The compiled Ink JSON file, containing the game's dialogue.")]
    [SerializeField] private TextAsset inkJSON;
    [Tooltip("When the player makes a choice, the speaker tag will be changed to this.")]
    [SerializeField] private string playerSpeakerTag = "Player";
    [Tooltip("New Input System bindings to control the dialogue.")]
    public InputActionAsset actions;
    [SerializeField]
    [Tooltip("The prefab button used to display dialogue choices.")]
    private Button choiceButtonPrefab = null;
    [Tooltip("Unity will send a warning if the player is given more choices than this.")]
    public int maxChoices = 6;

    // Input map constants (feel free to change)
    private const string ACTION_MAP = "Dialogue";
    private const string SUBMIT_ACTION = "Submit";
    
    // References to GUI objects
    private GameObject dialoguePanel;
    private TextMeshProUGUI dialogueText;
    private GameObject namePanel;
    private TextMeshProUGUI nameText;
    private GameObject buttonParent;
    
    // Unity events
    public UnityEvent onDialogueStart, onDialogueEnd; // Invoked when a dialogue starts/ends 

    // Internal private variables
    private string speakerTag = ""; // Who is speaking right now
    private Dictionary<string, string> nicknames; // If speakerTag exists as a key in here, show the value on-screen instead
    
    // Static variables and Unity actions
    public static Story story; // the main interface for communication with Ink
    public static event Action<string> OnSpeakerLine; // Sent when a new "line" starts. speakerTag is is sent along.
    public static event Action<string, string> OnLineTagPair; // Invoked when a line is tagged with <key>:<value>
    public static event Action<string> OnLineTag; // Invoked on other type of line tags

    private void Awake() 
    {
        if (story == null) 
            story = new Story(inkJSON.text);

        nicknames = new Dictionary<string, string>();
        dialoguePanel = transform.Find("DialoguePanel").gameObject;
        dialogueText = dialoguePanel.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        namePanel = dialoguePanel.transform.Find("NamePanel").gameObject;
        nameText = namePanel.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        buttonParent = transform.Find("Choices").gameObject;

        actions.FindActionMap(ACTION_MAP).FindAction(SUBMIT_ACTION).performed += OnSubmit;
    }

    private void Start() 
    {
        dialoguePanel.SetActive(false);
    }

    // Use this method when you want to start a dialogue
    // Starts from a specified "knot" in the ink story
    public void PlayKnot(string knot) 
    {
        dialoguePanel.SetActive(true);
        story.ChoosePathString(knot);
        onDialogueStart.Invoke();
        actions.FindActionMap(ACTION_MAP).Enable();
        ContinueStory();
    }

    private void ProcessLineTags()
    {
        if (story.currentTags.Count == 0)
            return;
        var tagList = story.currentTags.Where(x => x.Split(":", 2).Length != 2).ToList();
        var pairs = story.currentTags.Except(tagList).
            Select((x) => (x.Split(":")[0].Trim(), 
                                x.Split(":")[1].Trim()));
        foreach(string t in tagList)
        {
            OnLineTag?.Invoke(t.Trim());
        }
        
        foreach ((string key, string value) in pairs)
        {
            if (key == "nickname")
                nicknames[speakerTag] = value;
            OnLineTagPair?.Invoke(key, value);
        } 
    }
    
    private string ProcessInkText()
    {
        string text = story.Continue();
        string[] prefixes = text.Split(":", 2);
        if (prefixes.Length != 2)
        {
            SetSpeaker(speakerTag);
            return text;
        }
        SetSpeaker(prefixes[0].Trim());
        text = prefixes[1].Trim();
        return text;
    }

    private void ContinueStory()
    {
        if(story.canContinue)
        {
            dialogueText.SetText(ProcessInkText());
            OnSpeakerLine?.Invoke(speakerTag);
            StartCoroutine(DisplayChoices());
        }
        else 
            ExitDialogue();
    }

    private void OnClickChoiceButton (Choice choice) 
    {
        actions.FindActionMap(ACTION_MAP).Enable();
        story.ChooseChoiceIndex (choice.index);
        foreach (Transform child in buttonParent.transform)
        {
            Destroy(child.gameObject);
        }

        speakerTag = playerSpeakerTag;
        ContinueStory();
    }

    private Button CreateChoiceButton(string text)
    {
        Button choice = Instantiate(choiceButtonPrefab, buttonParent.transform) as Button;
        TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
        choiceText.text = text;
        return choice;
    }
    
    private IEnumerator DisplayChoices()
    {
        if (story.currentChoices.Count <= 0) yield break;
        yield return new WaitForEndOfFrame();
        yield return new WaitWhile(() => dialogueText.maxVisibleCharacters < dialogueText.textInfo.characterCount);
        if (story.currentChoices.Count > maxChoices)
        {
            Debug.LogWarning("More choices available than the set max choices.");
        }
        foreach (var choice in story.currentChoices)
        {
            Button button = CreateChoiceButton(choice.text.Trim());
            button.onClick.AddListener(delegate
            {
                OnClickChoiceButton(choice);
            });
        }
        // Mark the first choice in the GUI as selected
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(buttonParent.transform.GetChild(0).gameObject);
    }
    
    private void SetSpeaker(string speaker)
    {
        speakerTag = speaker;
        ProcessLineTags();
        nameText.text = nicknames.TryGetValue(speakerTag, out var nickname) ? nickname : speakerTag;
        namePanel.SetActive(speaker.Length > 0);
    }

    private void ExitDialogue() 
    {
        actions.FindActionMap(ACTION_MAP).Disable();
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        onDialogueEnd.Invoke();
        OnSpeakerLine?.Invoke("NOONE");
    }

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        if (story.currentChoices.Count == 0 && 
            dialogueText.maxVisibleCharacters >= dialogueText.textInfo.characterCount)
        {
            ContinueStory();
        }
    }
}
