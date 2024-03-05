// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-08
// Author: b22alesj
// Description: Updated the players health bar
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Game {
    namespace Player {
        public class PlayerHealthBar : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private GameObject playerHeartPrefab;
            [SerializeField] private Sprite fullHeart;
            [SerializeField] private Sprite halfHeart;
            [SerializeField] private Sprite emptyHeart;
            
            [Header("Settings")]
            [SerializeField] private float radius;
            [Range(0f, 360f)]
            [SerializeField] private float spreadAngle;
            
            private List<Image> hearts;

            public void SetupHealthBar(int _maxHealth, int _currenHealth)
            {
                hearts = new List<Image>();
                
                float _individualSpreadAngle = spreadAngle / (Mathf.Floor(_maxHealth / 2f) - 1);
                float _currentAngle = spreadAngle / 2;
                
                for (int i = 0; i < _maxHealth / 2f; i++)
                {
                    // This quaternion math is very fucked but it dose the job
                    Vector3 _spawnPosition = Quaternion.AngleAxis(_currentAngle + 90, Vector3.up)* new Vector3(radius, 0, 0);
                    _spawnPosition = new Vector3(_spawnPosition.x, _spawnPosition.z, _spawnPosition.y);
                    
                    GameObject _spawnedHeart = Instantiate(playerHeartPrefab, transform);
                    hearts.Add(_spawnedHeart.GetComponent<Image>());
                    
                    _spawnedHeart.transform.localPosition = _spawnPosition;
                    _currentAngle -= _individualSpreadAngle;
                }
                
                UpdateHealthBar(_currenHealth);
            }

            public void UpdateHealthBar(int _currenHealth)
            {
                for (int i = hearts.Count - 1; i >= 0; i--)
                {
                    if ((i + 1) * 2 > _currenHealth)
                    {
                        if ((i + 1) * 2 - 1 > _currenHealth)
                        {
                            hearts[i].sprite = emptyHeart;
                        }
                        else
                        {
                            hearts[i].sprite = halfHeart;
                        }
                    }
                    else
                    {
                        hearts[i].sprite = fullHeart;
                    }
                }
            }
        }
    }
}