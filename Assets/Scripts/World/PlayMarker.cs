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
            [Tooltip("The level data of the level. This is used to load the level scene.")]
            [SerializeField] private LevelDataSO levelData;
            
            [Header("Object Options")]
            [Tooltip("The level UI object. This is used to display the level UI.")]
            [SerializeField] private GameObject levelUI;
            [Tooltip("Set this for the visualization when a level is playable. This is automatically turned off if the level is locked.")]
            [SerializeField] private GameObject playMarkerObject;
            
            [Header("Events")]
            [Tooltip("When the level of this play marker is completed, this event will be called. This can be used to change the world map.")]
            public UnityEvent onLevelCompleted = new UnityEvent();
            
            // Internal Variables
            private Image levelImage;
            private string levelDescription;
            private string levelName;
            
            private bool canSwitchScene = false;
            private bool isLocked;
            
            private void Start() {
                // Set the level data
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
                    levelUI.SetActive(true);
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
                levelUI.SetActive(false);
                canSwitchScene = false;
            }
            
            private void SwitchScene() {
                LevelManager.Instance.worldMapManager.carriagePosition = transform.position;
                LevelManager.Instance.LoadLevel(levelData);
            }
        }
    }
}