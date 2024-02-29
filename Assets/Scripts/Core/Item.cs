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
            public float WeightMultiplier;
            public float InteractionTime;
            public GameObject Pickup;
            public Sprite Sprite;

            public Item(float _weightMultiplier, float _interactionTime, GameObject _pickup, Sprite _sprite)
            {
                WeightMultiplier = _weightMultiplier;
                InteractionTime = _interactionTime;
                Pickup = _pickup;
                Sprite = _sprite;
            }
        }
    }
}
