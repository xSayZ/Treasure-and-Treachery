// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-05
// Author: b22alesj
// Description: Item class
// --------------------------------
// ------------------------------*/

using Game.Scene;
using UnityEngine;


namespace Game {
    namespace Core {
        public class Item
        {
            public float WeightMultiplier;
            public float InteractionTime;
            public Pickup Pickup;
            public GameObject PickupObject;
            public Sprite Sprite;

            public Item(float _weightMultiplier, float _interactionTime, Pickup _pickup, GameObject _pickupObject, Sprite _sprite)
            {
                WeightMultiplier = _weightMultiplier;
                InteractionTime = _interactionTime;
                Pickup = _pickup;
                PickupObject = _pickupObject;
                Sprite = _sprite;
            }
        }
    }
}