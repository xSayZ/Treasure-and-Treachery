// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-11
// Author: b22feldy
// Description: Treasure and Treachery
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using Game.Backend;
using UnityEngine;


namespace Game {
    namespace Scene {
        public class DynamicObjectSpawn : MonoBehaviour
        {
            public List<GameObject> objects;

            private void Start() {
                var playerCount = GameManager.Instance.ActivePlayerControllers.Count;
                for (int i = 0; i < objects.Count; i++) {
                    if (i > playerCount - 1) {
                        objects[i].SetActive(false);
                    }
                }
            }
        }
    }
}
