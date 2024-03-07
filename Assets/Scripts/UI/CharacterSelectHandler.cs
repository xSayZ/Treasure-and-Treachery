// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/05
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using Game.Backend;
using Game.WorldMap;
using Ink.Parsed;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Game
{
    namespace UI
    {

        public class CharacterSelectHandler : MonoBehaviour
        {
            [Header("References")] 
            // This is the level that will be loaded when the character select is done
            public LevelDataSO LevelToLoad;
            // This is the image bank that holds all the character images
            [SerializeField] private ImageBank bank;
            // This is the text that will be displayed when the game is ready to start
            [SerializeField] private GameObject startGameText;
            
            [FormerlySerializedAs("imagePosition")]
            [Space]
            [Header("Lists")]
            [SerializeField] public List<Transform> ImagePosition = new List<Transform>();
            [FormerlySerializedAs("PressToJoinText")]
            [SerializeField] private List<GameObject> pressToJoinText;
            [FormerlySerializedAs("Datas")]
            [SerializeField] public List<PlayerData> PlayerDatas = new List<PlayerData>();
            [Space]
            
            // Public Static Lists
            public static List<CharacterSelect> StaticData = new List<CharacterSelect>();
            public static List<PlayerInput> PlayerList { get; } = new List<PlayerInput>();

            [Header("Character Selects")]
            //For selectedAmountOfPlayers
            [SerializeField]private List<CharacterSelect> selects = new List<CharacterSelect>();

            [Space]
            // Input Actions
            [SerializeField] private InputAction joinAction;
            [SerializeField] private InputAction leaveAction;
            
            // Public Dictionaries
            public Dictionary<int, Sprite> Images = new Dictionary<int, Sprite>();
            public Dictionary<int, Sprite> ImagesBackup = new Dictionary<int, Sprite>();

            // Public Booleans
            public bool BeginGame { get; private set; }

            //EVENTS
            public event System.Action<PlayerInput> PlayerJoinedGame;
            public event System.Action<PlayerInput> PlayerLeaveGame;
            
#region Unity Functions
            public void Start()
            {
                StaticData.Clear();
                for (int i = 0; i < bank.characterImages.Count; i++)
                {
                    Images.Add(i, bank.characterImages[i]);
                    ImagesBackup.Add(i, bank.characterImages[i]);
                }

                PlayerList.Clear();
                if (Input.GetJoystickNames().Length >0)
                {
                    PlayerInputManager.instance.JoinPlayer(0, -1, null);
                }
            }

            private void OnEnable()
            {
                joinAction.Enable();
                joinAction.performed += JoinActionOnPerformed;
                leaveAction.Enable();
                leaveAction.performed += LeaveActionOnPerformed;
            }
            private void OnDisable()
            {
                joinAction.Disable();   
                joinAction.performed -= JoinActionOnPerformed;
                leaveAction.Disable();
                leaveAction.performed -= LeaveActionOnPerformed;
            }
            
            private void Update()
            {
                // Expensive Method - Can we change this?
                StartGame();
            }
  #endregion

#region PlayerManagerInput

            public void OnPlayerJoin(PlayerInput player)
            {
                PlayerList.Add(player);
                pressToJoinText[player.playerIndex].gameObject.SetActive(false);
                selects.Add(player.GetComponent<CharacterSelect>());
                if (PlayerJoinedGame != null)
                {
                    PlayerJoinedGame(player);
                }
            }

            public void OnPlayerLeft(PlayerInput player)
            {
                Debug.Log("Player Left the game");
            }

            #endregion
            
#region Private Functions
            
            private void StartGame()
            {
                if (selects.All(p => p.PlayersIsReady))
                {
                    BeginGame = true;
                    startGameText.SetActive(true);
                    
                    selects.Sort((p1,p2)=>p1.deviceID.CompareTo(p2.deviceID));
                    StaticData = selects;
                }
                else
                {
                    BeginGame = false;
                    startGameText.SetActive(false);
                }
            }

            private void JoinAction(InputAction.CallbackContext context)
            {
                PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
            }

            private void LeaveAction(InputAction.CallbackContext context)
            {
                if (PlayerList.Count > 1)
                {
                    foreach (PlayerInput player in PlayerList)
                    {
                        foreach (InputDevice device in player.devices)
                        {
                            if (device == null || context.control.device != device) continue;
                            UnregisterPlayer(player);
                            return;
                        }
                    }
                }
            }

            private void UnregisterPlayer(PlayerInput player)
            {
                selects.Remove(player.GetComponent<CharacterSelect>());
                pressToJoinText[player.playerIndex].SetActive(true);
                PlayerList.Remove(player);
                if (PlayerLeaveGame != null)
                {
                    PlayerLeftGame(player);
                }

                Destroy(player.transform.gameObject);
            }

            private void PlayerLeftGame(PlayerInput player)
            {
                // ???
            }
            
            private void LeaveActionOnPerformed(InputAction.CallbackContext _context) {
                LeaveAction(_context);
            }
            private void JoinActionOnPerformed(InputAction.CallbackContext _context) {
                JoinAction(_context);
            }

            #endregion

        }
    }
}