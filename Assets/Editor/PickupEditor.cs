// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-02
// Author: alexa
// Description: Custom editor for pickup
// --------------------------------
// ------------------------------*/

using UnityEditor;


namespace Game {
    namespace Scene {
        [CustomEditor(typeof(Pickup))]
        public class PickupEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                Pickup pickup = (Pickup)target;
                if (!target) return;
                
                base.OnInspectorGUI();
                
                EditorGUILayout.Space();
                
                switch(pickup.PickupType)
                {
                    case Pickup.PickupTypes.QuestItem:
                        EditorGUILayout.LabelField("Quest Settings", EditorStyles.boldLabel);
                        var _weight = serializedObject.FindProperty("Weight");
                        EditorGUILayout.PropertyField(_weight);
                        var _interactionTime = serializedObject.FindProperty("InteractionTime");
                        EditorGUILayout.PropertyField(_interactionTime);
                        serializedObject.ApplyModifiedProperties();
                        break;
                    
                    case Pickup.PickupTypes.Gold:
                        EditorGUILayout.LabelField("Gold Settings", EditorStyles.boldLabel);
                        var _amount = serializedObject.FindProperty("Amount");
                        EditorGUILayout.PropertyField(_amount);
                        serializedObject.ApplyModifiedProperties();
                        break;
                }
            }
        }
    }
}
