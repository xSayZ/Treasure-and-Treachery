// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-21
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using Game.WorldMap;
using UnityEngine;


namespace Game {
    namespace World {
        public class PlayMarker : MonoBehaviour
        {
            public class LevelData
            {
                public string levelName;
                public string levelDescription;
                public Sprite levelImage;
                
                LevelData (string levelName, string levelDescription, Sprite levelImage)
                {
                    this.levelName = levelName;
                    this.levelDescription = levelDescription;
                    this.levelImage = levelImage;
                }
            }

            public LevelDataSO LevelDataSo;
            
            [SerializeField] private string levelName = null;

            [SerializeField] private GameObject obstacle;

            [Header("Map Marker Options")]
            public bool isLocked;
            
            [Header("Object Options")]
            [SerializeField] private GameObject unlockedObject = null;
            
            [Header("Image Options")]
            [SerializeField] private GameObject unlockedImage = null;
            
            private void Start() {
                
                LevelDataSo.levelName = levelName;
                LevelDataSo.levelDescription = "This is a level description";
                LevelDataSo.levelImage = null;
                
                if (isLocked) {
                    unlockedObject.SetActive(false);
                }
                else {
                    unlockedObject.SetActive(true);
                }
            }

            private void OnTriggerEnter(Collider other) {
                if (!other.CompareTag("Carriage"))
                    return;

                if (!isLocked) {
                    unlockedImage.SetActive(true);
                    
                }
                
            }
            
            private void OnTriggerExit(Collider other) {
                unlockedImage.SetActive(false);
            }
            
            private void SwitchScene() {
                if (!isLocked) {
                    //Managers.LevelManager.Instance.LoadScene();
                }
            }
        }
    }
}