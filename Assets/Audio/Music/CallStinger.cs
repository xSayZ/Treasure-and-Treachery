// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-06
// Author: johan
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Audio {
        public class CallStinger : MonoBehaviour
        {
        

#region Unity Functions
            // Start is called before the first frame update
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
                
            }
#endregion

#region Public Functions

        public void CallObjectiveStinger()
        {
            AudioMananger.Instance.CompleteObjectiveStinger();
            
        }

        public void CallFXObjStinger()
        {
            AudioMananger.Instance.FXObjStinger();
            
        }

        public void CallVictoryStinger()
        {
            AudioMananger.Instance.VictoryStinger();
            
        }

        public void CallWaveStinger()
        {
            AudioMananger.Instance.WaveStinger();
            
        }

#endregion

#region Private Functions

#endregion
        }
    }
}
