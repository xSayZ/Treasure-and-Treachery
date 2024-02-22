// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-21
// Author: b22alesj
// Description: Base class for player abilities
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace Player {
        public abstract class PlayerAbilityBehaviour : MonoBehaviour
        {
            protected PlayerController playerController;

            public void SetupBehaviour(PlayerController _playerController)
            {
                playerController = _playerController;
                playerController.PlayerMovementBehaviour.OnDashKill.AddListener(OnDashKill);
                Setup();
            }

            protected virtual void Setup(){}

            public virtual void OnDisable()
            {
                if (playerController)
                {
                    playerController.PlayerMovementBehaviour.OnDashKill.RemoveListener(OnDashKill);
                }
            }

            protected abstract void OnDashKill();
        }
    }
}