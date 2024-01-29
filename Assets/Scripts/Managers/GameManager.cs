// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using Game.Player;


namespace Game {
    namespace Backend {
        public class GameManager : MonoBehaviour
        {
            [SerializeField] private List<PlayerController> activePlayerControllers;



            #region Unity Functions
            void Awake()
            {
                players = new List<PlayerController>();
            }
            void Start()
            {
                
            }
    
            // Update is called once per frame
            void Update()
            {
                
            }
#endregion

#region Public Functions

#endregion

#region Private Functions

#endregion
        }
    }
}
