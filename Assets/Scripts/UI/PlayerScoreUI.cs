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
using UnityEngine.UI;


namespace Game {
    namespace UI {
        public class PlayerScoreUI : MonoBehaviour
        {
            [Header("Setup")]
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
            [SerializeField] private float countUpTime;
            
            [Header("Multipliers")]
            [SerializeField] private int coinPointMultiplier;
            [SerializeField] private int killPointMultiplier;
            
            private int coroutinesRunning;
            private bool done;

            public void SetupUI(PlayerData _playerData, Sprite _playerImage, Sprite _personalObjectiveImage)
            {
                playerImage.sprite = _playerImage;
                personalObjectiveImage.sprite = _personalObjectiveImage;
                
                Color _color = _playerData.playerMaterialColor;
                _color.a = backgroundImage.color.a;
                backgroundImage.color = _color;
                
                int _pointsThisLevel = _playerData.currencyThisLevel * coinPointMultiplier + _playerData.killsThisLevel * killPointMultiplier + _playerData.personalObjectiveThisLevel * _playerData.personalObjectiveMultiplier;
                _playerData.points += _pointsThisLevel;
                
                coinsText.text = (_playerData.currency - _playerData.currencyThisLevel).ToString();
                killsText.text = (_playerData.kills - _playerData.killsThisLevel).ToString();
                personalObjectiveText.text = (_playerData.personalObjective - _playerData.personalObjectiveThisLevel).ToString();
                pointsText.text = pointsPrefix + (_playerData.points - _pointsThisLevel);
                
                StartCoroutine(CountUp(coinsText, _playerData.currency - _playerData.currencyThisLevel, _playerData.currency, countUpTime / _playerData.currencyThisLevel));
                StartCoroutine(CountUp(killsText, _playerData.kills - _playerData.killsThisLevel, _playerData.kills, countUpTime / _playerData.killsThisLevel));
                StartCoroutine(CountUp(personalObjectiveText, _playerData.personalObjective - _playerData.personalObjectiveThisLevel, _playerData.personalObjective, countUpTime / _playerData.personalObjectiveThisLevel));
                StartCoroutine(CountUp(pointsText, _playerData.points - _pointsThisLevel, _playerData.points, countUpTime / _pointsThisLevel, pointsPrefix));
            }

            private void Update()
            {
                if (coroutinesRunning <= 0 && !done)
                {
                    done = true;
                    FindObjectOfType<ScoreScreenUI>().DoneCountingUp();
                }
            }

            private IEnumerator CountUp(TextMeshProUGUI _textMesh, int _startValue, int _endValue, float _delay, string _prefix = "")
            {
                coroutinesRunning += 1;
                
                yield return new WaitForSeconds(startDelay);
                
                int _currentValue = _startValue;
                
                while (_currentValue < _endValue)
                {
                    _currentValue += 1;
                    _textMesh.text = _prefix + _currentValue;
                    yield return new WaitForSeconds(_delay);
                }
                
                coroutinesRunning -= 1;
            }
        }
    }
}
