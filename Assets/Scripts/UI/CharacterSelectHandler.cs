
// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/05
// Author: Fredrik
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Backend;
using Game.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;

public class CharacterSelectHandler : MonoBehaviour
{
    [Header("References")] 
    public List<PlayerData> Datas;
    public List<Transform> imagePosition = new List<Transform>();
    public Dictionary<int, Sprite> Images = new Dictionary<int, Sprite>();
    public Dictionary<int, Sprite> ImagesBackup = new Dictionary<int, Sprite>();


  
    [SerializeField] private ImageBank bank;
    
    [SerializeField] private List<GameObject> PressToJoinText;
    [SerializeField] private GameObject StartGame;
    private List<PlayerInput> playerList = new List<PlayerInput>();
    private List<CharacterSelect> selects = new List<CharacterSelect>();
    
    
    [SerializeField] private InputAction joinAction;
    [SerializeField] private InputAction leaveAction;
    //EVENTS
    public event System.Action<PlayerInput> PlayerJoinedGame;
    public event System.Action<PlayerInput> PlayerLeaveGame; 
    
    public void Start()
    {
        for (int i = 0; i < bank.characterImages.Count; i++)
        {
            Images.Add(i,bank.characterImages[i]);
            ImagesBackup.Add(i,bank.characterImages[i]);
            
        }
        playerList.Clear();
        PlayerInputManager.instance.JoinPlayer(0, -1, null);
        
        joinAction.Enable();
        joinAction.performed += context => JoinAction(context);
        
        leaveAction.Enable();
        leaveAction.performed += context => LeaveAction(context);

    }

       
    private void Update()
    {

        if (selects.All(p => p.PlayersIsReady))
        {
            Debug.Log("Hello There");
            SceneManager.LoadScene("Adams World");
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


}
