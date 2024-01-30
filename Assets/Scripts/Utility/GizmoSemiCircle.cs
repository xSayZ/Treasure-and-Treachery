// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Operation_Donken
// Source: https://gist.github.com/luisparravicini/50d044a20c67f0615fdd28accd939df4
// --------------------------------
// ------------------------------*/


using UnityEngine;


namespace Utility {
    namespace Gizmos {
        public static class GizmoSemiCircle
        {
            public static void DrawWireArc(Vector3 position, Vector3 dir, float anglesRange, float radius, float maxSteps = 20)
            {
                var srcAngles = GetAnglesFromDir(position, dir);
                var initialPos = position;
                var posA = initialPos;
                var stepAngles = anglesRange / maxSteps;
                var angle = srcAngles - anglesRange / 2;
                for (var i = 0; i <= maxSteps; i++)
                {
                    var rad = Mathf.Deg2Rad * angle;
                    var posB = initialPos;
                    posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));

                    UnityEngine.Gizmos.DrawLine(posA, posB);

                    angle += stepAngles;
                    posA = posB;
                }
                UnityEngine.Gizmos.DrawLine(posA, initialPos);
            }

            private static float GetAnglesFromDir(Vector3 position, Vector3 dir)
            {
                var forwardLimitPos = position + dir;
                var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPos.z - position.z, forwardLimitPos.x - position.x);

                return srcAngles;
            }
        }
    }
}
