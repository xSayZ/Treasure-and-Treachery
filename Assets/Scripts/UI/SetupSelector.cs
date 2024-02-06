using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SetupSelector : MonoBehaviour
{
    public GameObject UIImage;

    public List<CharacterSelect> selects;
    public void SetActivePlayers()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Selection");
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            GameObject image = Instantiate(UIImage, canvas.transform, true);
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
            SceneManager.LoadScene("Daniel");
        }
        else
        {
            Debug.Log("All not ready");
        }
    }
}
