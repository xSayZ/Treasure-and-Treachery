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

using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    namespace UI
    {

        public class CharacterSelectHandler : MonoBehaviour
        {
            [Header("References")] 
            public List<Transform> imagePosition = new List<Transform>();
            
            public List<PlayerData> Datas;

            public List<PlayerData> SelectedData;
            public static List<PlayerData> staticData = new List<PlayerData>();

            public List<PlayerData> SelectedPlayerData;
            [SerializeField] private ImageBank bank;
            
            [SerializeField] private List<GameObject> PressToJoinText;

            [SerializeField] private GameObject StartGameText;

            //For selectedAmountOfPlayers
            public static List<PlayerInput> playerList { get; } = new List<PlayerInput>();
            private List<CharacterSelect> selects = new List<CharacterSelect>();

            public Dictionary<int, Sprite> Images = new Dictionary<int, Sprite>();
            public Dictionary<int, Sprite> ImagesBackup = new Dictionary<int, Sprite>();

            public bool BeginGame { get; private set; }

            [SerializeField] private InputAction joinAction;

            [SerializeField] private InputAction leaveAction;

            //EVENTS
            public event System.Action<PlayerInput> PlayerJoinedGame;
            public event System.Action<PlayerInput> PlayerLeaveGame;


            public void Start()
            {
                
                staticData.Clear();
                for (int i = 0; i < bank.characterImages.Count; i++)
                {
                    Images.Add(i, bank.characterImages[i]);
                    ImagesBackup.Add(i, bank.characterImages[i]);
                }

                playerList.Clear();
                PlayerInputManager.instance.JoinPlayer(0, -1, null);

              
            }

            private void OnEnable()
            {
                joinAction.Enable();
                joinAction.performed += context => JoinAction(context);
                leaveAction.Enable();
                leaveAction.performed += context => LeaveAction(context);
            }
            private void OnDisable()
            {
                joinAction.Disable();   
                joinAction.performed -= context => JoinAction(context);
                leaveAction.Disable();
                leaveAction.performed -= context => LeaveAction(context);
            }


            private void Update()
            {
                StartGame();
            }

            #region PlayerManagerInput

            public void OnPlayerJoin(PlayerInput player)
            {
                playerList.Add(player);
                PressToJoinText[player.playerIndex].gameObject.SetActive(false);
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

            #region private

            
            
            private void StartGame()
            {
                if (selects.All(p => p.PlayersIsReady))
                {
                    BeginGame = true;
                    StartGameText.SetActive(true);
                
                    SelectedData.Sort((d1,d2)=>d1.ControllerID.CompareTo(d2.ControllerID));
                    staticData = SelectedData;
                    SelectedPlayerData = staticData;
                }
                else
                {
                    BeginGame = false;
                    StartGameText.SetActive(false);
                }
            }

            private void JoinAction(InputAction.CallbackContext context)
            {
                PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
            }

            private void LeaveAction(InputAction.CallbackContext context)
            {
                if (playerList.Count > 1)
                {
                    foreach (PlayerInput player in playerList)
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
                PressToJoinText[player.playerIndex].SetActive(true);
                playerList.Remove(player);
                if (PlayerLeaveGame != null)
                {
                    PlayerLeftGame(player);
                }

                Destroy(player.transform.gameObject);
            }

            private void PlayerLeftGame(PlayerInput player)
            {
            }

            #endregion

        }
    }
}