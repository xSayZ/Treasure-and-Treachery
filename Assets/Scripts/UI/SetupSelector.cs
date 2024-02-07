
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
using Game.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SetupSelector : MonoBehaviour
{
    public GameObject UIImage;
    public List<CharacterSelect> selects;
    public static Dictionary<int, Sprite> Images = new Dictionary<int, Sprite>();
    
    public ImageBank bank;
    private HorizontalLayoutGroup _layoutGroup;
    
    public enum Select
    {
        wolf,
        lilith,
        gorgon,
        kobold
    }
    
    public void SetActivePlayers()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            GameObject image = Instantiate(UIImage, _layoutGroup.transform, true);
            selects.Add(image.GetComponent<CharacterSelect>());
        }
    }
    public void Start()
    {
        _layoutGroup = FindObjectOfType<HorizontalLayoutGroup>();
        
        for (int i = 0; i < bank.characterImages.Count; i++)
        {
            Images.Add(i,bank.characterImages[i]);
        }
        SetActivePlayers();
        
    }

    private void Update()
    {
       
        
        
        if (selects.All(a=> a.playersIsReady))
        {
            // Debug.log("All Ready")
        }
        
    }
   
}
