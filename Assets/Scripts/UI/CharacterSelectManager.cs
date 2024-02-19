
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
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CharacterSelectManager : Singleton<CharacterSelectManager>
{
    public List<PlayerData> Datas;
    public List<CharacterSelect> selects;
    public Dictionary<int, Sprite> Images = new Dictionary<int, Sprite>();

    public List<PlayerInput> playerList = new List<PlayerInput>();
    public ImageBank bank;
    
    private HorizontalLayoutGroup _layoutGroup;

    
    [SerializeField] private PlayerData[] data;
    

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
        if (selects.All(a=> a.playersIsReady) && selects.Any(a => a.beginGame))
        {
            Debug.Log("high");
            SceneManager.LoadScene("Adams World");
        }
        
    }

    private void JoinAction(InputAction.CallbackContext context)
    {
        PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
    }
    
    private void LeaveAction(InputAction.CallbackContext context)
    {
        
    }


    public void OnPlayerJoin(PlayerInput player)
    {
        playerList.Add(player);
        
        selects.Add(player.GetComponent<CharacterSelect>());
        if (PlayerJoinedGame != null)
        {
            PlayerJoinedGame(player);
        }
    }
    
    public void OnPlayerLeft(PlayerInput player)
    {
        //playerList.Remove(player);
        
    }


}
