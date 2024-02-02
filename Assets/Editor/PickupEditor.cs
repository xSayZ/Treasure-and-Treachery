// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-30
// Author: Felix
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using UnityEditor;
using UnityEngine;


namespace Game {
    namespace Scene {
        [CustomEditor(typeof(Pickup))]
        public class PickupEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                Pickup pickup = (Pickup)target;

                DrawDefaultInspector();

                switch(pickup.pickupType){
                    case Pickup.PickupType.Gold:
                        pickup.Amount = EditorGUILayout.IntField("Amount", pickup.Amount);
                        break;
                    case Pickup.PickupType.Objective:
                        pickup.Weight = EditorGUILayout.IntField("Weight", pickup.Weight);
                        break;
                }
            }
        }
    }
}
