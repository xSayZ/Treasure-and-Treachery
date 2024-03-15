// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-21
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using System.Linq;
using Game.Managers;
using Game.Racer;
using Game.WorldMap;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;


namespace Game {
    namespace World {
        public class PlayMarker : MonoBehaviour
        {
            [Header("Level Data")]
            [Tooltip("The level data of the level. This is used to load the level scene.")]
            [SerializeField] private LevelDataSO levelData;
            [SerializeField] private List<GameObject> gameObjectsForWorldChange = new List<GameObject>();
            
            [Header("Object Options")]
            [Tooltip("The level UI object. This is used to display the level UI.")]
            [SerializeField] private GameObject levelDescriptionUI;
            [Tooltip("Set this for the visualization when a level is playable. This is automatically turned off if the level is locked.")]
            [SerializeField] private GameObject playMarkerObject;
            [SerializeField] private Image levelImage;
            [SerializeField] private TextMeshProUGUI levelDescription;
            [SerializeField] private TextMeshProUGUI levelName;
            
            [Header("Events")]
            [Tooltip("When the level of this play marker is completed, this event will be called. This can be used to change the world map.")]
            public UnityEvent onLevelCompleted = new UnityEvent();
            
            // Internal Variables
            private bool isLocked;
            
            private void Start()
            {
                isLocked = true;
                
                // Set the level data
                levelImage.sprite = levelData.levelImage;
                levelDescription.text = levelData.levelDescription;
                levelName.text = levelData.levelName;
                
                if (isLocked)
                {
                    playMarkerObject.SetActive(false);
                }
                else
                {
                    playMarkerObject.SetActive(true);
                }
                
                if (levelData.isCompleted)
                {
                    onLevelCompleted.Invoke();
                }
            }

            private void Update()
            {
                for (int i = 0; i < levelData.prerequisites.Count; i++)
                {
                    if(levelData.prerequisites.All(_data => _data.isCompleted))
                    {
                        isLocked = false;
                        playMarkerObject.SetActive(true);
                    }
                }
                
                if (levelData.prerequisites.Count == 0)
                {
                    isLocked = false;
                    playMarkerObject.SetActive(true);
                }
            }

            private void OnTriggerEnter(Collider _other)
            {
                if (_other.CompareTag("Carriage"))
                {
                    _other.GetComponent<CarriageRacer>().SetPlayMarkerInRange(this);
                    
                    levelDescriptionUI.SetActive(true);
                }
            }

            private void OnTriggerExit(Collider _other)
            {
                if (_other.CompareTag("Carriage"))
                {
                    _other.GetComponent<CarriageRacer>().SetPlayMarkerInRange(null);
                    
                    levelDescriptionUI.SetActive(false);
                }
            }

            public void SwitchScene()
            {
                LevelManager.Instance.worldMapManager.carriagePosition = transform.position;
                LevelManager.Instance.worldMapManager.carriageRotation = transform.rotation;
                LevelManager.Instance.LoadLevel(levelData);
            }
        }
    }
}