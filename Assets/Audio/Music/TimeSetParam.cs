// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-12
// Author: johan
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using Game.Backend;
using UnityEngine;


namespace Game {
    namespace Audio {
        public class TimeSetParam : MonoBehaviour
        {

            private float timer;
            

#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
                timer = GameManager.Instance.timer.GetCurrentTime();
                Debug.Log("time is" + timer);

                
                if (timer >= 4)
                {
                    AudioMananger.Instance.SetMusicParam("MusicProg", 2f, false);
                }
                
                if (timer >= 8)
                {
                    AudioMananger.Instance.SetMusicParam("MusicProg", 3f, false);
                }

                if (timer >= 12)
                {
                    AudioMananger.Instance.SetMusicParam("MusicProg", 4f, false);
                }

                if (timer >= 16)
                {
                    AudioMananger.Instance.SetMusicParam("MusicProg", 5f, false);
                }
                
                
            }
#endregion

#region Public Functions

#endregion

#region Private Functions

        private void SetParamByTime()
        {
            


        }

#endregion
        }
    }
}
