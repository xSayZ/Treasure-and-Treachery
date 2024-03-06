// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-26
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;
using UnityEngine.UI;


namespace Game {
    namespace Dialogue {
        [CreateAssetMenu(fileName = "Current Dialogue", menuName = "Dialogue/Current Dialogue")]
        public class CurrentDialogueSO : ScriptableObject
        {
            [Header("Ink Story")]
            [SerializeField] private TextAsset storyJSON;
            
            [Header("Params")]
            [SerializeField] private float typingSpeed = 0.05f;

            [SerializeField] private Sprite eventImage;
            
            private bool hasBeenRead = false;

            private void OnEnable() {
                hasBeenRead = false;
            }

            public TextAsset StoryJSON
            {
                get => storyJSON;
                set => storyJSON = value;
            }
            
            public float TypingSpeed
            {
                get => typingSpeed;
                set => typingSpeed = value;
            }
            
            public bool HasBeenRead
            {
                get => hasBeenRead;
                set => hasBeenRead = value;
            }
            
            public Sprite EventImage
            {
                get => eventImage;
                set => eventImage = value;
            }
        }
    }
}
