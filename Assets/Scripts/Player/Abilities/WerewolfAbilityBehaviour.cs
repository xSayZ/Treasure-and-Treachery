// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-22
// Author: b22alesj
// Description: The werewolf's ability
// --------------------------------
// ------------------------------*/

using Game.Audio;
using UnityEngine;
using UnityEngine.UI;


namespace Game {
    namespace Player {
        public class WerewolfAbilityBehaviour : PlayerAbilityBehaviour
        {
            [Header("Setup")]
            [SerializeField] private Slider wrathSlider;
            
            [Header("Wrath")]
            [SerializeField] private float percentagePerKill;
            [SerializeField] private float decayGracePeriod;
            [SerializeField] private float decayTime;
            [SerializeField] private float decayAmount;
            
            [Header("Enraged")]
            [SerializeField] private float meleeAttackCooldownMultiplier;
            [SerializeField] private float moveSpeedMultiplier;
            
            [Header("Dash Heal")]
            [SerializeField] private int dashHealAmount;
            [SerializeField] private float dashHealWrathLost;

            [Header("Audio")] 
            [SerializeField] private DialogueAudio _dialogueAudio;
            
            private float wrathPercentage;
            private float currentDecayGracePeriod;
            private float currentDecayTime;
            private bool isEnraged;

            protected override void Setup()
            {
                playerController.PlayerAttackBehaviour.OnKill.AddListener(EnemyKilled);
                playerController.PlayerMovementBehaviour.OnDash.AddListener(Dash);
            }

            protected override void OnDisable()
            {
                base.OnDisable();
                
                if (playerController)
                {
                    playerController.PlayerAttackBehaviour.OnKill.RemoveListener(EnemyKilled);
                    playerController.PlayerMovementBehaviour.OnDash.RemoveListener(Dash);
                }
            }

            protected override void OnDashKill(bool _stunned)
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
                
                if (wrathPercentage >= 50)
                {
                    SetEnraged(true);
                    isEnraged = true;
                }
                else if (wrathPercentage <= 0)
                {
                    SetEnraged(false);
                    isEnraged = false;
                }
            }

            private void SetEnraged(bool _active)
            {
                if (_active)
                {
                    playerController.PlayerAttackBehaviour.MeleeAttackCooldownMultiplier = meleeAttackCooldownMultiplier;
                    playerController.PlayerMovementBehaviour.MoveSpeedMultiplier = moveSpeedMultiplier;
                }
                else
                {
                    playerController.PlayerAttackBehaviour.MeleeAttackCooldownMultiplier = 1f;
                    playerController.PlayerMovementBehaviour.MoveSpeedMultiplier = 1f;
                }
            }

            private void EnemyKilled()
            {
                currentDecayGracePeriod = decayGracePeriod;
                currentDecayTime = decayTime;
                AddWrath(percentagePerKill);
                _dialogueAudio.UpdateWereWolfRageAudio(wrathPercentage);
            }

            private void Dash()
            {
                if (wrathPercentage >= dashHealWrathLost)
                {
                    if (playerController.Heal(dashHealAmount))
                    {
                        AddWrath(-dashHealWrathLost);
                    }
                }
            }
        }
    }
}