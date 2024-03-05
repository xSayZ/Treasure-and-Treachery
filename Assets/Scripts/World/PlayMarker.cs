// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-21
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using System.Linq;
using Game.Managers;
using Game.WorldMap;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace Game {
    namespace World {
        public class PlayMarker : MonoBehaviour
        {
            [Header("Level Data")]
            [SerializeField] private LevelDataSO levelData;
            
            [Header("Object Options")]
            [SerializeField] private GameObject LevelUI;
            [SerializeField] private GameObject playMarkerObject;
            
            [Header("Map Marker Options")]
            public bool isLocked;
            
            public UnityEvent onLevelCompleted = new UnityEvent();
            
            private Image levelImage;
            private string levelName;
            bool canSwitchScene = false;
            
            private void Start() {
                //levelData.levelName = levelName;
                levelData.levelDescription = "This is a level description";
                levelData.levelImage = levelImage;
                levelData.OnLevelCompleted = onLevelCompleted;
                
                if (isLocked) {
                    playMarkerObject.SetActive(false);
                }
                else {
                    playMarkerObject.SetActive(true);
                }
            }

            private void Update() {
                for (int i = 0; i < levelData.prerequisites.Count; i++)
                {
                    if(levelData.prerequisites.All(data => data.isCompleted ))
                    {
                        isLocked = false;
                        playMarkerObject.SetActive(true);
                    }
                }
                
                
                
                
            }

            private void OnTriggerEnter(Collider other) {
                if (!other.CompareTag("Carriage"))
                    return;

                if (!isLocked) {
                    LevelUI.SetActive(true);
                    canSwitchScene = true;


                    InputSystem.onAnyButtonPress.Call(ctrl =>
                    {
                        if (ctrl.device is not Gamepad pad || !canSwitchScene) return;
                        
                        if (ctrl == pad.buttonSouth)
                        {
                            canSwitchScene = false;
                            SwitchScene();
                        }
                    });

                }
            }
            
            private void OnTriggerExit(Collider other) {
                LevelUI.SetActive(false);
                canSwitchScene = false;
            }
            
            private void SwitchScene() {
                LevelManager.Instance.LoadLevel(levelData);
            }
        }
    }
}