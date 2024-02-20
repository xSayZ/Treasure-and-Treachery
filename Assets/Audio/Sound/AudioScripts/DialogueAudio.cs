// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/07
// Author: herman
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using Game.Backend;
using Game.Core;
using Game.Player;
using Game.Quest;
using Random = UnityEngine.Random;
using STOP_MODE = FMOD.Studio.STOP_MODE;


namespace Game {
    namespace Audio {
        
        [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Player/Dialogue")]
        public class DialogueAudio : ScriptableObject
        {
            //EventInstances
            private EventInstance shovelPickupInstance, goldPickupInstance, shovelReactInstance, deathDialogueInstance;

            //EventReferences
            [SerializeField]
            private EventReference shovelPickup, goldPickupAudio, objectiveProgressionReaction, deathAudio;

            private int playerIndex;
           
            private void OnEnable() {
                QuestManager.OnGoldPickedUp.AddListener(GoldPickupDialogue); 
                QuestManager.OnItemPickedUp.AddListener(ShovelPickupAudio);
                GameManager.OnPlayerDeath.AddListener(PlayerDeathDialogue);
            }

#region Unity Functions

#endregion

#region Public Functions

private void PlayerDeathDialogue(int _playerID)
{
    var _players = GameManager.Instance.activePlayerControllers;

    switch (_playerID)
    {
        case 0:
            deathDialogueInstance = RuntimeManager.CreateInstance(deathAudio);
            RuntimeManager.AttachInstanceToGameObject(deathDialogueInstance, _players[0].gameObject.transform);
            deathDialogueInstance.setParameterByName("SpeakerCharacter", 0);
            break;
        case 1:
            deathDialogueInstance = RuntimeManager.CreateInstance(deathAudio);
            RuntimeManager.AttachInstanceToGameObject(deathDialogueInstance, _players[1].gameObject.transform);
            deathDialogueInstance.setParameterByName("SpeakerCharacter", 1);
            break;
        case 2:
            deathDialogueInstance = RuntimeManager.CreateInstance(deathAudio);
            RuntimeManager.AttachInstanceToGameObject(deathDialogueInstance, _players[2].gameObject.transform);
            deathDialogueInstance.setParameterByName("SpeakerCharacter", 2);
            break;
        case 3:
            deathDialogueInstance = RuntimeManager.CreateInstance(deathAudio);
            RuntimeManager.AttachInstanceToGameObject(deathDialogueInstance, _players[3].gameObject.transform);
            deathDialogueInstance.setParameterByName("SpeakerCharacter", 3);
            break;
    }
    deathDialogueInstance.start();
    deathDialogueInstance.release();
}

private void ShovelPickupAudio(int _playerID, Item _item)
{
    var _players = GameManager.Instance.activePlayerControllers;
    
    switch (_playerID)
    {
        case 0:
            shovelPickupInstance = RuntimeManager.CreateInstance(shovelPickup);
            RuntimeManager.AttachInstanceToGameObject(shovelPickupInstance, _players[0].gameObject.transform);
            shovelPickupInstance.setParameterByName("SpeakerCharacter", 0);
            break;
        case 1:
            shovelPickupInstance = RuntimeManager.CreateInstance(shovelPickup);
            RuntimeManager.AttachInstanceToGameObject(shovelPickupInstance, _players[1].gameObject.transform);
            shovelPickupInstance.setParameterByName("SpeakerCharacter", 1);
            break;
        case 2:
            shovelPickupInstance = RuntimeManager.CreateInstance(shovelPickup);
            RuntimeManager.AttachInstanceToGameObject(shovelPickupInstance, _players[2].gameObject.transform);
            shovelPickupInstance.setParameterByName("SpeakerCharacter", 2);
            break;
        case 3:
            shovelPickupInstance = RuntimeManager.CreateInstance(shovelPickup);
            RuntimeManager.AttachInstanceToGameObject(shovelPickupInstance, _players[3].gameObject.transform);
            shovelPickupInstance.setParameterByName("SpeakerCharacter", 3);
            break;
    }
    shovelPickupInstance.start();
    shovelPickupInstance.release();
    
    if (_players.Count > 1 && !IsPlaying(shovelPickupInstance))
    {
        GetRandomPlayerAndPlayResponse(_playerID, _players);
    }

}
private void ObjectiveProgressionReactionAudio(GameObject characterObj, int speakerCharacter)
{
    switch (speakerCharacter)
    {
        case 0:
          
            shovelReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
            RuntimeManager.AttachInstanceToGameObject(shovelReactInstance, characterObj.transform);
            shovelReactInstance.setParameterByName("SpeakerCharacter", 0);
            break;
        case 1:
            shovelReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
            RuntimeManager.AttachInstanceToGameObject(shovelReactInstance, characterObj.transform);
            shovelReactInstance.setParameterByName("SpeakerCharacter", 1);
            break;
        case 2:
            shovelReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
            RuntimeManager.AttachInstanceToGameObject(shovelReactInstance, characterObj.transform);
            shovelReactInstance.setParameterByName("SpeakerCharacter", 2);
            break;
        case 3:
            shovelReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
            RuntimeManager.AttachInstanceToGameObject(shovelReactInstance, characterObj.transform);
            shovelReactInstance.setParameterByName("SpeakerCharacter", 3);
            break;
    }
    shovelReactInstance.start();
    shovelReactInstance.release();
}
private void GoldPickupDialogue(int _playerID, int amount)
{
    var _players = GameManager.Instance.activePlayerControllers;

    switch (_playerID)
    {
     case 0:
         goldPickupInstance = RuntimeManager.CreateInstance(goldPickupAudio);
         RuntimeManager.AttachInstanceToGameObject(goldPickupInstance, _players[0].gameObject.transform);
         goldPickupInstance.setParameterByName("SpeakerCharacter", 0);
         break;
     case 1:
         goldPickupInstance = RuntimeManager.CreateInstance(goldPickupAudio);
         RuntimeManager.AttachInstanceToGameObject(goldPickupInstance, _players[1].gameObject.transform);
         goldPickupInstance.setParameterByName("SpeakerCharacter", 1);
         break;
     case 2:
         goldPickupInstance = RuntimeManager.CreateInstance(goldPickupAudio);
         RuntimeManager.AttachInstanceToGameObject(goldPickupInstance, _players[2].gameObject.transform);
         goldPickupInstance.setParameterByName("SpeakerCharacter", 2);
         break;
     case 3:
         goldPickupInstance = RuntimeManager.CreateInstance(goldPickupAudio);
         RuntimeManager.AttachInstanceToGameObject(goldPickupInstance, _players[3].gameObject.transform);
         goldPickupInstance.setParameterByName("SpeakerCharacter", 3);
         break;
    }
    goldPickupInstance.start();
    goldPickupInstance.release();
}

#endregion

#region Private Functions

        private bool IsPlaying(EventInstance _instance) {
            PLAYBACK_STATE state;
            _instance.getPlaybackState(out state);
            return state != PLAYBACK_STATE.STOPPED;
        }
        
private void GetRandomPlayerAndPlayResponse(int _playerID, Dictionary<int, PlayerController> _players) 
{ 
    var _randomPlayer = _players[Random.Range(0, _players.Count)];

    if (_randomPlayer != _players[_playerID])
    {
        Debug.Log("random player is:" + _randomPlayer.PlayerIndex);
        ObjectiveProgressionReactionAudio(_randomPlayer.gameObject, _randomPlayer.PlayerIndex);
    }
    else {
        GetRandomPlayerAndPlayResponse(_playerID, _players);
    }
}
#endregion
        }
    }
}
