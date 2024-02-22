// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-22
// Author: b22alesj
// Description: The werewolves ability
// --------------------------------
// ------------------------------*/

using UnityEngine;
using UnityEngine.UI;


namespace Game {
    namespace Player {
        public class WerewolfAbilityBehaviour : PlayerAbilityBehaviour
        {
            [Header("Setup")]
            [SerializeField] private Slider wrathSlider;
            
            [Header("Settings")]
            [SerializeField] private float percentagePerKill;
            [SerializeField] private float decayGracePeriod;
            [SerializeField] private float decayTime;
            [SerializeField] private float decayAmount;
            
            private float wrathPercentage;
            private float currentDecayGracePeriod;
            private float currentDecayTime;

            protected override void Setup()
            {
                playerController.PlayerAttackBehaviour.OnKill.AddListener(EnemyKilled);
            }

            protected override void OnDisable()
            {
                base.OnDisable();
                
                if (playerController)
                {
                    playerController.PlayerAttackBehaviour.OnKill.RemoveListener(EnemyKilled);
                }
            }

            protected override void OnDashKill()
            {
                EnemyKilled();
            }

            private void Update()
            {
                if (currentDecayGracePeriod <= 0)
                {
                    currentDecayTime -= Time.deltaTime;
                    
                    while (currentDecayTime < 0)
                    {
                        AddWrath(-decayAmount);
                        currentDecayTime += decayTime;
                    }
                }
                else
                {
                    currentDecayGracePeriod -= Time.deltaTime;
                }
            }

            private void AddWrath(float _amount)
            {
                wrathPercentage += _amount;
                wrathPercentage = Mathf.Clamp(wrathPercentage, 0f, 100f);
                wrathSlider.value = wrathPercentage / 100f;
            }

            private void EnemyKilled()
            {
                currentDecayGracePeriod = decayGracePeriod;
                currentDecayTime = decayTime;
                AddWrath(percentagePerKill);
            }
        }
    }
}