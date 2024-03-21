// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: b22alesj
// Description: Controls the player score canvas
// --------------------------------
// ------------------------------*/

using System.Collections;
using Game.Backend;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Game {
    namespace UI {
        public class PlayerScoreUI : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private PlayerInput playerInput;
            [SerializeField] private Image playerImage;
            [SerializeField] private Image personalObjectiveImage;
            [SerializeField] private Image backgroundImage;
            [SerializeField] private TextMeshProUGUI coinsText;
            [SerializeField] private TextMeshProUGUI killsText;
            [SerializeField] private TextMeshProUGUI personalObjectiveText;
            [SerializeField] private TextMeshProUGUI pointsText;

            [Header("Settings")]
            [SerializeField] private string pointsPrefix;
            [SerializeField] private float startDelay;
            [SerializeField] private float countUpInterval;
            [SerializeField] private float countUpTime;
            
            [Header("Multipliers")]
            [SerializeField] private int coinPointMultiplier;
            [SerializeField] private int killPointMultiplier;

            private ScoreScreenUI scoreScreenUI;
            private int coroutinesRunning;
            private bool done;

            public void SetupUI(InputDevice _inputDevice, ScoreScreenUI _scoreScreenUI, PlayerData _playerData, Sprite _playerImage, Sprite _personalObjectiveImage)
            {
                playerInput.SwitchCurrentControlScheme(_inputDevice);
                
                scoreScreenUI = _scoreScreenUI;
                
                playerImage.sprite = _playerImage;
                personalObjectiveImage.sprite = _personalObjectiveImage;
                
                Color _color = _playerData.playerMaterialColor;
                _color.a = backgroundImage.color.a;
                backgroundImage.color = _color;
                
                int _pointsThisLevel = _playerData.currencyThisLevel * coinPointMultiplier + _playerData.killsThisLevel * killPointMultiplier + _playerData.personalObjectiveThisLevel * _playerData.personalObjectiveMultiplier + _playerData.pointsFromDialogue;
                _playerData.points += _pointsThisLevel;
                
                coinsText.text = (_playerData.currency - _playerData.currencyThisLevel - _playerData.currencyFromDialogue).ToString();
                killsText.text = (_playerData.kills - _playerData.killsThisLevel - _playerData.killsFromDialogue).ToString();
                personalObjectiveText.text = (_playerData.personalObjective - _playerData.personalObjectiveThisLevel - _playerData.personalObjectiveFromDialogue).ToString();
                pointsText.text = pointsPrefix + (_playerData.points - _pointsThisLevel - _playerData.pointsFromDialogue);
                
                StartCoroutine(CountUp(coinsText, _playerData.currency - _playerData.currencyThisLevel - _playerData.currencyFromDialogue, _playerData.currency));
                StartCoroutine(CountUp(killsText, _playerData.kills - _playerData.killsThisLevel - _playerData.killsFromDialogue, _playerData.kills));
                StartCoroutine(CountUp(personalObjectiveText, _playerData.personalObjective - _playerData.personalObjectiveThisLevel - _playerData.personalObjectiveFromDialogue, _playerData.personalObjective));
                StartCoroutine(CountUp(pointsText, _playerData.points - _pointsThisLevel - _playerData.pointsFromDialogue, _playerData.points, pointsPrefix));
                
                _playerData.ResetDialogueValues();
            }

            public void OnSubmitPressed(InputAction.CallbackContext _value)
            {
                scoreScreenUI.OnSubmitPressed(_value);
            }

            private void Update()
            {
                if (coroutinesRunning <= 0 && !done)
                {
                    done = true;
                    scoreScreenUI.DoneCountingUp();
                }
            }

            private IEnumerator CountUp(TextMeshProUGUI _textMesh, int _startValue, int _endValue, string _prefix = "")
            {
                if (_startValue == _endValue)
                {
                    yield break;
                }
                
                coroutinesRunning += 1;
                
                float _valuePerInterval = (_endValue - _startValue) / (countUpTime / countUpInterval);
                
                yield return new WaitForSeconds(startDelay);
                
                float _currentValue = _startValue;
                
                while (_currentValue < _endValue)
                {
                    _currentValue += _valuePerInterval;
                    _textMesh.text = _prefix + (int)Mathf.Floor(_currentValue);
                    yield return new WaitForSeconds(countUpInterval);
                }
                
                _textMesh.text = _prefix + _endValue;
                
                coroutinesRunning -= 1;
            }
        }
    }
}