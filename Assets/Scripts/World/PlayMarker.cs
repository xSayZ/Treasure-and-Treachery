// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-21
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEngine;


namespace Game {
    namespace World {
        public class PlayMarker : MonoBehaviour {
            
            [SerializeField] private int levelIndex = 0;

            [Header("Map Marker Options")]
            public bool isLocked;
            
            [Header("Object Options")]
            [SerializeField] private GameObject unlockedObject = null;
            
            [Header("Image Options")]
            [SerializeField] private GameObject unlockedImage = null;
            
            private void Start() {
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
                    if (other.gameObject.GetComponent<Racer.CarriageRacer>().GetSubmitPressed()){
                        SwitchScene();
                    }
                }
                
            }
            
            private void OnTriggerExit(Collider other) {
                unlockedImage.SetActive(false);
            }
            
            private void SwitchScene() {
                if (!isLocked) {
                    Managers.LevelManager.Instance.LoadScene(levelIndex);
                }
            }
        }
    }
}