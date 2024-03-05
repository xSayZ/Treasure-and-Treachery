// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-22
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Threading.Tasks;
using Game.Managers;
using Game.WorldMap;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Game {
    namespace UI {
        public class LoadingScreen : MonoBehaviour
        {
            public GameObject loadingScreen;
            public Slider progressBar;
            public WorldMapManager worldMapManager;
            
            private float sceneProgress;

            public void Start()
            {
                LoadSceneAsync(LevelManager.Instance.worldMapManager.levelToLoad);
            }

            private async void LoadSceneAsync(LevelDataSO _levelData)
            {
                sceneProgress = 0;
                progressBar.value = 0;
                
                AsyncOperation scene = SceneManager.LoadSceneAsync(_levelData.levelName ,LoadSceneMode.Single);
                
                Debug.Log(scene);
                scene.allowSceneActivation = false;
                loadingScreen.SetActive(true);
                do
                {
                    sceneProgress = scene.progress;
                    
                } while (scene.progress <0.9f);
                
                await Task.Delay(600);
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                
                scene.allowSceneActivation = true;
            }

            private void Update()
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, sceneProgress, 3 * Time.deltaTime);
            }
        }
    }
}