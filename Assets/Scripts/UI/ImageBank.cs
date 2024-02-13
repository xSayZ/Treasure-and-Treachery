using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [CreateAssetMenu(fileName = "UI images", menuName = "Game Images/sprites")] 
    public class ImageBank : ScriptableObject
    {
        public List<Sprite> characterImages;

        public List<Sprite> IntroImages;


    }
}