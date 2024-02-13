using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Object = UnityEngine.Object;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    [Tooltip("New Input System bindings to control the typewriter effect.")]
    public InputActionAsset actions;
    [Tooltip("Speed of the typewriter effect.")]
    public float charactersPerSecond = 25;
    [SerializeField] [Range(0.001f, 0.01f)]
    [Tooltip("Speed when skipping dialogue (lower is faster).")]
    public float skippingDelay = 0.003f;
    [Tooltip("Delay the effect when there is interpunctuation in the text (higher is more delay).")]
    public float interpunctuationFactor = 4.5f;

    private TMP_Text textBox;
    private bool typewriterWriting = false;
    private bool skipping = false;
    private Coroutine typewriterCoroutine;

    public static event Action OnTypewriterStop; // Invoked when the typewriter effect is done
    public static event Action<char> OnTypewriterCharacter; // Invoked when a character ("letter") is shown

    // Input map constants (feel free to change)
    private const string ACTION_MAP = "Dialogue";
    private const string SUBMIT_ACTION = "Submit";

    private void Awake()
    {
        textBox = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareNewText);
        actions.FindActionMap(ACTION_MAP).FindAction(SUBMIT_ACTION).performed += OnSubmit;
    }

    private void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareNewText);
        actions.FindActionMap(ACTION_MAP).FindAction(SUBMIT_ACTION).performed -= OnSubmit;
    }

    private void PrepareNewText(Object obj)
    {
        if (obj != textBox || typewriterWriting)
            return;
        typewriterWriting = true;
        skipping = false;
        textBox.maxVisibleCharacters = 0;
        if(typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);
        typewriterCoroutine = StartCoroutine(Typewriter());
    }

    private bool IsInterpunctuation(char c)
    {
        switch (c)
        {
            case '.':
            case ',':
            case '?':
            case '!':
            case ':':
            case ';':
            case '-':
            case '—':
            case '–':
                return true;
            default:
                return false;
        }
    }

    private IEnumerator Typewriter()
    {
        textBox.ForceMeshUpdate();
        TMP_TextInfo info = textBox.textInfo;
        for (int i = 0; i < info.characterCount; i++)
        {
            char c = info.characterInfo[i].character;
            textBox.maxVisibleCharacters++;
            if (skipping)
            {
                yield return new WaitForSeconds(skippingDelay);
                continue;
            }
            OnTypewriterCharacter?.Invoke(c);
            float delay = 1.0f / charactersPerSecond;
            if(IsInterpunctuation(c))
                delay *= interpunctuationFactor;
            yield return new WaitForSeconds(delay);
        }
        typewriterWriting = false;
        skipping = false;
        OnTypewriterStop?.Invoke();
    }

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        if (typewriterWriting)
            skipping = true;
    }
}
