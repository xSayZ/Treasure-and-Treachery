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


namespace Game {
    namespace Audio {
        
        [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Player/Dialogue")]
        public class DialogueAudio : ScriptableObject
        {

            public EventInstance eventInstance;

            [Header("Players")]
            private List<GameObject> playerObjects = new List<GameObject>();
            

            [Header("Dialogue")] 
            [SerializeField]
            private EventReference shovelPickup, mudReaction, objectiveStartReaction, objectiveProgressionReaction;

            private EventInstance shovelPickupInstAudio;

            private int playerIndex;
            
            private void OnEnable() {
                QuestManager.OnItemPickedUp.AddListener(ShovelPickupAudio);
            }

#region Unity Functions

#endregion

#region Public Functions

public void ShovelPickupAudio(int _playerID, Item _item)
{
    var _players = GameManager.Instance.activePlayerControllers;
    switch (_playerID)
    {
        case 0:
            shovelPickupInstAudio = RuntimeManager.CreateInstance(shovelPickup);
            RuntimeManager.AttachInstanceToGameObject(shovelPickupInstAudio, _players[0].gameObject.transform);
            shovelPickupInstAudio.setParameterByName("SpeakerCharacter", 0);
            break;
        case 1:
            shovelPickupInstAudio = RuntimeManager.CreateInstance(shovelPickup);
            RuntimeManager.AttachInstanceToGameObject(shovelPickupInstAudio, _players[1].gameObject.transform);
            shovelPickupInstAudio.setParameterByName("SpeakerCharacter", 1);
            break;
        case 2:
            shovelPickupInstAudio = RuntimeManager.CreateInstance(shovelPickup);
            RuntimeManager.AttachInstanceToGameObject(shovelPickupInstAudio, _players[2].gameObject.transform);
            shovelPickupInstAudio.setParameterByName("SpeakerCharacter", 2);
            break;
        case 3:
            shovelPickupInstAudio = RuntimeManager.CreateInstance(shovelPickup);
            RuntimeManager.AttachInstanceToGameObject(shovelPickupInstAudio, _players[3].gameObject.transform);
            shovelPickupInstAudio.setParameterByName("SpeakerCharacter", 3);
            break;
    }
    
    shovelPickupInstAudio.start();
    shovelPickupInstAudio.release();

    if (!IsPlaying(shovelPickupInstAudio)) {
    
        GetRandomPlayerAndPlaySound(_playerID, _players);
    }

}
private void GetRandomPlayerAndPlaySound(int _playerID, Dictionary<int, PlayerController> _players) {

    var _randomPlayer = _players[Random.Range(0, playerObjects.Count)];

    if (_randomPlayer != _players[_playerID])
    {
        ObjectiveProgressionReactionAudio(eventInstance, _randomPlayer.gameObject, _randomPlayer.PlayerIndex);
    }
    else {
        GetRandomPlayerAndPlaySound(_playerID, _players);
    }
}

public EventInstance ObjectiveProgressionReactionAudio(EventInstance objectiveProgReactInstance, GameObject characterObj, int speakerCharacter)
{
    switch (speakerCharacter)
    {
        case 0:
          
            objectiveProgReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
            RuntimeManager.AttachInstanceToGameObject(objectiveProgReactInstance, characterObj.transform);
            objectiveProgReactInstance.setParameterByName("ResponseCharacter", 0);
            objectiveProgReactInstance.start();
            break;
        case 1:
            objectiveProgReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
            RuntimeManager.AttachInstanceToGameObject(objectiveProgReactInstance, characterObj.transform);
            objectiveProgReactInstance.setParameterByName("ResponseCharacter", 1);
            objectiveProgReactInstance.start();
            break;
        case 2:
            objectiveProgReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
            RuntimeManager.AttachInstanceToGameObject(objectiveProgReactInstance, characterObj.transform);
            objectiveProgReactInstance.setParameterByName("ResponseCharacter", 2);
            objectiveProgReactInstance.start();
            break;
        case 3:
            objectiveProgReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
            RuntimeManager.AttachInstanceToGameObject(objectiveProgReactInstance, characterObj.transform);
            objectiveProgReactInstance.setParameterByName("ResponseCharacter", 3);
            objectiveProgReactInstance.start();
            break;
    }
    objectiveProgReactInstance.release();
    return objectiveProgReactInstance;
}

#endregion

#region Private Functions
            private bool IsPlaying(EventInstance _instance) {
                PLAYBACK_STATE state;
                _instance.getPlaybackState(out state);
                return state != PLAYBACK_STATE.STOPPED;
            }
#endregion
        }
    }
}
