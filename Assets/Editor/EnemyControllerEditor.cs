// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-30
// Author: alexa
// Description: Custom editor for enemy controller
// --------------------------------
// ------------------------------*/

using UnityEditor;
using UnityEngine;


namespace Game {
    namespace Enemy {
        [CustomEditor(typeof(EnemyController))]
        public class EnemyControllerEditor : Editor
        {
            private bool debugBool;
            
            public override void OnInspectorGUI()
            {
                EnemyController enemyController = (EnemyController)target;
                if (!target) return;
                
                base.OnInspectorGUI();
                
                debugBool = EditorGUILayout.BeginFoldoutHeaderGroup(debugBool, "Debug");
                if (debugBool)
                {
                    EditorGUILayout.BeginHorizontal();
                     EditorGUILayout.PrefixLabel("Current State");
                     if (Application.isPlaying)
                     {
                         EditorGUILayout.LabelField(enemyController.GetCurrentState().Name);
                     }
                     else
                     {
                         EditorGUILayout.LabelField("-");
                     }
                     EditorGUILayout.EndHorizontal();
                    
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }
    }
}
