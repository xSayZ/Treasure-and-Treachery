// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-12
// Author: alexa
// Description: Character select script for individual player
// --------------------------------
// ------------------------------*/

using System;
using System.Collections.Generic;
using Game.Backend;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Game {
    namespace CharacterSelection {
        public class PlayerCharacterSelect : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private PlayerInput playerInput;
            [SerializeField] private Image characterImage;
            [SerializeField] private GameObject selectedIndicator;
            [SerializeField] private List<PlayerData> playerDatas;
            
            private CharacterSelect characterSelect;
            private bool canSelectCharacter;
            private bool hasSelectedCharacter;
            private int currentSelectedCharacter;
            private Vector2 previousMoveValue;

#region Unity Functions
            private void OnEnable()
            {
                characterSelect = FindObjectOfType<CharacterSelect>();
                characterSelect.playerInputs.Add(playerInput);
                
                previousMoveValue = new Vector2();
                
                characterImage.sprite = playerDatas[currentSelectedCharacter].characterSelectImage;
                
                // Set parent, position, rotation and scale
                Transform _playerImageTransform = characterSelect.PlayerImageTransforms[playerInput.playerIndex];
                transform.SetParent(_playerImageTransform);
                transform.position = _playerImageTransform.position;
                transform.rotation = _playerImageTransform.rotation;
                transform.localScale = _playerImageTransform.GetChild(0).localScale;
                
                characterSelect.UpdateImageTint.AddListener(UpdateImageTint);
            }

            private void OnDisable()
            {
                characterSelect.playerInputs.Remove(playerInput);
                characterSelect.UpdateImageTint.RemoveListener(UpdateImageTint);
            }
#endregion

#region Input Functions
            public void OnMove(InputAction.CallbackContext _context)
            {
                if (hasSelectedCharacter)
                {
                    return;
                }
                
                // Get which way to increment
                Vector2 moveValue = _context.ReadValue<Vector2>();
                int increment = 0;
                
                if (moveValue.x > 0 && previousMoveValue.x <= 0)
                {
                    increment = 1;
                }
                else if (moveValue.x < 0 && previousMoveValue.x >= 0)
                {
                    increment = -1;
                }
                
                // Increment selected character
                currentSelectedCharacter = (currentSelectedCharacter + increment + 4) % 4;
                
                // Update image
                characterImage.sprite = playerDatas[currentSelectedCharacter].characterSelectImage;
                
                // Update previous move value
                previousMoveValue = _context.ReadValue<Vector2>();
                
                UpdateImageTint();
            }

            public void OnSubmit(InputAction.CallbackContext _context)
            {
                if (!_context.started || !canSelectCharacter)
                {
                    UpdateImageTint();
                    canSelectCharacter = true; // Stops input from triggering when joining
                    return;
                }
                
                // Try to start game if a character is already selected
                if (hasSelectedCharacter)
                {
                    characterSelect.StartGame();
                    return;
                }
                
                // Select character
                hasSelectedCharacter = characterSelect.SelectCharacter(playerInput.devices[0], playerDatas[currentSelectedCharacter]);
                
                if (hasSelectedCharacter)
                {
                    selectedIndicator.SetActive(true);
                }
            }

            public void OnCancel(InputAction.CallbackContext _context)
            {
                if (!_context.started)
                {
                    return;
                }
                
                // Deselect character
                characterSelect.DeselectCharacter(playerInput.devices[0]);
                selectedIndicator.SetActive(false);
                
                // Leave
                if (!hasSelectedCharacter)
                {
                    Destroy(gameObject);
                }
                
                hasSelectedCharacter = false;
            }
#endregion

#region Private Functions
            private void UpdateImageTint()
            {
                if (CharacterSelect.selectedCharacters.ContainsValue(playerDatas[currentSelectedCharacter]))
                {
                    characterImage.color = Color.gray;
                }
                else
                {
                    characterImage.color = Color.white;
                }
            }
#endregion
        }
    }
}