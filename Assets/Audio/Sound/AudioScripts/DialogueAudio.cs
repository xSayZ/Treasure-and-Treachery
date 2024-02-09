// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/07
// Author: herman
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using Game.Backend;


namespace Game {
    namespace Audio {
        
        [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Player/Dialogue")]
        public class DialogueAudio : ScriptableObject
        {
            [Header("Players")]
            private List<GameObject> playerObjects = new List<GameObject>();
            

            [Header("Dialogue")] 
            [SerializeField]
            private EventReference shovelPickup, mudReaction, objectiveStartReaction, objectiveProgressionReaction;
            public AudioMananger audioManager;
        

#region Unity Functions

        
#endregion

#region Public Functions

public void SetPlayerAudioObjects(GameObject player1, GameObject player2, GameObject player3, GameObject player4)
{
    playerObjects.Clear();
    playerObjects.Add(player1);
    playerObjects.Add(player2);
    playerObjects.Add(player3);
    playerObjects.Add(player4);
}

public EventInstance ShovelPickupAudio(GameObject characterObj, EventInstance shovelPickupInstAudio, int playerCharacterType)
{
    switch (playerCharacterType)
    {
        case 0:
            shovelPickupInstAudio = RuntimeManager.CreateInstance(shovelPickup);
            RuntimeManager.AttachInstanceToGameObject(shovelPickupInstAudio, characterObj.transform);
            shovelPickupInstAudio.setParameterByName("SpeakerCharacter", 0);
            shovelPickupInstAudio.start();
            break;
        case 1:
            shovelPickupInstAudio = RuntimeManager.CreateInstance(shovelPickup);
            RuntimeManager.AttachInstanceToGameObject(shovelPickupInstAudio, characterObj.transform);
            shovelPickupInstAudio.setParameterByName("SpeakerCharacter", 1);
            shovelPickupInstAudio.start();
            break;
        case 2:
            shovelPickupInstAudio = RuntimeManager.CreateInstance(shovelPickup);
            RuntimeManager.AttachInstanceToGameObject(shovelPickupInstAudio, characterObj.transform);
            shovelPickupInstAudio.setParameterByName("SpeakerCharacter", 2);
            shovelPickupInstAudio.start();
            break;
        case 3:
            shovelPickupInstAudio = RuntimeManager.CreateInstance(shovelPickup);
            RuntimeManager.AttachInstanceToGameObject(shovelPickupInstAudio, characterObj.transform);
            shovelPickupInstAudio.setParameterByName("SpeakerCharacter", 3);
            shovelPickupInstAudio.start();
            break;
    }

    shovelPickupInstAudio.release();
    return shovelPickupInstAudio;

}

public EventInstance ObjectiveProgressionReactionAudio(EventInstance objectiveProgReactInstance, GameObject characterObj, int speakerCharacter)
{
    switch (speakerCharacter)
    {
        case 0:
          
            objectiveProgReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
            RuntimeManager.AttachInstanceToGameObject(objectiveProgReactInstance, characterObj.transform);
            objectiveProgReactInstance.setParameterByName("SpeakerCharacter", 0);
            objectiveProgReactInstance.start();
            break;
        case 1:
            objectiveProgReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
            RuntimeManager.AttachInstanceToGameObject(objectiveProgReactInstance, characterObj.transform);
            objectiveProgReactInstance.setParameterByName("SpeakerCharacter", 1);
            objectiveProgReactInstance.start();
            break;
        case 2:
            objectiveProgReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
            RuntimeManager.AttachInstanceToGameObject(objectiveProgReactInstance, characterObj.transform);
            objectiveProgReactInstance.setParameterByName("SpeakerCharacter", 2);
            objectiveProgReactInstance.start();
            break;
        case 3:
            objectiveProgReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
            RuntimeManager.AttachInstanceToGameObject(objectiveProgReactInstance, characterObj.transform);
            objectiveProgReactInstance.setParameterByName("SpeakerCharacter", 3);
            objectiveProgReactInstance.start();
            break;
    }
    objectiveProgReactInstance.release();
    return objectiveProgReactInstance;
}

public GameObject GetRandomPlayer(GameObject excludedPlayer = null)
{
    // Check if there's only one player
    if (playerObjects.Count == 1 && (excludedPlayer == null || playerObjects.Contains(excludedPlayer)))
    {
        Debug.LogError("Only one player GameObject available, which is the speaker. no response generated.");
        return null;
    }

    GameObject randomPlayer = null;

    do
    {
        int randomIndex = Random.Range(0, playerObjects.Count);
        randomPlayer = playerObjects[randomIndex];
    }
    while (randomPlayer == excludedPlayer);

    return randomPlayer;
}

#endregion

#region Private Functions

#endregion
        }
    }
}
