// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-05
// Author: b22alesj
// Description: Interface for interacting
// --------------------------------
// ------------------------------*/


namespace Game {
    namespace Core {
        public interface IInteractable
        {
            public void Interact(int _playerIndex, bool _start);
            
            public void InInteractionRange(int _playerIndex, bool _inRange);
        }
    }
}
