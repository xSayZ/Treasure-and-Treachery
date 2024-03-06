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
            [Header("Setup")]
            public Slider progressBar;
            public WorldMapManager worldMapManager;
            
            [Header("Settings")]
            [SerializeField] private float minSceneLoadTime;
            [SerializeField] private float maxSceneLoadTime;
            
            private float actualSceneProgress;
            private float fakeSceneProgress;
            private float fakeSceneLoadTime;
            private AsyncOperation scene;

            public void Start()
            {
                progressBar.value = 0;
                fakeSceneLoadTime = Random.Range(minSceneLoadTime, maxSceneLoadTime);
                LoadSceneAsync(LevelManager.Instance.worldMapManager.levelToLoad);
            }

            private void Update()
            {
                actualSceneProgress = scene.progress;
                fakeSceneProgress += Time.deltaTime / fakeSceneLoadTime;
                
                if (fakeSceneProgress < actualSceneProgress || scene.progress >= 0.9f)
                {
                    progressBar.value = Mathf.MoveTowards(progressBar.value, fakeSceneProgress, 3 * Time.deltaTime);
                }
                else
                {
                    progressBar.value = Mathf.MoveTowards(progressBar.value, actualSceneProgress, 3 * Time.deltaTime);
                }
                
                if (fakeSceneProgress >= 1f && scene.progress >= 0.9f)
                {
                    DoneLoadingScene();
                }
            }

            private async void LoadSceneAsync(LevelDataSO _levelData)
            {
                scene = SceneManager.LoadSceneAsync(_levelData.levelPath, LoadSceneMode.Single);
                
                scene.allowSceneActivation = false;
            }

            private void DoneLoadingScene()
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                
                scene.allowSceneActivation = true;
            }
        }
    }
}