// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-29
// Author: johan
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using Game.Quest;
using UnityEngine;


namespace Game {
    namespace Audio {
        public class LeadingPlayerSetParam : MonoBehaviour
        {

            private int leadingPlayer;
            
#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                leadingPlayer = QuestManager.IndexOfLeadingPlayer;

            }
    
            // Update is called once per frame
            void Update()
            {
                if (leadingPlayer == 0)
                {
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic, "MusicProg", 2f, false,false);
                    Debug.Log("Leadingplayer was run on Dragon");
                    
                }

                if (leadingPlayer == 1)
                {
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic, "MusicProg", 3f, false,false);
                    Debug.Log("Leadingplayer was run on Dragon");
                }

                if (leadingPlayer == 2)
                {
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic, "MusicProg", 4f, false,false);
                    Debug.Log("Leadingplayer was run on Witch");
                }

                if (leadingPlayer == 3)
                {
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic, "MusicProg", 5f, false,false);
                    Debug.Log("Leadingplayer was run On Gorgon");
                }
            }
#endregion

#region Public Functions

#endregion

#region Private Functions

#endregion
        }
    }
}
