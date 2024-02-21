// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-19
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Task = System.Threading.Tasks.Task;


namespace Game {
    namespace Managers {
        public class LevelManager : Singleton<LevelManager>
        {
            [SerializeField] private GameObject loaderCanvas;
            [SerializeField] private Image progressBar;


            public void Start()
            {
                DontDestroyOnLoad(gameObject);
            }

            private float target;
            public async void LoadSceneAsync(int level)
            {
                target = 0;
                progressBar.fillAmount = 0;
                AsyncOperation scene = SceneManager.LoadSceneAsync(level);
                scene.allowSceneActivation = false;
                loaderCanvas.SetActive(true);
                do
                {
                    await Task.Delay(100);

                    target = scene.progress;
                } while (scene.progress <0.9f);
                await Task.Delay(1000);

                scene.allowSceneActivation = true;
                loaderCanvas.SetActive(false);
                
            }

            public void LoadGameplayScene(int level)    
            {
                SceneManager.LoadScene(level, LoadSceneMode.Single);
            }
            
            public void LoadScoreScreen()
            {
                SceneManager.LoadScene("ScoreScreen");
            }

            public void Update()
            {
                progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 3 * Time.deltaTime);
            }
        }
    }
}
