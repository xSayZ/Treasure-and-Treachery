// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-19
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections;

using Game.WorldMap;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Game {
    namespace Managers {
        public class LevelManager : Singleton<LevelManager> {
            
            
            [SerializeField] private string loadingScreenPath = "LoadingScreen";
            public WorldMapManager worldMapManager;
            private static WorldMapManager _manager;

            protected override void SingletonAwakened()
            {
                if (FindObjectsByType<LevelManager>(FindObjectsSortMode.None).Length > 1)
                {
                    Destroy(gameObject);
                }
                
                DontDestroyOnLoad(gameObject);
                
                base.SingletonAwakened();
                DontDestroyOnLoad(this);
            }

            public void LoadLevel(LevelDataSO _levelData)
            {
                StartCoroutine(LoadLoadingScreen(_levelData));
            }

            public void ReloadLevel()
            {
                LoadLevel(worldMapManager.levelToLoad);
            }
            
            private IEnumerator LoadLoadingScreen(LevelDataSO _levelData)
            {
               
                worldMapManager.levelToLoad = null;
                yield return new WaitForSeconds(0.1f);
                worldMapManager.levelToLoad = _levelData;
                Debug.Log(worldMapManager.levelToLoad.levelPath);
                SceneManager.LoadScene(loadingScreenPath,LoadSceneMode.Single);
                
            }
            
            public void LoadScoreScreen()
            {
                SceneManager.LoadScene("ScoreScreen",LoadSceneMode.Single);
            }
        }
    }
}
