// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Scenes {

        public enum Type
        {
            Main_Menu,
            Pause_Menu
        }

        [CreateAssetMenu(fileName = "NewMenu", menuName = "Scene Data/Menu")]
        public class Menu : GameScene
        {
            //Settings specific to menu only
            [Header("Menu specific")]
            public Type type;
        }
    }
}
