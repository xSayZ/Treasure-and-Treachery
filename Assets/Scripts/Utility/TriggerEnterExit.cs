// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-30
// Author: alexa
// Description: Triggers a unity event when an object enters or exits a collider
// --------------------------------
// ------------------------------*/

using UnityEngine;
using UnityEngine.Events;


namespace Game {
    namespace Other {
        public class TriggerEnterExit : MonoBehaviour
        {
            [SerializeField] private UnityEvent<Transform> triggerEnter;
            [SerializeField] private UnityEvent<Transform> triggerExit;
            
            private void OnTriggerEnter(Collider other)
            {
                if (!other.isTrigger)
                {
                    triggerEnter.Invoke(other.transform);
                }
            }
            
            private void OnTriggerExit(Collider other)
            {
                if (!other.isTrigger)
                {
                    triggerExit.Invoke(other.transform);
                }
            }
        }
    }
}
