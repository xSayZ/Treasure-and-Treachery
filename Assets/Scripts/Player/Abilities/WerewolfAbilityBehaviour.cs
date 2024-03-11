// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-22
// Author: b22alesj
// Description: The werewolf's ability
// --------------------------------
// ------------------------------*/

using Game.Audio;
using Game.Quest;
using UnityEngine;
using UnityEngine.UI;


namespace Game {
    namespace Player {
        public class WerewolfAbilityBehaviour : PlayerAbilityBehaviour
        {
            [Header("Setup")]
            [SerializeField] private Slider wrathSlider;
            
            [Header("Wrath")]
            [Range(0f, 100f)]
            [SerializeField] private float percentagePerKill;
            [SerializeField] private float decayGracePeriod;
            [SerializeField] private float decayTime;
            [SerializeField] private float decayAmount;

            [Header("Enraged")]
            [Range(0f, 100f)]
            [SerializeField] private float percentageToBecomeEnraged;
            [Range(0f, 100f)]
            [SerializeField] private float percentageToLoseEnraged;
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
            private float enragedScore;

            protected override void Setup()
            {
                playerController.PlayerOverheadUIBehaviour.UpdatePersonalObjective(playerController.PlayerData.personalObjective, 0);
                
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
                EnemyKilled(_stunned);
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
                
                if (isEnraged)
                {
                    enragedScore += Time.deltaTime;
                }
                
                while (enragedScore >= 1)
                {
                    enragedScore -= 1;
                    
                    playerController.PlayerData.personalObjective += 1;
                    playerController.PlayerData.personalObjectiveThisLevel += 1;
                    playerController.PlayerOverheadUIBehaviour.UpdatePersonalObjective(playerController.PlayerData.personalObjective, 1);
                    QuestManager.PersonalObjectiveScoreUpdated(playerController.PlayerIndex, playerController.PlayerData.personalObjective);
                }
            }

            private void AddWrath(float _amount)
            {
                wrathPercentage += _amount;
                wrathPercentage = Mathf.Clamp(wrathPercentage, 0f, 100f);
                wrathSlider.value = wrathPercentage / 100f;
                
                if (wrathPercentage >= percentageToBecomeEnraged)
                {
                    SetEnraged(true);
                }
                else if (wrathPercentage <= percentageToLoseEnraged)
                {
                    SetEnraged(false);
                }
            }

            private void SetEnraged(bool _active)
            {
                if (_active)
                {
                    if (!isEnraged)
                    {
                        // Became enraged
                        playerController.PlayerAttackBehaviour.MeleeAttackCooldownMultiplier = meleeAttackCooldownMultiplier;
                        playerController.PlayerMovementBehaviour.MoveSpeedMultiplier = moveSpeedMultiplier;
                        playerController.PlayerMovementBehaviour.DisableDashMove = true;
                    }
                }
                else
                {
                    if (isEnraged)
                    {
                        // Lost enraged
                        playerController.PlayerAttackBehaviour.MeleeAttackCooldownMultiplier = 1f;
                        playerController.PlayerMovementBehaviour.MoveSpeedMultiplier = 1f;
                        playerController.PlayerMovementBehaviour.DisableDashMove = false;
                    }
                }
                
                isEnraged = _active;
            }

            private void EnemyKilled(bool _stunned)
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