// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/07
// Author: herman
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using Game.Backend;
using Game.Core;
using Game.Player;
using Game.Quest;
using Random = UnityEngine.Random;
using STOP_MODE = FMOD.Studio.STOP_MODE;


namespace Game {
    namespace Audio {
        [CreateAssetMenu(menuName = "ScriptableObjects/Audio/Player/Dialogue")]
        public class DialogueAudio : ScriptableObject
        {
            private Dictionary<int, EventInstance> currentDialogues = new Dictionary<int, EventInstance>();
            //EventInstances
            private EventInstance bajsInstance, shovelPickupInstance, goldPickupInstance, shovelReactInstance, deathDialogueInstance, damageAudioInstance, goldReactInstance, playerAttackAudioInstance, deathReactioninstance, cartEnterInstance;
            
            //EventReferences
            [SerializeField]
            private EventReference shovelPickup, goldPickupAudio, goldReactAudio, objectiveProgressionReaction, deathAudio, damageAudio, playerAttackAudio, deathReactAudio, cartEnterAudio, returnCartAudio, wolfAttackRef, dragonAttackRef, witchAttackRef, gorgonAttackRef;

            [Header("QuestDialogue")] 
            
            public EventReference logicalThinkingAudio;
            public EventInstance logicalThinkingInstance;
            

            private void OnEnable()
            {
                QuestManager.OnGoldPickedUp.AddListener(GoldPickupDialogue); 
                QuestManager.OnItemPickedUp.AddListener(ShovelPickupAudio);
                GameManager.OnPlayerDeath.AddListener(PlayerDeathDialogue);
            }

            private void OnDisable()
            {
                QuestManager.OnGoldPickedUp.RemoveListener(GoldPickupDialogue); 
                QuestManager.OnItemPickedUp.RemoveListener(ShovelPickupAudio);
                GameManager.OnPlayerDeath.RemoveListener(PlayerDeathDialogue);
            }

#region Public Functions
            public void PlayerDamageAudio(int _playerID)
            {
                try
                {
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var damageAudioInstance = RuntimeManager.CreateInstance(damageAudio);
                    PlayDialogue(_playerID, damageAudioInstance, true);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            public void PlayerCartEnterDialogue(int PlayerID)
            {
                try
                {
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var cartEnterInstance = RuntimeManager.CreateInstance(cartEnterAudio);
                    //PlayDialogue(_playerID, cartEnterInstance);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[{DialogueAudio}]: Error Exception " + e);
                }
            }
            public void UpdateWereWolfRageAudio(float rageAmount)
            {
                playerAttackAudioInstance.setParameterByName("WolfRage", rageAmount);
            }
            public void PlayerAttackAudio(int _playerID)
            {
                try
                {
                    switch (_playerID)
                    {
                        case 0:
                            playerAttackAudioInstance = RuntimeManager.CreateInstance(wolfAttackRef);
                            break;
                        case 1:
                            playerAttackAudioInstance = RuntimeManager.CreateInstance(dragonAttackRef);
                            break;
                        case 2:
                            playerAttackAudioInstance = RuntimeManager.CreateInstance(witchAttackRef);
                            break;
                        case 3:
                            playerAttackAudioInstance = RuntimeManager.CreateInstance(gorgonAttackRef);
                            break;
                    }
                    PlayDialogue(_playerID, playerAttackAudioInstance, false);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            private void PlayerDeathDialogue(int _playerID)
            {
                try
                {
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var deathDialogueInstance = RuntimeManager.CreateInstance(deathAudio);
                    PlayDialogue(_playerID, deathDialogueInstance, true);
                
                    if (_players.Count > 1)
                    {
                        DialogueAudioWrapper.Instance.PlayResponseDialogue(bajsInstance, _playerID, _players, "death");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            private void ShovelPickupAudio(int _playerID, Item _item)
            {
                try
                {
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var shovelPickupInstance = RuntimeManager.CreateInstance(shovelPickup);
                    PlayDialogue(_playerID, shovelPickupInstance, true);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            private void ObjectiveProgressionReactionAudio(int _playerID)
            {
                try
                {
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var shovelReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
                    PlayDialogue(_playerID, shovelReactInstance, true);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[{DialogueAudio}]: Error Exception " + e);
                }
            }
            private void GoldPickupDialogue(int _playerID, int amount)
            {
                try
                {
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var goldPickupInstance = RuntimeManager.CreateInstance(goldPickupAudio);
                    PlayDialogue(_playerID, goldPickupInstance, true);
                    
                    if (_players.Count > 1)
                    {
                        DialogueAudioWrapper.Instance.PlayResponseDialogue(goldPickupInstance, _playerID, _players, "gold");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            private void GoldPickupReaction(int _playerID)
            {
                try
                {
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var goldReactInstance = RuntimeManager.CreateInstance(goldReactAudio);
                    PlayDialogue(_playerID, goldReactInstance, true);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            private void DeathReactionAudio(int _playerID)
            {
                try
                {
                    Debug.Log("player " + _playerID + " has something to say");
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var deathReactioninstance = RuntimeManager.CreateInstance(deathReactAudio);
                    PlayDialogue(_playerID, deathReactioninstance, true);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            public void ReturnToCartDialogue(int _playerID)
            {
                try
                {
                    EventInstance returnToCartInstance = RuntimeManager.CreateInstance(returnCartAudio);
                   PlayDialogue(_playerID, returnToCartInstance, true); 
                   
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[{DialogueAudio}]: Error Exception " + e);
                }
                
            }

            #endregion

#region Private Functions
            private void PlayDialogue(int characterID, EventInstance dialogueInstance, bool cutOff)
            {
                // Check if there's already a dialogue instance playing for this character
                if (currentDialogues.ContainsKey(characterID) && cutOff == true) {
                    // If so, stop the current dialogue instance
                    StopDialogue(characterID);
                }

                // Attach instance to character GameObject and start playing
                var player = GameManager.Instance.ActivePlayerControllers[characterID];
                RuntimeManager.AttachInstanceToGameObject(dialogueInstance, player.gameObject.transform);
                dialogueInstance.setParameterByName("SpeakerCharacter", characterID);
                dialogueInstance.start();
                dialogueInstance.release();
                // Update the dictionary with the new dialogue instance
                currentDialogues[characterID] = dialogueInstance;
            }
            
            private void PlayLongDialogue(int characterID, EventInstance dialogueInstance)
            {
                var player = GameManager.Instance.ActivePlayerControllers[characterID];
                
                RuntimeManager.AttachInstanceToGameObject(dialogueInstance, player.gameObject.transform);
                dialogueInstance.setParameterByName("SpeakerCharacter", characterID);
                dialogueInstance.start();
                dialogueInstance.release();
            }

            private void StopDialogue(int characterID)
            {
                // Check if there's a dialogue instance playing for this character
                if (currentDialogues.TryGetValue(characterID, out EventInstance dialogueInstance)) {
                    // Stop the dialogue instance and remove it from the dictionary
                    dialogueInstance.stop(STOP_MODE.ALLOWFADEOUT);
                    
                    currentDialogues.Remove(characterID);
                }
            }

            public IEnumerator WaitForResponseAudio(EventInstance askerInstance, int _playerID, Dictionary<int, PlayerController> _players, string context)
            {
                while (IsPlaying(askerInstance))
                {
                    yield return null;
                }
                askerInstance.release();
                GetRandomPlayerAndPlayResponse(_playerID, _players, context);
            }
            
            public IEnumerator WaitForLongResponseAudio(int _playerID, EventInstance askerInstance, EventReference eventRef)
            {
                while (IsPlaying(askerInstance))
                {
                    yield return null;
                }
                askerInstance.release();
                askerInstance = RuntimeManager.CreateInstance(eventRef);
                PlayLongDialogue(_playerID, askerInstance);
            }


            private bool IsPlaying(EventInstance _instance)
            {
                PLAYBACK_STATE state;
                _instance.getPlaybackState(out state);
                return state != PLAYBACK_STATE.STOPPED;
            }

            public void GetRandomPlayerAndPlayResponse(int _playerID, Dictionary<int, PlayerController> _players, string context) 
            {
                var _randomPlayer = _players.ElementAt(Random.Range(0, _players.Count)).Value;
                
                if (_randomPlayer != _players[_playerID])
                {
                    if (context == "gold")
                    {
                        GoldPickupReaction(_randomPlayer.PlayerIndex);
                    }
                    else if (context == "death")
                    {
                        DeathReactionAudio(_randomPlayer.PlayerIndex);
                    }
                    else if (context == "return")
                    {
                        ReturnToCartDialogue(_randomPlayer.PlayerIndex);
                    }
                }
                else {
                    GetRandomPlayerAndPlayResponse(_playerID, _players, context);
                }
            }
            
            public void GetRandomPlayerAndPlayDialogue(Dictionary<int, PlayerController> _players, string context) 
            { 
                var _randomPlayer = _players[Random.Range(0, _players.Count)];
                if (context == "return")
                {
                        ReturnToCartDialogue(_randomPlayer.PlayerIndex);
                }
               
                
            }
        #endregion
        }
    }
}
