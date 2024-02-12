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
    //deklarerar en lokal variabel av typen eventinstance och initialiserar denna med referens till innehållet i vår eventreferece "playermeeleattack"
    EventInstance playerMeleeAttackInstance = RuntimeManager.CreateInstance(playerMeleeAttack);
        
    //fäster eventinstance till ett specefikt gameobjekts transform och rigidbody-komponent. gör så att ljudet spelas upp på rätt position.
    RuntimeManager.AttachInstanceToGameObject(playerMeleeAttackInstance, meleeObj.transform, meleeObj.GetComponent<Rigidbody>());

    // Spelar upp Eventet från FMOD
    playerMeleeAttackInstance.start();

    // Stänger Eventinstansens resurser
    playerMeleeAttackInstance.release();
}

// spelar upp fotsteg efter en raycast skjuts ut och kollar taggen på marken under. Detta sker i playercontroller-scriptet.
// Taggen matas in som en string och byter parameter i scriptet nedan
public void PlayerFootstepPlay(GameObject footObj, string surface)
{
    EventInstance playerFootstepInstance = RuntimeManager.CreateInstance(playerFootStep);
    RuntimeManager.AttachInstanceToGameObject(playerFootstepInstance, footObj.transform);

    //switch (surface)
    //{
    //    case "Grass":
   //         playerFootstepInstance.setParameterByName("Surface", 0f);
  //          break;
  //      case "Rock":
  //          playerFootstepInstance.setParameterByName("Surface", 1f);
  //          break;
  //      case "Metal":
   //         playerFootstepInstance.setParameterByName("Surface", 2f);
    //        break;
  //  }
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
