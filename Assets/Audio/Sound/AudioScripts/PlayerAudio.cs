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
using STOP_MODE = FMOD.Studio.STOP_MODE;


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
            private EventReference soulFireAudio;
            [SerializeField]
            private EventReference projectileHit;
            [SerializeField] 
            private EventReference projectileSwoosh;
            [SerializeField] 
            private EventReference interactionAudio;
            [SerializeField] 
            private EventReference gorgonPetrifyAudio;
            [SerializeField] 
            private EventReference gorgonPetrifySmashAudio;
            [SerializeField] 
            private EventReference dragonShootAudio;
            [SerializeField] 
            private EventReference dragonArrowAudio;


            private EventInstance dragonShootInstance;
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

public void PetrifySmashAudio(GameObject enemyObj)
{
    EventInstance petrifySmashInstance = RuntimeManager.CreateInstance(gorgonPetrifySmashAudio);
    RuntimeManager.AttachInstanceToGameObject(petrifySmashInstance, enemyObj.transform, enemyObj.GetComponent<Rigidbody>());
    
    petrifySmashInstance.start();
    petrifySmashInstance.release();
}

public void SoulFireAudio(GameObject projectileObj)
{
    EventInstance soulFireInstance = RuntimeManager.CreateInstance(soulFireAudio);
    RuntimeManager.AttachInstanceToGameObject(soulFireInstance, projectileObj.transform, projectileObj.GetComponent<Rigidbody>());
    
    soulFireInstance.start();
    soulFireInstance.release();
}

public void PetrifyAudio(GameObject enemyObj)
{
    EventInstance petrifyInstance = RuntimeManager.CreateInstance(gorgonPetrifyAudio);
    RuntimeManager.AttachInstanceToGameObject(petrifyInstance, enemyObj.transform, enemyObj.GetComponent<Rigidbody>());
    
    petrifyInstance.start();
    petrifyInstance.release();
}

public EventInstance DragonShoot(GameObject shootObj, bool isCharging, EventInstance dragonShootinstance)
{
    
    if (isCharging == true)
    {
        dragonShootInstance = RuntimeManager.CreateInstance(dragonShootAudio);
        dragonShootInstance.setParameterByName("RangeCharge", 0);
        RuntimeManager.AttachInstanceToGameObject(dragonShootInstance, shootObj.transform);
        dragonShootInstance.start();
    }

    if (isCharging == false)
    {
        dragonShootInstance.stop(STOP_MODE.IMMEDIATE);
        dragonShootInstance.release();
    }
    return dragonShootInstance;
}

public void DragonArrowAudio(GameObject arrowObj)
{
    EventInstance dragonArrowInstance = RuntimeManager.CreateInstance(dragonArrowAudio);
    RuntimeManager.AttachInstanceToGameObject(dragonArrowInstance, arrowObj.transform);
    dragonArrowInstance.start();
    dragonArrowInstance.release();
}

public void MeleeAudioPlay(GameObject meleeObj, string characterType)
{
    EventInstance playerMeleeAttackInstance = RuntimeManager.CreateInstance(playerMeleeAttack);
    if (characterType == "gorgon")
    {
        playerMeleeAttackInstance.setParameterByName("MeleeType", 1);
    }
    else if (characterType == "wolf")
    {
        playerMeleeAttackInstance.setParameterByName("MeleeType", 0);
    }
    RuntimeManager.AttachInstanceToGameObject(playerMeleeAttackInstance, meleeObj.transform, meleeObj.GetComponent<Rigidbody>());
    
    playerMeleeAttackInstance.start();
    playerMeleeAttackInstance.release();
}

public void PlayerFootstepPlay(float grassValue, float dirtValue, float rockValue, GameObject footObj)

{
    EventInstance playerFootstepInstance = RuntimeManager.CreateInstance(playerFootStep);
    RuntimeManager.AttachInstanceToGameObject(playerFootstepInstance, footObj.transform);

    playerFootstepInstance.setParameterByName("Grass", grassValue);
    playerFootstepInstance.setParameterByName("Path", dirtValue);
    playerFootstepInstance.setParameterByName("Rock", rockValue);
    
    playerFootstepInstance.start();
    playerFootstepInstance.release();
}

public void PlayerRangedAudio(GameObject weaponObj)
{
    EventInstance playerRangedInstance = RuntimeManager.CreateInstance(playerShoot);
    RuntimeManager.AttachInstanceToGameObject(playerRangedInstance, weaponObj.transform);
    playerRangedInstance.start();
    playerRangedInstance.release();
}

public void ProjectileHitAudio(GameObject projectileObj, int type)
{
    EventInstance projectileHitInstance = RuntimeManager.CreateInstance(projectileHit);
    RuntimeManager.AttachInstanceToGameObject(projectileHitInstance, projectileObj.transform);
    projectileHitInstance.setParameterByName("ParticleType", type);
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

public EventInstance InteractionAudio(EventInstance interactionInstance, GameObject questObject, int isLooping, bool startEvent)
{
    if (startEvent == true)
    {
        interactionInstance = RuntimeManager.CreateInstance(interactionAudio);
        RuntimeManager.AttachInstanceToGameObject(interactionInstance, questObject.transform);
        interactionInstance.start();
    }

    switch (isLooping)
    {
        case 0:
            interactionInstance.setParameterByName("InteractLooping", 1);
            break;
        case 1:
            interactionInstance.setParameterByName("InteractLooping", 0);
            interactionInstance.release();
            break;
        case 2:
            interactionInstance.setParameterByName("InteractLooping", 0);
            interactionInstance.release();
            interactionInstance.keyOff();
            interactionInstance.stop(STOP_MODE.ALLOWFADEOUT);
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
