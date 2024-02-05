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
            private EventReference enemyChaseAudio;


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

public void EnemyChaseAudio(GameObject enemyObj)
{
    EventInstance enemyChaseInstance = RuntimeManager.CreateInstance(enemyChaseAudio);
    RuntimeManager.AttachInstanceToGameObject(enemyChaseInstance, enemyObj.transform);
    enemyChaseInstance.start();
    enemyChaseInstance.release();
}
#endregion

#region Private Functions

#endregion
        }
    }
}
