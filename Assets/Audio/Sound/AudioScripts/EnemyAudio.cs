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
        
        [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Enemy")]
        public class EnemyAudio : ScriptableObject
        {
            [Header("Enemy States")]
            [SerializeField] 
            private EventReference enemyRoamAudio;
            [SerializeField] 
            private EventReference enemyAlertAudio;
            [SerializeField]
            private EventReference spiritStateAudio;


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

public void EnemyRoamAudio(GameObject enemyObj)
{
    EventInstance enemyRoamInstance = RuntimeManager.CreateInstance(enemyRoamAudio);
    RuntimeManager.AttachInstanceToGameObject(enemyRoamInstance, enemyObj.transform);
    enemyRoamInstance.start();
    enemyRoamInstance.release();
}

public void EnemyAlertAudio(GameObject enemyObj)
{
    EventInstance enemyAlertInstance = RuntimeManager.CreateInstance(enemyAlertAudio);
    RuntimeManager.AttachInstanceToGameObject(enemyAlertInstance, enemyObj.transform);
    enemyAlertInstance.start();
    enemyAlertInstance.release();
}


public EventInstance SpiritStateAudioUpdate(GameObject enemyObj, EventInstance spiritStateInstance, int spiritAudioParam)
{
    switch (spiritAudioParam)
    {
        //När Spirit spawnar startar vi eventet och attatchar det.
        case 5:
            spiritStateInstance = RuntimeManager.CreateInstance(spiritStateAudio);
            RuntimeManager.AttachInstanceToGameObject(spiritStateInstance, enemyObj.transform);
            spiritStateInstance.start();
            Debug.Log("Spirit have an event");
            break;
        //Roam State
        case 0:
            Debug.Log("Spirit State Param Changed To: " + spiritAudioParam);
            spiritStateInstance.setParameterByName("SpiritState", 0);
            break;
        //Alert State
        case 1:
            Debug.Log("Spirit State Param Changed To: " + spiritAudioParam);
            spiritStateInstance.setParameterByName("SpiritState", 1);
            break;
        //Chase
        case 2:
            Debug.Log("Spirit State Param Changed To: " + spiritAudioParam);
            spiritStateInstance.setParameterByName("SpiritState", 2);
            break;
        //Death
        case 3:
            Debug.Log("Spirit State Param Changed To: " + spiritAudioParam);
            spiritStateInstance.setParameterByName("SpiritState", 3);
            spiritStateInstance.release();
            break;
    }
    
    return spiritStateInstance;
}
#endregion

#region Private Functions

#endregion
        }
    }
}
