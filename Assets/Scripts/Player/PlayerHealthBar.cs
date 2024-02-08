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
            [SerializeField] private GameObject playerHeartPrefab;
            [SerializeField] private Sprite fullHeart;
            [SerializeField] private Sprite halfHeart;
            [SerializeField] private Sprite emptyHeart;

            private List<Image> hearts;
            
            public void SetupHealthBar(int _maxHealth)
            {
                hearts = new List<Image>();
                
                for (int i = 0; i < _maxHealth / 2f; i++)
                {
                    hearts.Add(Instantiate(playerHeartPrefab, transform).GetComponent<Image>());
                }
                
                UpdateHealthBar(_maxHealth);
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
