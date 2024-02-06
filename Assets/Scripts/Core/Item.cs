// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-05
// Author: b22alesj
// Description: Item class
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Core {
        public class Item
        {
            public int Weight;
            public float InteractionTime;
            public GameObject Pickup;

            public Item(int _weight, float _interactionTime, GameObject _pickup)
            {
                Weight = _weight;
                InteractionTime = _interactionTime;
                Pickup = _pickup;
            }
        }
    }
}
