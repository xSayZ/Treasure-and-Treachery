// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-27
// Author: Felix
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Game {
    namespace Voting {
        public class VotingButton : MonoBehaviour
        {
            [SerializeField] private List<Image> playerIndicators;
            private void Start()
            {
                foreach (Image playerIndicator in playerIndicators)
                {
                    playerIndicator.gameObject.SetActive(false);
                }
            }
            
            /// <summary>
            /// Sets the PlayerIndicator to be active or not
            /// </summary>
            /// <param name="playerIndex">The unique identifier for the player.</param>
            /// <param name="active">The bool set for the gameObject to be on or off</param>
            public void SetPlayerIndicator(int playerIndex, bool active)
            {
                playerIndicators[playerIndex].gameObject.SetActive(active);
            }
            
            public void TurnOffAllPlayerIndicators()
            {
                foreach (Image playerIndicator in playerIndicators)
                {
                    playerIndicator.gameObject.SetActive(false);
                }
            }
            
            
        }
    }
}
