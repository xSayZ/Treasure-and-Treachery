using System.Collections.Generic;
using FMOD.Studio;
using Game.Audio;
using Game.Player;
using UnityEngine;

public class DialogueAudioWrapper : MonoBehaviour
{
    private static DialogueAudioWrapper instance;
    public static DialogueAudioWrapper Instance => instance;

    public DialogueAudio dialogueAudio;
    
    private void Awake()
    {
        // Ensure only one instance of DialogueAudioWrapper exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
    public void StartGoldPickupDialogue(EventInstance instance, int _playerID, Dictionary<int, PlayerController> _players)
    {
        StartCoroutine(dialogueAudio.WaitForResponseAudio(instance, _playerID, _players));
    }
}