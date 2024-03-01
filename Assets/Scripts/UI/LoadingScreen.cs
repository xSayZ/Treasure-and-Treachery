// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-22
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Threading.Tasks;
using Game.Backend;
using Game.Managers;
using Game.WorldMap;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Game {
    namespace UI {
        public class LoadingScreen : MonoBehaviour
        {
            
            public GameObject loadingScreen;
            public Image progressBar;
            public WorldMapManager worldMapManager;

            private float target;
            public void Start()
            {
                LoadSceneAsync(worldMapManager.levelToLoad);
            }
            
            private async void LoadSceneAsync(LevelDataSO _levelData)
            {
                target = 0;
                progressBar.fillAmount = 0;
                
                AsyncOperation scene = SceneManager.LoadSceneAsync(_levelData.levelName ,LoadSceneMode.Single);

                Debug.Log(scene);
                scene.allowSceneActivation = false;
                loadingScreen.SetActive(true);
                do
                {
                    target = scene.progress;
                    
                } while (scene.progress <0.9f);

                await Task.Delay(600);
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());


                scene.allowSceneActivation = true;
                
            }

            private void Update()
            {
                progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 3 * Time.deltaTime);
            }
        }
    }
}
