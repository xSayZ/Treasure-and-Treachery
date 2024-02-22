// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-22
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Threading.Tasks;
using Game.Managers;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Game {
    namespace UI {
        public class LoadingScreen : MonoBehaviour
        {
            
            public GameObject loadingScreen;
            public Image progressBar;

            private float target;
            public void Start()
            {
                loadingScreen.SetActive(true);
                LoadSceneAsync();

            }
            
            private async void LoadSceneAsync()
            {
                target = 0;
                progressBar.fillAmount = 0;
                AsyncOperation scene = SceneManager.LoadSceneAsync(LevelManager.nextLevel);
                Debug.Log(scene);
                scene.allowSceneActivation = false;
                loadingScreen.SetActive(true);
                do
                {
                    await Task.Delay(100);
                    target = scene.progress;
                    
                } while (scene.progress <0.9f);
                await Task.Delay(900);

                scene.allowSceneActivation = true;
                
            }

            private void Update()
            {
                progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 3 * Time.deltaTime);
            }
        }
    }
}
