// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-05
// Author: b22alesj
// Description: Item class
// --------------------------------
// ------------------------------*/


namespace Game {
    namespace Core {
        public class Item
        {
            public int Weight;
            public float InteractionTime;

            public Item(int _weight, float _interactionTime)
            {
                Weight = _weight;
                InteractionTime = _interactionTime;
            }
        }
    }
}
