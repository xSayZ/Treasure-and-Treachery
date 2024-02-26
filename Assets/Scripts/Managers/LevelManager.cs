// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-19
// Author: c21frejo
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine.SceneManagement;


namespace Game {
    namespace Managers {
        public class LevelManager : Singleton<LevelManager>
        {
            public static int nextLevel { get; private set; }
            private float target;
            public void LoadScene(int level)
            {
               SceneManager.LoadScene("LoadingScreen",LoadSceneMode.Single);
               nextLevel = level;

            }
            

            public void LoadGameplayScene(int level)    
            {
                SceneManager.LoadScene(level, LoadSceneMode.Single);
            }
            
            public void LoadScoreScreen()
            {
                SceneManager.LoadScene("ScoreScreen");
            }
        }
    }
}
