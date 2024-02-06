using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Game.Backend;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class CharacterSelect : MonoBehaviour
{
    public GameObject UIImage;
    public PlayerInput playerInputs;
    public PlayerData data;
    public Image Image;
    public int id;
    public int currentId;
    
    public bool playersIsReady;
    
    public GameObject Ready;

    private Sprite selected;
    public enum Select
    {
        wolf,lilith,gorgon,kobold
    }
    
    public List<Sprite> Images;
    
    // Start is called before the first frame update
    public void Start()
    {
        Image.sprite = Images[id % 4];
    }

    public void OnNavigation(InputAction.CallbackContext context)
    {
        if (!playersIsReady)
        {
            Vector2 value = context.ReadValue<Vector2>();
            id += (int)value.y;
            id = Wrap(id, 0, 4);
            if (id == 4) id = 0;
            Image.sprite = Images[id % 4];
            currentId = id % 4;
            
        }
     
    }


    public int Wrap(int Target, int LowerBounds, int UpperBounds)
    {
        int dif = UpperBounds - LowerBounds;

        if (Target > LowerBounds)
        {
            return LowerBounds + (Target - LowerBounds) % dif;
        }

        Target = LowerBounds + (LowerBounds - Target);
        int tempVal = LowerBounds + (Target - LowerBounds) % dif;
        return UpperBounds - tempVal;
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (context.performed && !playersIsReady)
        {
            selected = Images[currentId];
            data.playerIndex = currentId;
            Ready.SetActive(true);
            playersIsReady = true;
            
            
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed && playersIsReady)
        {
            Ready.SetActive(false);
            playersIsReady = false;
        }
    }
    
}
