using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Game.Audio;
using Game.Backend;
using Game.Player;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DialogueAudioWrapper : MonoBehaviour
{
    private static DialogueAudioWrapper instance;
    public static DialogueAudioWrapper Instance => instance;

    public DialogueAudio dialogueAudio;

    public EventReference wolfDialogue, dragonDialogue, witchDialogue, gorgonDialogue, bossDialogue, bossEventDialogue;
    private EventInstance wolfDialogueInstance, dragonDialogueInstance, witchDialogueInstance, gorgonDialogueInstance, bossDialogueInstance;
    public int dialogueProgressionWolf, dialogueProgressionDragon, dialogueProgressionWitch, dialogueProgressionGorgon, dialogueProgressionBoss;
    public int speakerAfterWolf, speakerAfterDragon, speakerAfterWitch, speakerAfterGorgon, speakerAfterBoss;

    [SerializeField] private GameObject wolfIcon, dragonIcon, witchIcon, gorgonIcon, bossIcon;
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
        StartCoroutine(dialogueAudio.WaitForLongResponseAudio(_playerID, askerInstance, eventRef));
    }

    private IEnumerator QueNextDialogue(EventInstance previousSpeakerInstance, int nextSpeaker, GameObject icon)
    {
        while (IsPlaying(previousSpeakerInstance))
        {
            yield return null;
        }
        previousSpeakerInstance.release();
        icon.SetActive(false);
        switch (nextSpeaker)
        {
            case 0:
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
            case 4:
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
        var player = GameManager.Instance.ActivePlayerControllers;
        if (player.ContainsKey(0))
        {
            if (!wolfIcon)
            {
                return;
            }
        
            wolfIcon.SetActive(true);
            wolfDialogueInstance.release();
            wolfDialogueInstance = RuntimeManager.CreateInstance(wolfDialogue);
            wolfDialogueInstance.setParameterByName("DialogueProgression", dialogueProgressionWolf);
            PlayDialogue(0, wolfDialogueInstance);
            StartCoroutine(QueNextDialogue(wolfDialogueInstance, speakerAfterWolf, wolfIcon));
        
            dialogueProgressionWolf++;
            speakerAfterWolf = 4;
        }
    }
    public void DragonQuestDialogue()
    {
        var player = GameManager.Instance.ActivePlayerControllers;
        if (player.ContainsKey(1))
        {
            if (!dragonIcon)
            {
                return;
            }

            dragonIcon.SetActive(true);
            dragonDialogueInstance.release();
            dragonDialogueInstance = RuntimeManager.CreateInstance(dragonDialogue);
            dragonDialogueInstance.setParameterByName("DialogueProgression", dialogueProgressionDragon);
            PlayDialogue(1, dragonDialogueInstance);
            StartCoroutine(QueNextDialogue(dragonDialogueInstance, speakerAfterDragon, dragonIcon));
        
            dialogueProgressionDragon++;
            speakerAfterDragon = 4;
        }
    }
    public void WitchQuestDialogue()
    {
        var player = GameManager.Instance.ActivePlayerControllers;
        if (player.ContainsKey(2))
        {
            if (!witchIcon)
            {
                return;
            }
            witchIcon.SetActive(true);
            witchDialogueInstance.release();
            witchDialogueInstance = RuntimeManager.CreateInstance(witchDialogue);
            witchDialogueInstance.setParameterByName("DialogueProgression", dialogueProgressionWitch);
            PlayDialogue(2, witchDialogueInstance);
            StartCoroutine(QueNextDialogue(witchDialogueInstance, speakerAfterWitch, witchIcon));

            dialogueProgressionWitch++;
            speakerAfterWitch = 4;
        }

    }
    public void GorgonQuestDialogue()
    {
        var player = GameManager.Instance.ActivePlayerControllers;
        if (player.ContainsKey(3))
        {
            if (!gorgonIcon)
            {
                return;
            }
            gorgonIcon.SetActive(true);
            gorgonDialogueInstance.release();
            gorgonDialogueInstance = RuntimeManager.CreateInstance(gorgonDialogue);
            gorgonDialogueInstance.setParameterByName("DialogueProgression", dialogueProgressionGorgon);
            PlayDialogue(3, gorgonDialogueInstance);
            StartCoroutine(QueNextDialogue(gorgonDialogueInstance, speakerAfterGorgon, gorgonIcon));   
        
            dialogueProgressionGorgon++;
            speakerAfterGorgon++;
        }
    }

    public void BossEventDialogue(int dialogueProgressionBoss)
    {
        try
        {
            EventInstance bossEventDialogueInstance = RuntimeManager.CreateInstance(bossEventDialogue);
                    bossEventDialogueInstance.setParameterByName("DialogueProgression", dialogueProgressionBoss);
                    bossEventDialogueInstance.start();
                    bossEventDialogueInstance.release();
        }
        catch (Exception e)
        {
            Debug.LogWarning("[{DialogueAudioWrapper}]: Error Exception " + e);
        }
        
    }
    
    public void BossQuestDialogue()
    {
        if (!bossIcon)
        {
            return;
        }
        
        bossIcon.SetActive(true);
        bossDialogueInstance.release();
        bossDialogueInstance = RuntimeManager.CreateInstance(bossDialogue);
        bossDialogueInstance.setParameterByName("DialogueProgression", dialogueProgressionBoss);
        bossDialogueInstance.start();
        bossDialogueInstance.release();
        StartCoroutine(QueNextDialogue(bossDialogueInstance, speakerAfterBoss, bossIcon));   
        
        dialogueProgressionBoss++;
        speakerAfterBoss = 4;
    }
    private void PlayDialogue(int _playerID, EventInstance dialogueInstance)
    {
        try
        {
            var player = GameManager.Instance.ActivePlayerControllers[_playerID];
            
            RuntimeManager.AttachInstanceToGameObject(dialogueInstance, player.gameObject.transform);
            dialogueInstance.setParameterByName("SpeakerCharacter", _playerID);
            dialogueInstance.start();
            dialogueInstance.release();
        }
        catch (Exception e)
        {
            Debug.LogWarning("[{DialogueAudioWrapper}]: Error Exception " + e);
        }
    }

    public void ReturnToCart()
    {
        var _players = GameManager.Instance.ActivePlayerControllers;
        dialogueAudio.GetRandomPlayerAndPlayDialogue(_players,"return");
    }

}