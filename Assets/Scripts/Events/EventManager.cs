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
            public static UnityEvent<bool, int> OnObjectivePickup = new UnityEvent<bool, int>();
            public static UnityEvent<int,int> OnCurrencyPickup = new UnityEvent<int,int>();
            public static UnityEvent<int,int> OnHealthChange = new UnityEvent<int,int>();
        }
    }
}
