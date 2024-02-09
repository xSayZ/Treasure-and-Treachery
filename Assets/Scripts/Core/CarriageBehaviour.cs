// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22feldy
// Description: Carriage for leaving level
// --------------------------------
// ------------------------------*/

using Game.Backend;
using Game.Core;
using UnityEngine;
using Game.Quest;
using Game.Player;


namespace Game {
    namespace Scenes {
        public class CarriageBehaviour : MonoBehaviour, IInteractable, IDamageable {
            [Header("Setup")]
            [SerializeField] private CarriageData carriageData;
            [SerializeField] private GameObject interactionUI;
            [SerializeField] GameObject playerTeleportPosition;
            
            // Interaction variables
            [HideInInspector] public bool[] CanInteractWith { get; set; }
            [HideInInspector] public bool[] PlayersThatWantsToInteract { get; set; }
            [HideInInspector] public Transform InteractionTransform { get; set; }
            
            [HideInInspector] public int Health { get; set; }
            
            private bool canLeave = true;
            private int playersInCarriage;

#region Unity Functions
            private void OnEnable()
            {
                QuestManager.OnRequiredQuestRegistered.AddListener(RequiredQuestRegistered);
                QuestManager.OnAllRequiredQuestsCompleted.AddListener(AllRequiredQuestsCompleted);
            }
            
            private void OnDisable()
            {
                QuestManager.OnRequiredQuestRegistered.RemoveListener(RequiredQuestRegistered);
                QuestManager.OnAllRequiredQuestsCompleted.RemoveListener(AllRequiredQuestsCompleted);
            }
            
            private void Awake()
            {
                CanInteractWith = new bool[4]; // Hard coded to max 4 players
                for (int i = 0; i < CanInteractWith.Length; i++)
                {
                    CanInteractWith[i] = true;
                }
                PlayersThatWantsToInteract = new bool[4]; // Hard coded to max 4 players
                InteractionTransform = transform;

                Health = carriageData.currentHealth;
            }
#endregion

#region Public Functions
            public void Interact(int _playerIndex, bool _start)
            {
                if (_start && canLeave)
                {
                    CanInteractWith[_playerIndex] = false;
                    
                    GameObject player = GameManager.Instance.activePlayerControllers[_playerIndex];
                    player.GetComponent<PlayerController>().SetInputPausedState(true);
                    player.transform.position = playerTeleportPosition.transform.position;
                    player.transform.localScale = new Vector3(0,0,0);
                    
                    playersInCarriage++;
                    if (playersInCarriage >= GameManager.Instance.activePlayerControllers.Count)
                    {
                        // All players are in carriage, time to end level
                        Debug.Log("Level Done");
                    }
                }
            }
            
            public void ToggleInteractionUI(int _playerIndex, bool _active)
            {
                PlayersThatWantsToInteract[_playerIndex] = _active;

                bool _displayUI = false;
                for (int i = 0; i < PlayersThatWantsToInteract.Length; i++)
                {
                    if (PlayersThatWantsToInteract[i])
                    {
                        _displayUI = true;
                    }
                }
                
                interactionUI.SetActive(_displayUI);
            }
            
            public void Death()
            {
                carriageData.currentHealth = 0;
                
                // Carriage was destroyed, level lost
                Debug.Log("Carriage destroyed, you lost");
            }
            
            public void DamageTaken()
            {
                carriageData.currentHealth = Health;
            }
#endregion

#region Private Functions
            private void RequiredQuestRegistered()
            {
                canLeave = false;
                
                for (int i = 0; i < CanInteractWith.Length; i++)
                {
                    CanInteractWith[i] = false;
                }
            }
            
            private void AllRequiredQuestsCompleted()
            {
                canLeave = true;
                
                for (int i = 0; i < CanInteractWith.Length; i++)
                {
                    CanInteractWith[i] = true;
                }
            }
#endregion
        }
    }
}
