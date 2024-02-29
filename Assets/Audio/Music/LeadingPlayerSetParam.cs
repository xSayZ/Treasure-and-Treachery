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

            
#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
                if (QuestManager.IndexOfLeadingPlayer == 0)
                {
                    AudioMananger.Instance.SetParameterMusicEvent(EventsToBePlayed.GamePlayMusic, "AvatarProg", 2f, false,false);
                    Debug.Log("Leadingplayer was run");
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
