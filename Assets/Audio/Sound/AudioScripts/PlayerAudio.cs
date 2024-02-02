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

public void MeleeAudioPlay(GameObject meleeObj, int comboValue)
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
#endregion

#region Private Functions

#endregion
        }
    }
}
