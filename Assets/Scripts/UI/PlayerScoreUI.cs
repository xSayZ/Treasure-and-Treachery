// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: b22alesj
// Description: Controls the player score canvas
// --------------------------------
// ------------------------------*/

using System.Collections;
using System.Collections.Generic;
using Game.Backend;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Game {
    namespace UI {
        public class PlayerScoreUI : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private TextMeshProUGUI pointsText;
            [SerializeField] private TextMeshProUGUI coinsText;
            [SerializeField] private TextMeshProUGUI killsText;
            [SerializeField] private RawImage playerImage;
            
            [Header("Delays")]
            [SerializeField] private float startDelay;
            [SerializeField] private float pointsDelay;
            [SerializeField] private float coinsDelay;
            [SerializeField] private float killsDelay;
            
            [Header("Points")]
            [SerializeField] private int coinPointMultiplier;
            [SerializeField] private int killPointMultiplier;

            private int coroutinesRunning;
            private bool done;

            public void SetupUI(RenderTexture _renderTexture, PlayerData _playerData)
            {
                playerImage.texture = _renderTexture;
                
                int _pointsThisLevel = _playerData.currencyThisLevel * coinPointMultiplier + _playerData.killsThisLevel * killPointMultiplier;
                _playerData.points += _pointsThisLevel;
                
                pointsText.text = (_playerData.points - _pointsThisLevel).ToString();
                coinsText.text = (_playerData.currency - _playerData.currencyThisLevel).ToString();
                killsText.text = (_playerData.kills - _playerData.killsThisLevel).ToString();
                
                StartCoroutine(CountUp(pointsText, _playerData.points - _pointsThisLevel, _playerData.points, pointsDelay));
                StartCoroutine(CountUp(coinsText, _playerData.currency - _playerData.currencyThisLevel, _playerData.currency, coinsDelay));
                StartCoroutine(CountUp(killsText, _playerData.kills - _playerData.killsThisLevel, _playerData.kills, killsDelay));
            }

            private void Update()
            {
                if (coroutinesRunning <= 0 && !done)
                {
                    Debug.Log("Done");
                    done = true;
                    FindObjectOfType<ScoreScreenUI>().DoneCountingUp();
                }
            }

            private IEnumerator CountUp(TextMeshProUGUI _textMesh, int _startValue, int _endValue, float _delay)
            {
                coroutinesRunning += 1;
                
                yield return new WaitForSeconds(startDelay);
                
                int _currentValue = _startValue;
                
                while (_currentValue < _endValue)
                {
                    _currentValue += 1;
                    _textMesh.text = _currentValue.ToString();
                    yield return new WaitForSeconds(_delay);
                }
                
                coroutinesRunning -= 1;
            }
        }
    }
}
