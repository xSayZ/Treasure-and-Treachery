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
            public Sprite Sprite;

            public Item(int _weight, float _interactionTime, GameObject _pickup, Sprite _sprite)
            {
                Weight = _weight;
                InteractionTime = _interactionTime;
                Pickup = _pickup;
                Sprite = _sprite;
            }
        }
    }
}
