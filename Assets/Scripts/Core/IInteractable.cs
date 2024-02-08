// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-05
// Author: b22alesj
// Description: Interface for interacting
// --------------------------------
// ------------------------------*/


using UnityEngine;
namespace Game {
    namespace Core {
        public interface IInteractable
        {
            public bool[] CanInteractWith { get; set; }
            public bool[] PlayersThatWantsToInteract { get; set; }
            public Transform InteractionTransform { get; set; }
            
            public void Interact(int _playerIndex, bool _start);
            
            public void ToggleInteractionUI(int _playerIndex, bool _active);
        }
    }
}
