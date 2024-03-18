// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-24
// Author: Felix
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Game {
    namespace WorldMap {
        [CreateAssetMenu(fileName = "Level Data", menuName = "World Map/Level Data", order = 1)]
        public class LevelDataSO : ScriptableObject {
            
            [Header("Level Data")]
            [Tooltip("The path to the level scene. This is used to load the level scene.")]
            public string levelPath;
            
            [Header("UI Data")]
            [Tooltip("The name of the level. This is used to display the level name in the UI.")]
            public string levelName;
            [Tooltip("The description of the level. This is used to display the level description in the UI.")]
            public string levelDescription;
            [Tooltip("The image of the level. This is used to display the level image in the UI.")]
            public Sprite levelImage;
            
            [Header("Level Status")]
            [Tooltip("Don't change this value. This is used to check if the level is completed.")]
            public bool isCompleted;
            [Tooltip("This is used to check if the level is a gameplay scene. If it is a gameplay scene, the game will load the level scene. If it is not a gameplay scene, the game will load the world map scene.")]
            public bool isGameplayScene = true;
            [Tooltip("This is used to check if the game should display the loading screen. If it is set to false, the game will not display the loading screen.")]
            public bool allowLoadingScreen = true;            
            [Space]
            [Header("Prerequisites")]
            [Tooltip("This is used to check if the level is locked. If it is locked, the game will not load the level scene.")]
            public List<LevelDataSO> prerequisites = new List<LevelDataSO>();

            private void OnEnable() {
                isCompleted = false;
            }
        }
    }
}
