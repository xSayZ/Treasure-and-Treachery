// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: b22alesj
// Description: UI for quests
// --------------------------------
// ------------------------------*/

using System;
using Game.Audio;
using Game.Quest;
using TMPro;
using UnityEngine;


namespace Game {
    namespace UI {
        public class QuestUI : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private TextMeshProUGUI mainQuestText;
            [SerializeField] private Animator mainSpriteAnimator;
            [SerializeField] private Animator mainMaskAnimator;
            [SerializeField] private TextMeshProUGUI sideQuestText;
            [SerializeField] private Animator sideSpriteAnimator;
            [SerializeField] private Animator sideMaskAnimator;
            
            [Header("Settings")]
            [SerializeField] private float mainShowTime;
            [SerializeField] private string mainStartingText;
            [SerializeField] private string sideStartingText;
            [SerializeField] private float sideStartingDelay;
            [SerializeField] private float sideGoneTime;

            [Header("Audio")] 
            [SerializeField] private UIAudio uiAudio;

            private float mainShowTimeLeft;
            private bool mainShowRunning;
            private bool sideShowing;
            private bool sideGoneRunning;
            private float sideGoneTimeLeft;
            private string sideText;
            private bool currentSideStartingDelayRunning;
            private float currentSideStartingDelay;

#region Unity Functions
            private void OnEnable()
            {
                QuestManager.OnKillQuestProgress.AddListener(UpdateSideScrollText);
            }

            private void OnDisable()
            {
                QuestManager.OnKillQuestProgress.RemoveListener(UpdateSideScrollText);
            }

            private void Start()
            {
                DisplayMainScroll();
            }

            private void Update()
            {
                if (mainShowTimeLeft <= 0 && mainShowRunning)
                {
                    mainSpriteAnimator.SetTrigger("Close");
                    mainMaskAnimator.SetTrigger("Close");
                    mainShowRunning = false;

                    currentSideStartingDelayRunning = true;

                    try
                    {
                        uiAudio.ScrollCloseAudio();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("[{DialogueAudio}]: Error Exception " + e);
                    }
                    
                }
                else if (mainShowRunning)
                {
                    mainShowTimeLeft -= Time.deltaTime;
                }
                
                if (currentSideStartingDelayRunning)
                {
                    currentSideStartingDelay += Time.deltaTime;
                    
                    if (currentSideStartingDelay >= sideStartingDelay)
                    {
                        currentSideStartingDelayRunning = false;
                        UpdateSideScroll(sideStartingText);
                    }
                }
                
                if (sideGoneTimeLeft <= 0 && sideGoneRunning)
                {
                    sideGoneRunning = false;
                    OpenSideScroll();
                }
                else if (sideGoneRunning)
                {
                    sideGoneTimeLeft -= Time.deltaTime;
                }
            }
#endregion

#region Public Functions
            public void UpdateSideScroll(string _text)
            {
                sideText = _text;
                
                if (sideShowing)
                {
                    if (!sideGoneRunning)
                    {
                        sideSpriteAnimator.SetTrigger("Close");
                        sideMaskAnimator.SetTrigger("Close");
                    }
                    
                    sideGoneTimeLeft = sideGoneTime;
                    sideGoneRunning = true;
                }
                else
                {
                    OpenSideScroll();
                }
                
                sideShowing = true;
            }
#endregion

#region Private Functions
            private void DisplayMainScroll()
            {
                mainQuestText.text = mainStartingText;
                mainShowTimeLeft = mainShowTime;
                
                mainSpriteAnimator.SetTrigger("Open");
                mainMaskAnimator.SetTrigger("Open");
                
                mainShowRunning = true;
                
                try
                {
                    uiAudio.ScrollOpenAudio();
                }
                catch (Exception e)
                {
                    Debug.LogError("[{DialogueAudio}]: Error Exception " + e);
                }

            }

            private void OpenSideScroll()
            {
                sideSpriteAnimator.SetTrigger("Open"); 
                sideMaskAnimator.SetTrigger("Open");
                
                try
                {
                    uiAudio.UiPingAudio();
                }
                catch (Exception e)
                {
                    Debug.LogError("[{DialogueAudio}]: Error Exception " + e);
                }

                
                sideQuestText.text = sideText;
            }

            private void UpdateSideScrollText(string _textBefore, int _amount, string _textAfter)
            {
                sideQuestText.text = _textBefore.Trim() + " " + _amount + " " + _textAfter.Trim();
            }
#endregion
        }
    }
}