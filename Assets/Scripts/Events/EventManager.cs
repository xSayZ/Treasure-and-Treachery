// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Event Manager
// --------------------------------
// ------------------------------*/

using UnityEngine.Events;


namespace Game {
    namespace Events {
        public class EventManager
        {
            public static UnityEvent<bool> OnObjectivePickup = new UnityEvent<bool>();
        }
    }
}
