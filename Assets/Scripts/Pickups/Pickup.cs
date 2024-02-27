// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-02
// Author: alexa
// Description: Item that can be picked up
// --------------------------------
// ------------------------------*/

using System;
using Game.Audio;
using Game.Backend;
using Game.Core;
using Game.Player;
using Game.Quest;
using UnityEngine;
using UnityEngine.Events;


namespace Game {
    namespace Scene {
        public class Pickup : MonoBehaviour, IInteractable
        {
            public enum PickupTypes
            {
                QuestItem,
                Gold,
                MeleeWeapon,
                RangedWeapon
            }
            
            [Header("Setup")]
            [SerializeField] private GameObject interactionUI;
            
            [Header("Pickup Type")]
            public PickupTypes PickupType;
            
            [Header("Pickup Events")]
            [SerializeField] private UnityEvent itemPickedUpFirstTime = new UnityEvent();
            
            [Header("Audio")] 
            [SerializeField] private InteractablesAudio interactablesAudio;
            
            // Interaction variables
            [HideInInspector] public bool[] CanInteractWith { get; set; }
            [HideInInspector] public bool[] PlayersThatWantsToInteract { get; set; }
            [HideInInspector] public Transform InteractionTransform { get; set; }
            
            // Item variables
            [HideInInspector] public int Weight;
            [HideInInspector] public float InteractionTime;
            [HideInInspector] public Sprite ItemSprite;
            
            // Gold variables
            [HideInInspector] public int Amount;

            private Item item;
            private bool hasBeenPickedUpOnce;

#region Unity Functions
            private void Awake()
            {
                CanInteractWith = new bool[4]; // Hard coded to max 4 players
                for (int i = 0; i < CanInteractWith.Length; i++)
                {
                    CanInteractWith[i] = true;
                }
                PlayersThatWantsToInteract = new bool[4]; // Hard coded to max 4 players
                InteractionTransform = transform;
                
                CreateItem();
            }
#endregion

#region Public Functions
            public void Interact(int _playerIndex, bool _start)
            {
                if (!_start)
                {
                    return;
                }
                
                switch (PickupType)
                {
                    case PickupTypes.QuestItem:
                        if (!GameManager.Instance.ActivePlayerControllers[_playerIndex].PlayerData.canPickUp)
                        {
                            return;
                        }
                        QuestManager.OnItemPickedUp.Invoke(_playerIndex, item);
                        TriggerPickUpEvent();
                        break;
                        
                    case PickupTypes.Gold:
                        QuestManager.OnGoldPickedUp.Invoke(_playerIndex, Amount);
                        TriggerPickUpEvent();
                        
                        try
                        {
                            interactablesAudio.GoldPickupAudio(gameObject);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("[{Pickup}]: Error Exception " + e);
                        }
                        
                        Destroy(gameObject);
                        break;
                    
                    case PickupTypes.MeleeWeapon:
                        QuestManager.OnMeleeWeaponPickedUp.Invoke(_playerIndex);
                        TriggerPickUpEvent();
                        Destroy(gameObject);
                        break;
                    
                    case PickupTypes.RangedWeapon:
                        QuestManager.OnRagedWeaponPickedUp.Invoke(_playerIndex);
                        TriggerPickUpEvent();
                        Destroy(gameObject);
                        break;
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

            public Item GetItem()
            {
                return item;
            }
#endregion

#region Private Functions
            private void CreateItem()
            {
                item = new Item(Weight, InteractionTime, gameObject, ItemSprite);
            }

            private void TriggerPickUpEvent()
            {
                if (!hasBeenPickedUpOnce)
                {
                    hasBeenPickedUpOnce = true;
                    itemPickedUpFirstTime.Invoke();
                }
            }
#endregion
        }
    }
}
