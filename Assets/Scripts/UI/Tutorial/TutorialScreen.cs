// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-14
// Author: b22alesj
// Description: Controls the tutorial screen
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Backend;
using Game.CharacterSelection;
using Game.Managers;
using Game.WorldMap;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game {
    namespace Tutorial {
        public class TutorialScreen : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private List<GameObject> continueIcons;
            [SerializeField] private List<GameObject> doneIcons;
            [SerializeField] private GameObject tutorialPlayerInputPrefab;
            [SerializeField] private LevelDataSO levelToLoad;
            
            private int doneCount;

            private void Awake()
            {
                List<int> playerIndexes = new List<int>();
                
                foreach (KeyValuePair<InputDevice, PlayerData> _kvp in CharacterSelect.selectedCharacters)
                {
                    playerIndexes.Add(_kvp.Value.playerIndex);
                    
                    Instantiate(tutorialPlayerInputPrefab).GetComponent<TutorialPlayerInput>().Setup(this, _kvp.Value.playerIndex, _kvp.Key);
                }
                
                for (int i = 0; i < 4; i++)
                {
                    if (playerIndexes.Contains(i))
                    {
                        continueIcons[i].SetActive(true);
                        doneIcons[i].SetActive(false);
                    }
                    else
                    {
                        continueIcons[i].SetActive(false);
                        doneIcons[i].SetActive(false);
                    }
                }
            }

            public void PlayerDone(int _playerIndex)
            {
                doneCount++;
                
                continueIcons[_playerIndex].SetActive(false);
                doneIcons[_playerIndex].SetActive(true);
                
                if (doneCount >= CharacterSelect.selectedCharacters.Count)
                {
                    LevelManager.Instance.LoadLevel(levelToLoad);
                }
            }
        }
    }
}