using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class SetupSelector : MonoBehaviour
{
    public GameObject UIImage;
    public int SetStartImageX;
    public int SetStartImageY;
    public int ImageSpacing;

    public Image Border;
    public List<CharacterSelect> selects;
    public List<Sprite> Images;

    public ImageBank bank;
    
    public void SetActivePlayers()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Selection");
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            GameObject image = Instantiate(UIImage, canvas.transform, true);
            image.GetComponent<RectTransform>().anchoredPosition = new Vector2(-SetStartImageX+ImageSpacing*(i+1), SetStartImageY);
            selects.Add(image.GetComponent<CharacterSelect>());
        }
    }
    public void Start()
    {
        SetActivePlayers();
    }

    private void Update()
    {
        if (selects.All(a=> a.playersIsReady))
        {
            Debug.Log("all ready");
        }
        else
        {
            Debug.Log("All not ready");
        }
        
        
    }
   
}
