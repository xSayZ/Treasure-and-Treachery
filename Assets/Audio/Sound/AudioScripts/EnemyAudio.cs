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
            // Debug.Log("Spirit have an event");
            spiritStateInstance.start();
            break;
        //Roam State
        case 0:
            spiritStateInstance.setParameterByName("SpiritState", 0);
            spiritStateInstance.release();
            spiritStateInstance.start();
            break;
        //Alert State
        case 1:
            spiritStateInstance.setParameterByName("SpiritState", 1);
            spiritStateInstance.release();
            break;
        //Chase
        case 2:
            spiritStateInstance.setParameterByName("SpiritState", 2);
            spiritStateInstance.release();
            break;
        //Death
        case 3:
            spiritStateInstance.setParameterByName("SpiritState", 3);
            spiritStateInstance.release();
            break;
        case 6:
            spiritStateInstance.setParameterByName("SpiritState", 4);
            spiritStateInstance.release();
            break;
    }

    if (enemyObj.activeInHierarchy == false)
    {
        spiritStateInstance.release();
    }
    
    return spiritStateInstance;
}
#endregion

#region Private Functions

#endregion
        }
    }
}
