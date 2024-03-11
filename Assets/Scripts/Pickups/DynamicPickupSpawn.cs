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
        public class DynamicPickupSpawn : MonoBehaviour
        {
            public List<GameObject> pickups;

            private void Start() {
                var playerCount = GameManager.Instance.ActivePlayerControllers.Count;
                for (int i = 0; i < pickups.Count; i++) {
                    if (i > playerCount - 1) {
                        pickups[i].SetActive(false);
                    }
                }
            }
        }
    }
}
