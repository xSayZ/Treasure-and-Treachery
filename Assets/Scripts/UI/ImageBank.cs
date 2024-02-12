using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Player sprites", menuName = "Player sprite Data/sprites")]    public class ImageBank : ScriptableObject
    {
        public List<Sprite> characterImages;
    }
}