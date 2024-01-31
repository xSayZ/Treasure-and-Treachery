using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharUnityEvent : UnityEvent<char> {}

public class DialogueLineEvent : MonoBehaviour
{
    [Tooltip("Only run the script events when this is the active speaker.")]
    public string speakerTag = "undefined";
    [Tooltip("Set to a higher value to trigger On Typewriter Character less often.")]
    public uint typewriterEventFrequency = 2;
    private bool speaking = false;
    private int typewriterChar = 0;

    public UnityEvent onStartTalking, onStopTalking, onNewLine;
    public CharUnityEvent onTypewriterCharacter;
    public UnityEvent onTypewriterStop;

    private void OnEnable()
    {
        DialogueSystem.OnSpeakerLine += LineEvent;
        TypewriterEffect.OnTypewriterCharacter += TypewriterCharacterEvent;
        TypewriterEffect.OnTypewriterStop += TypewriterStopEvent;
    }

    private void OnDisable()
    {
        DialogueSystem.OnSpeakerLine -= LineEvent;
        TypewriterEffect.OnTypewriterCharacter -= TypewriterCharacterEvent;
        TypewriterEffect.OnTypewriterStop -= TypewriterStopEvent;
    }

    private void LineEvent(string tag)
    {
        if (tag == speakerTag)
        {
            onNewLine.Invoke();
            if(!speaking) 
                onStartTalking.Invoke();
            speaking = true;
        }
        else
        {
            if (speaking) 
                onStopTalking.Invoke();
            speaking = false;
        }
    }

    private void TypewriterCharacterEvent(char character) {
        if (!speaking) 
            return;
        if(Char.IsPunctuation(character) || Char.IsSeparator(character))
            typewriterChar = 0;
        if(!Char.IsLetterOrDigit(character)) 
            return;
        if(typewriterChar % typewriterEventFrequency == 0)
            onTypewriterCharacter.Invoke(character);
        typewriterChar++;
    }

    private void TypewriterStopEvent()
    {
        if (!speaking) 
            return;
        onTypewriterStop.Invoke();
        typewriterChar = 0;
    }
}
