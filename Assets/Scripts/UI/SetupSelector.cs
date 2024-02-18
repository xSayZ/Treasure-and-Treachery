
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
using Game;
using Game.Backend;
using Game.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class SetupSelector : Singleton<SetupSelector>
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
    

    public void SetActivePlayers()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            //GameObject image = Instantiate(UIImage, _layoutGroup.transform, true);
            //selects.Add(image.GetComponent<CharacterSelect>());
        }
    }
    public void Start()
    {
        playerList.Clear();
        for (int i = 0; i < bank.characterImages.Count; i++)
        {
            Images.Add(i,bank.characterImages[i]);
        }
        /*
        _layoutGroup = FindObjectOfType<HorizontalLayoutGroup>();
        
        for (int i = 0; i < bank.characterImages.Count; i++)
        {
            Images.Add(i,bank.characterImages[i]);
        }
        SetActivePlayers();
        */
        PlayerInputManager.instance.JoinPlayer(0, -1, null);
        joinAction.Enable();
        joinAction.performed += context => JoinAction(context);
        
        leaveAction.Enable();
        leaveAction.performed += context => LeaveAction(context);

    }

   

    private void JoinAction(InputAction.CallbackContext context)
    {
        PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
    }
    
    private void LeaveAction(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }


    public void OnPlayerJoin(PlayerInput player)
    {
        
        /*
        Debug.Log("Hi");
        _layoutGroup = FindObjectOfType<HorizontalLayoutGroup>();
        
        GameObject image = Instantiate(UIImage, _layoutGroup.transform, true);
        selects.Add(image.GetComponent<CharacterSelect>());
        */

        playerList.Add(player);
        if (PlayerJoinedGame != null)
        {
            PlayerJoinedGame(player);
        }
    }

    public void OnPlayerLeft(PlayerInput player)
    {
        
    }



    private void Update()
    {
        if (selects.All(a=> a.playersIsReady))
        {
            // Debug.log("All Ready")
        }
        
    }

}
