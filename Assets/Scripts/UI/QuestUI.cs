// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-13
// Author: b22alesj
// Description: UI for quests
// --------------------------------
// ------------------------------*/

using System.Collections.Generic;
using Game.Core;
using Game.Quest;
using Game.Scene;
using TMPro;
using UnityEngine;


namespace Game {
    namespace UI {
        public class QuestUI : MonoBehaviour
        {
            [Header("Setup")]
            [SerializeField] private TextMeshProUGUI questText;
            
            [Header("Start")]
            [SerializeField] private string startingText;
            [SerializeField] private float startingShowTime;
            
            [Header("Events")]
            [SerializeField] private List<QuestUIItem> QuestUIItems;
            
            [System.Serializable]
            private class QuestUIItem
            {
                public Pickup Pickup;
                public string Text;
                public float ShowingTime;
            }
            
            private float showTimeLeft;
            
#region Unity Functions
            private void OnEnable()
            {
                QuestManager.OnItemPickedUp.AddListener(ItemPickedUp);
            }
            
            private void OnDisable()
            {
                QuestManager.OnItemPickedUp.RemoveListener(ItemPickedUp);
            }
            
            private void Start()
            {
                SetText(startingText, startingShowTime);
            }
            
            private void Update()
            {
                if (showTimeLeft <= 0)
                {
                    questText.gameObject.SetActive(false);
                }
                else
                {
                    showTimeLeft -= Time.deltaTime;
                }
            }
#endregion

#region Private Functions
            private void SetText(string _text, float _showTime)
            {
                questText.text = _text;
                showTimeLeft = _showTime;
                questText.gameObject.SetActive(true);
            }
            
            private void ItemPickedUp(int _playerIndex, Item _item)
            {
                for (int i = QuestUIItems.Count - 1; i >= 0; i--)
                {
                    if (QuestUIItems[i].Pickup.GetItem() == _item)
                    {
                        SetText(QuestUIItems[i].Text, QuestUIItems[i].ShowingTime);
                        QuestUIItems.Remove(QuestUIItems[i]);
                    }
                }
            }
#endregion
        }
    }
}
