// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;
using Game.Events;
using Game.Player;
using System;

namespace Game {
    namespace Scene {
        public class Pickup : MonoBehaviour
        {
            public enum PickupType
            {
                Gold,
                Objective
            }

            public PickupType pickupType;
            
            [HideInInspector] private int amount;
            [HideInInspector] private int weight;

#region Unity Functions

            private void OnTriggerEnter(Collider other)
            {
                if (other.CompareTag("Player"))
                {

                    switch (pickupType) {
                        case PickupType.Objective:
                            EventManager.OnObjectivePickup.Invoke(true, other.gameObject.transform.GetComponent<PlayerController>().PlayerID);
                            break;
                        case PickupType.Gold:
                            EventManager.OnCurrencyPickup.Invoke(amount,other.gameObject.transform.GetComponent<PlayerController>().PlayerID);
                            break;
                    }

                    // TODO: Player needs to know that the shovel has been picked up and that its on that specific player.
                    gameObject.SetActive(false);
                    gameObject.transform.SetParent(other.transform);
                }
            }
#endregion

#region Public Functions

            public int Amount {
                get {
                    return this.amount;
                } set {
                    this.amount = value;
                }
            }

            public int Weight {
                get {
                    return this.weight;
                } set {
                    this.weight = value;
                }
            }

#endregion
        }
    }
}
