// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-19
// Author: b22feldy
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Backend;
using Game.CharacterSelection;
using Game.Dialogue;
using Game.Managers;
using Game.World;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game {
    namespace Racer {
        public class CarriageRacer : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private GameObject racerPlayerInputPrefab;
            [SerializeField] private DialogueManager dialogueManager;
            
            [Header("Sub Behaviours")]
            [SerializeField] private CarriageMovementBehaviour carriageMovementBehaviour;
            [SerializeField] private CarriageAnimationBehaviour carriageAnimationBehaviour;
            
            [Header("Debug")]
            [SerializeField] private List<Vector2> activeLeftStickValues = new List<Vector2>();
            
            private Vector3 averageLeftStickValue;
            private PlayMarker playMarkerInRange;

#region Unity Functions
            private void Awake()
            {
                if (CharacterSelect.selectedCharacters.Count == 0)
                {
                    foreach (InputDevice _inputDevice in InputSystem.devices)
                    {
                        GameObject _racerPlayerInput = Instantiate(racerPlayerInputPrefab);
                        _racerPlayerInput.GetComponent<RacerPlayerInput>().Setup(this, dialogueManager, _inputDevice);
                        activeLeftStickValues.Add(new Vector2());
                    }
                }
                else
                {
                    foreach (KeyValuePair<InputDevice, PlayerData> _kvp in CharacterSelect.selectedCharacters)
                    {
                        GameObject _racerPlayerInput = Instantiate(racerPlayerInputPrefab);
                        _racerPlayerInput.GetComponent<RacerPlayerInput>().Setup(this, dialogueManager, _kvp.Key);
                        activeLeftStickValues.Add(new Vector2());
                    }
                }
            }

            private void Start()
            {
                playMarkerInRange = null;
                
                transform.position = LevelManager.Instance.worldMapManager.carriagePosition;
                transform.rotation = LevelManager.Instance.worldMapManager.carriageRotation;
                carriageMovementBehaviour.SetupBehaviour();
                carriageAnimationBehaviour.SetupBehaviour();
            }

            private void FixedUpdate()
            {
                UpdateCarriageMovement();
            }
#endregion

#region Public Functions
            public void SetCarriageActive(bool _active)
            {
                carriageMovementBehaviour.enabled = _active;
            }

            public void OnMoveInput(int _playerInputIndex, Vector2 _moveValue)
            {
                activeLeftStickValues[_playerInputIndex] = _moveValue;
                
                // Calculate the average left stick value
                averageLeftStickValue = CalculateAverageLeftStickValue();
                carriageAnimationBehaviour.UpdateMovementAnimation(averageLeftStickValue.magnitude);
            }

            public void OnSelectPlayMarker()
            {
                if (playMarkerInRange != null)
                {
                    playMarkerInRange.SwitchScene(null);
                }
            }

            public void SetPlayMarkerInRange(PlayMarker _playMarker)
            {
                playMarkerInRange = _playMarker;
            }
#endregion

#region Private Functions
            private void UpdateCarriageMovement()
            {
                carriageMovementBehaviour.UpdateMovementData(averageLeftStickValue);
            }

            private Vector3 CalculateAverageLeftStickValue()
            {
                if (activeLeftStickValues.Count == 0)
                    return Vector2.zero;
                
                Vector2 average = Vector2.zero;
                
                int notZeroCount = 0;
                
                foreach (var value in activeLeftStickValues)
                {
                    average += value;
                    if (value != Vector2.zero)
                    {
                        notZeroCount++;
                    }
                }
                
                if (notZeroCount > 0)
                {
                    average /= notZeroCount;
                }
                
                averageLeftStickValue = new Vector3(average.x, 0, average.y);
                return averageLeftStickValue;
            }
#endregion
        }
    }
}