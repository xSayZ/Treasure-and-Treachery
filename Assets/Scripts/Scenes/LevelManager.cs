// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-19
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;


namespace Game {
    namespace Managers {
        public class LevelManager : Singleton<LevelManager>
        {
            public static string nextLevel { get; private set; }
            
            private float target;
            [SerializeField] private string loadingScreenPath = "LoadingScreen";
            public void LoadLoadingScreen(string level)
            {
               
               SceneManager.LoadScene(loadingScreenPath,LoadSceneMode.Additive);
               nextLevel = level;
               
            }
            

            public void LoadGameplayScene(int level)    
            {
                SceneManager.LoadScene(level, LoadSceneMode.Single);
            }
            
            public void LoadScoreScreen()
            {
                SceneManager.LoadSceneAsync("ScoreScreen");
            }
        }
    }
}
