// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-19
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Game {
    namespace Managers {
        public class CustomSceneManager : Singleton<CustomSceneManager>
        {
            private AsyncOperation asyncLoad;
            private bool bLoadDone;
            
            IEnumerator LoadSceneAsync()
            {
                asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
                asyncLoad.allowSceneActivation = false;
                
                while (!asyncLoad.isDone)
                {
                    //scene has loaded as much as possible,
                    // the last 10% can't be multi-threaded
                    if (asyncLoad.progress >= 0.9f)
                    {
                        asyncLoad.allowSceneActivation = true;
                    }
                    yield return null;
                }
                bLoadDone = asyncLoad.isDone;
                
            }

            public void ChangeScene()
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
            }
            
            public void ChangeToLoadingScene()
            {
                StartCoroutine(LoadSceneAsync());
            }
            
            

        }
    }
}
