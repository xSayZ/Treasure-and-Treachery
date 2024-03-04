// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-19
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections;
using Game.Backend;
using Game.UI;
using Game.WorldMap;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Game {
    namespace Managers {
        public class LevelManager : Singleton<LevelManager> {
            
            
            [SerializeField] private string loadingScreenPath = "LoadingScreen";
            public WorldMapManager worldMapManager;
            
            public void LoadLevel(LevelDataSO _levelData) {
                
                StartCoroutine(LoadLoadingScreen(_levelData));
            }
            
            private IEnumerator LoadLoadingScreen(LevelDataSO _levelData) {
                worldMapManager.levelToLoad = null;
                yield return new WaitForSeconds(0.1f);
                worldMapManager.levelToLoad = _levelData;
                SceneManager.LoadScene(loadingScreenPath,LoadSceneMode.Additive);
            }
        }
    }
}
