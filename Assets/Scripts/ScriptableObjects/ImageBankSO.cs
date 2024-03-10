using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace UI
    {
        [CreateAssetMenu(fileName = "UI images", menuName = "Game Images/sprites")] 
        public class ImageBankSO : ScriptableObject
        {
            public List<Sprite> characterImages;

            public List<Sprite> IntroImages;
        }
    }
}