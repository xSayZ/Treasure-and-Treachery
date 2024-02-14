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

                
                if (timer >= 60)
                {
                    AudioMananger.Instance.SetMusicParam("MusicProg", 2f, false);
                }
                
                if (timer >= 120)
                {
                    AudioMananger.Instance.SetMusicParam("MusicProg", 3f, false);
                }

                if (timer >= 180)
                {
                    AudioMananger.Instance.SetMusicParam("MusicProg", 4f, false);
                }

                if (timer >= 240)
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
