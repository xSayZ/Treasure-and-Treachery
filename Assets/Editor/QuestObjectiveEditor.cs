// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-21
// Author: b22alesj
// Description: Custom editor for quest objective
// --------------------------------
// ------------------------------*/

using Game.Quest;
using UnityEditor;


namespace Game {
    namespace Scene {
        [CustomEditor(typeof(QuestObjective))]
        public class QuestObjectiveEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                QuestObjective questObjective = (QuestObjective)target;
                if (!target) return;
                
                base.OnInspectorGUI();
                
                EditorGUILayout.Space();
                
                switch(questObjective.QuestType)
                {
                    case QuestObjective.QuestTypes.Fetch:
                        EditorGUILayout.LabelField("Fetch Settings", EditorStyles.boldLabel);
                        var _requiredPickups = serializedObject.FindProperty("RequiredPickups");
                        EditorGUILayout.PropertyField(_requiredPickups);
                        serializedObject.ApplyModifiedProperties();
                        break;
                    
                    case QuestObjective.QuestTypes.Kill:
                        EditorGUILayout.LabelField("Kill Settings", EditorStyles.boldLabel);
                        var _requiredKills = serializedObject.FindProperty("RequiredKills");
                        EditorGUILayout.PropertyField(_requiredKills);
                        var _counterTextBeforeNumber = serializedObject.FindProperty("CounterTextBeforeNumber");
                        EditorGUILayout.PropertyField(_counterTextBeforeNumber);
                        var _counterTextAfterNumber = serializedObject.FindProperty("CounterTextAfterNumber");
                        EditorGUILayout.PropertyField(_counterTextAfterNumber);
                        serializedObject.ApplyModifiedProperties();
                        break;
                    
                    case QuestObjective.QuestTypes.Time:
                        EditorGUILayout.LabelField("Time Settings", EditorStyles.boldLabel);
                        var _waitTime = serializedObject.FindProperty("WaitTime");
                        EditorGUILayout.PropertyField(_waitTime);
                        serializedObject.ApplyModifiedProperties();
                        break;
                }
            }
        }
    }
}
