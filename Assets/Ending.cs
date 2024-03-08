using System;
using System.Collections.Generic;
using Game.Backend;
using Game.Dialogue;
using Game.Managers;
using Game.WorldMap;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Game {
    namespace Scenes {
        public class Ending : MonoBehaviour {
            [Header("Ink Story")]
            [SerializeField] private LevelDataSO level;
            [SerializeField] private List<Dialogue> dialogues = new List<Dialogue>();
            [SerializeField] private DialogueManager dialogueManager;
            [Serializable]
            private class Dialogue {
                public CurrentDialogueSO dialogueSO;
                public TextMeshProUGUI dialogueText;
                public Sprite eventImage;
                public bool hasBeenRead;
                
                Dialogue(CurrentDialogueSO _dialogueSO, TextMeshProUGUI _dialogueText, bool _hasBeenRead) {
                    dialogueSO = _dialogueSO;
                    dialogueText = _dialogueText;
                    hasBeenRead = _hasBeenRead;
                }
            }
            void Start() {
                StartDialogue(0);
            }
            
            public void StartDialogue(int _dialogueIndex) {
                if (dialogues[_dialogueIndex].dialogueSO.HasBeenRead) {
                    LevelManager.Instance.LoadLevel(level);
                    return;
                }
                
                dialogueManager.dialogueText = dialogues[_dialogueIndex].dialogueText;
                dialogueManager.StartDialogue(
                    dialogues[_dialogueIndex].dialogueSO.StoryJSON,
                    dialogues[_dialogueIndex].dialogueSO.TypingSpeed, 
                    dialogues[_dialogueIndex].dialogueSO.EventImage);
                foreach (var dialogue in dialogues) {
                    dialogue.dialogueSO.HasBeenRead = true;
                }
            }

            public void GetHighestScore() {
                var _players = dialogueManager.playerDatas;
                var _highestScore = 0;
                List<PlayerData> _playersToRandomize = new List<PlayerData>();
                
                _players.Sort((a, b) => b.points.CompareTo(a.points));
                _highestScore = _players[0].playerIndex;

                foreach (var player in _players) {
                    if (player.points == _players[_highestScore].points) {
                        _playersToRandomize.Add(player);
                    }
                }
                
                if (_playersToRandomize.Count > 1) {
                    var _random = new System.Random();
                    var _index = _random.Next(_playersToRandomize.Count);
                    _highestScore = _playersToRandomize[_index].playerIndex;
                }
                
                Debug.Log("Player: " + _playersToRandomize[_highestScore].playerIndex + " Highest score: " + _highestScore);
                
                dialogueManager.eventImage.sprite = dialogues[_highestScore + 1].eventImage;
                StartDialogue(_highestScore + 1);
            }
        }
    }
}
