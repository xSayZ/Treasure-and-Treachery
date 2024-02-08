// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/07
// Author: herman
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using FMOD.Studio;
using FMODUnity;


namespace Game {
    namespace Audio {
        
        [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Player/Dialogue")]
        public class DialogueAudio : ScriptableObject
        {
            [Header("Dialogue")]
            
            [SerializeField]
            private EventReference shovelPickup, mudReaction, objectiveStartReaction, objectiveProgressionReaction;
            
        

#region Unity Functions

#endregion

#region Public Functions

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

public EventInstance ObjectiveProgressionReactionAudio(GameObject characterObj, EventInstance objectiveProgReactInstance, int speakerCharacter, int responderCharacter)
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

#endregion

#region Private Functions

#endregion
        }
    }
}
