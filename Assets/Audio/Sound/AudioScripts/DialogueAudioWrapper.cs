using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Game.Audio;
using Game.Backend;
using Game.Player;
using UnityEngine;

public class DialogueAudioWrapper : MonoBehaviour
{
    private static DialogueAudioWrapper instance;
    public static DialogueAudioWrapper Instance => instance;

    public DialogueAudio dialogueAudio;

    public EventReference wolfDialogue, dragonDialogue, witchDialogue, gorgonDialogue;
    private EventInstance wolfDialogueInstance, dragonDialogueInstance, witchDialogueInstance, gorgonDialogueInstance;
    public int dialogueProgressionWolf, dialogueProgressionDragon, dialogueProgressionWitch, dialogueProgressionGorgon;
    public int speakerAfterWolf, speakerAfterDragon, speakerAfterWitch, speakerAfterGorgon;

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
    public void PlayResponseDialogue(EventInstance askerInstance, int _playerID, Dictionary<int, PlayerController> _players, string context)
    {
        StartCoroutine(dialogueAudio.WaitForResponseAudio(askerInstance, _playerID, _players, context));
    }
    
    public void PlayQuestResponseDialogue(int _playerID, EventInstance askerInstance, EventReference eventRef)
    {
        StartCoroutine(dialogueAudio.WaitForQuestResponseAudio(_playerID, askerInstance, eventRef));
    }

    private IEnumerator QueNextDialogue(EventInstance previousSpeakerInstance, int nextSpeaker)
    {
        while (IsPlaying(previousSpeakerInstance))
        {
            yield return null;
        }
        previousSpeakerInstance.release();
        switch (nextSpeaker)
        {
            case 0:
                Debug.Log("prut");
                WolfQuestDialogue();
                break;
            case 1:
                DragonQuestDialogue();
                break;
            case 2:
                WitchQuestDialogue();
                break;
            case 3:
                GorgonQuestDialogue();
                break;
        }
    }
    
    private bool IsPlaying(EventInstance _instance)
    {
        PLAYBACK_STATE state;
        _instance.getPlaybackState(out state);
        return state != PLAYBACK_STATE.STOPPED;
    }
    public void WolfQuestDialogue()
    {
        wolfDialogueInstance = RuntimeManager.CreateInstance(wolfDialogue);
        wolfDialogueInstance.setParameterByName("DialogueProgression", dialogueProgressionWolf);
        PlayDialogue(0, wolfDialogueInstance);
        StartCoroutine(QueNextDialogue(wolfDialogueInstance, speakerAfterWolf));
        dialogueProgressionWolf++;
    }
    public void DragonQuestDialogue()
    {
        dragonDialogueInstance = RuntimeManager.CreateInstance(dragonDialogue);
        dragonDialogueInstance.setParameterByName("DialogueProgression", dialogueProgressionDragon);
        PlayDialogue(1, dragonDialogueInstance);
        StartCoroutine(QueNextDialogue(dragonDialogueInstance, speakerAfterDragon));
        dialogueProgressionDragon++;
    }
    public void WitchQuestDialogue()
    {
        witchDialogueInstance = RuntimeManager.CreateInstance(witchDialogue);
        witchDialogueInstance.setParameterByName("DialogueProgression", dialogueProgressionWitch);
        PlayDialogue(1, witchDialogueInstance);
        StartCoroutine(QueNextDialogue(witchDialogueInstance, speakerAfterWitch));
        dialogueProgressionWitch++;
    }
    public void GorgonQuestDialogue()
    {
        gorgonDialogueInstance = RuntimeManager.CreateInstance(gorgonDialogue);
        gorgonDialogueInstance.setParameterByName("DialogueProgression", dialogueProgressionGorgon);
        PlayDialogue(1, gorgonDialogueInstance);
        StartCoroutine(QueNextDialogue(gorgonDialogueInstance, speakerAfterGorgon));
        dialogueProgressionGorgon++;
    }
    private void PlayDialogue(int _playerID, EventInstance dialogueInstance)
    {
        var player = GameManager.Instance.ActivePlayerControllers[_playerID];
                
        RuntimeManager.AttachInstanceToGameObject(dialogueInstance, player.gameObject.transform);
        dialogueInstance.setParameterByName("SpeakerCharacter", _playerID);
        dialogueInstance.start();
        dialogueInstance.release();
    }

}