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
            private EventReference shovelPickup, goldPickupAudio, goldReactAudio, objectiveProgressionReaction, deathAudio, damageAudio, playerAttackAudio, deathReactAudio, cartEnterAudio;
            
            private int playerIndex;

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
                    PlayDialogue(_playerID, damageAudioInstance);
                }
                catch (Exception e)
                {
                    Debug.LogError("[{DialogueAudio}]: Error Exception " + e);
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
                    Debug.LogError("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            public void UpdateWereWolfRageAudio(float rageAmount)
            {
                playerAttackAudioInstance.setParameterByName("WolfRage", rageAmount);
                Debug.Log("Wrath is" + rageAmount);
            }
            public void PlayerAttackAudio(int _playerID)
            {
                try
                {
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var playerAttackAudioInstance = RuntimeManager.CreateInstance(playerAttackAudio);
                    PlayDialogue(_playerID, playerAttackAudioInstance);
                }
                catch (Exception e)
                {
                    Debug.LogError("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            private void PlayerDeathDialogue(int _playerID)
            {
                try
                {
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var deathDialogueInstance = RuntimeManager.CreateInstance(deathAudio);
                    PlayDialogue(_playerID, deathDialogueInstance);
                
                    if (_players.Count > 1)
                    {
                        DialogueAudioWrapper.Instance.PlayResponseDialogue(bajsInstance, _playerID, _players, "death");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            private void ShovelPickupAudio(int _playerID, Item _item)
            {
                try
                {
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var shovelPickupInstance = RuntimeManager.CreateInstance(shovelPickup);
                    PlayDialogue(_playerID, shovelPickupInstance);
                }
                catch (Exception e)
                {
                    Debug.LogError("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            private void ObjectiveProgressionReactionAudio(int _playerID)
            {
                try
                {
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var shovelReactInstance = RuntimeManager.CreateInstance(objectiveProgressionReaction);
                    PlayDialogue(_playerID, shovelReactInstance);
                }
                catch (Exception e)
                {
                    Debug.LogError("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            private void GoldPickupDialogue(int _playerID, int amount)
            {
                try
                {
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var goldPickupInstance = RuntimeManager.CreateInstance(goldPickupAudio);
                    PlayDialogue(_playerID, goldPickupInstance);
                    
                    if (_players.Count > 1)
                    {
                        DialogueAudioWrapper.Instance.PlayResponseDialogue(goldPickupInstance, _playerID, _players, "gold");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            private void GoldPickupReaction(int _playerID)
            {
                try
                {
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var goldReactInstance = RuntimeManager.CreateInstance(goldReactAudio);
                    PlayDialogue(_playerID, goldReactInstance);
                }
                catch (Exception e)
                {
                    Debug.LogError("[{DialogueAudio}]: Error Exception " + e);
                }
            }

            private void DeathReactionAudio(int _playerID)
            {
                try
                {
                    Debug.Log("player " + _playerID + " has something to say");
                    var _players = GameManager.Instance.ActivePlayerControllers;
                    var deathReactioninstance = RuntimeManager.CreateInstance(deathReactAudio);
                    PlayDialogue(_playerID, deathReactioninstance);
                }
                catch (Exception e)
                {
                    Debug.LogError("[{DialogueAudio}]: Error Exception " + e);
                }
            }
#endregion

#region Private Functions
            private void PlayDialogue(int characterID, EventInstance dialogueInstance)
            {
                // Check if there's already a dialogue instance playing for this character
                if (currentDialogues.ContainsKey(characterID)) {
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

            private void StopDialogue(int characterID)
            {
                // Check if there's a dialogue instance playing for this character
                if (currentDialogues.TryGetValue(characterID, out EventInstance dialogueInstance)) {
                    // Stop the dialogue instance and remove it from the dictionary
                    dialogueInstance.stop(STOP_MODE.ALLOWFADEOUT);
                    dialogueInstance.release();
                    currentDialogues.Remove(characterID);
                }
            }

            public IEnumerator WaitForResponseAudio(EventInstance askerInstance, int _playerID, Dictionary<int, PlayerController> _players, string context)
            {
                while (IsPlaying(askerInstance))
                {
                    yield return null;
                }
                GetRandomPlayerAndPlayResponse(_playerID, _players, context);
            }

            private bool IsPlaying(EventInstance _instance)
            {
                PLAYBACK_STATE state;
                _instance.getPlaybackState(out state);
                return state != PLAYBACK_STATE.STOPPED;
            }

            private void GetRandomPlayerAndPlayResponse(int _playerID, Dictionary<int, PlayerController> _players, string context) 
            { 
                var _randomPlayer = _players[Random.Range(0, _players.Count)];
                
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
                }
                else {
                    GetRandomPlayerAndPlayResponse(_playerID, _players, context);
                }
            }
#endregion
        }
    }
}
