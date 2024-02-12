// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/02
// Author: herman
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using FMOD.Studio;
using FMODUnity;


namespace Game {
    namespace Audio {
        
        [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Player")]
        public class PlayerAudio : ScriptableObject
        {
            [Header("Player Actions")]
            [SerializeField] 
            private EventReference playerMeleeAttack;
            [SerializeField] 
            private EventReference playerFootStep;
            [SerializeField]
            private EventReference playerShoot;
            [SerializeField]
            private EventReference projectileHit;
            [SerializeField] 
            private EventReference projectileSwoosh;
            [SerializeField] 
            private EventReference interactionAudio;
            
            [Header("Player Vox")]
            [SerializeField] 
            private EventReference playerWitchVoxTest;



            #region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
                
            }
#endregion

#region Public Functions

public void MeleeAudioPlay(GameObject meleeObj)
{
    EventInstance playerMeleeAttackInstance = RuntimeManager.CreateInstance(playerMeleeAttack);
    RuntimeManager.AttachInstanceToGameObject(playerMeleeAttackInstance, meleeObj.transform, meleeObj.GetComponent<Rigidbody>());
    
    playerMeleeAttackInstance.start();
    playerMeleeAttackInstance.release();
}

public void PlayerFootstepPlay(int textureValue, GameObject footObj)
{
    EventInstance playerFootstepInstance = RuntimeManager.CreateInstance(playerFootStep);
    RuntimeManager.AttachInstanceToGameObject(playerFootstepInstance, footObj.transform);

    switch (textureValue)
    {
        case 0://Grass
            // playerFootstepInstance.setParameterByName("Surface", 0f);
            Debug.Log("ur on grass");
            break;
        case 1: //Path/Dirt
            // playerFootstepInstance.setParameterByName("Surface", 1f);
            Debug.Log("ur on dirt feller");
            break;
        case 2: //Rock
            // playerFootstepInstance.setParameterByName("Surface", 2f);
            break;
    }
    // playerFootstepInstance.start();
    playerFootstepInstance.release();
}

public void PlayerRangedAudio(GameObject weaponObj)
{
    EventInstance playerRangedInstance = RuntimeManager.CreateInstance(playerShoot);
    RuntimeManager.AttachInstanceToGameObject(playerRangedInstance, weaponObj.transform);
    playerRangedInstance.start();
    playerRangedInstance.release();
}

public void ProjectileHitAudio(GameObject projectileObj)
{
    EventInstance projectileHitInstance = RuntimeManager.CreateInstance(projectileHit);
    RuntimeManager.AttachInstanceToGameObject(projectileHitInstance, projectileObj.transform);
    projectileHitInstance.start();
    projectileHitInstance.release();
}

public void ProjectileSwooshAudio(GameObject projectileObj)
{
    EventInstance projectileSwooshInstance = RuntimeManager.CreateInstance(projectileSwoosh);
    RuntimeManager.AttachInstanceToGameObject(projectileSwooshInstance, projectileObj.transform);
    projectileSwooshInstance.start();
    projectileSwooshInstance.release();
}

public EventInstance InteractionAudio(EventInstance interactionInstance, GameObject questObject, bool isLooping)
{
    switch (isLooping)
    {
        case true:
            interactionInstance = RuntimeManager.CreateInstance(interactionAudio);
            RuntimeManager.AttachInstanceToGameObject(interactionInstance, questObject.transform);
            interactionInstance.setParameterByName("InteractLooping", 1);
            interactionInstance.start();
            break;
        case false:
            interactionInstance.setParameterByName("InteractLooping", 0);
            interactionInstance.keyOff();
            interactionInstance.release();
            break;
    }
    return interactionInstance;
}




#endregion

#region Private Functions

#endregion
        }
    }
}
