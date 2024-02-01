// /*------------------------------
// --------------------------------
// Creation Date: 2024-01-29
// Author: b22alesj
// Description: Extra functions for drawing gizmos
// Initial source: https://gist.github.com/luisparravicini/50d044a20c67f0615fdd28accd939df4
// --------------------------------
// ------------------------------*/


using UnityEngine;


namespace Utility {
    namespace Gizmos {
        public static class GizmosExtra
        {
            public static void DrawArc(Vector3 _position, Vector3 _direction, float _anglesRange, float _radius, int _maxSteps = 20)
            {
                DrawWireArc(_position, _direction, _anglesRange, _radius, false, 0, _maxSteps);
            }
            
            public static void DrawCircle(Vector3 _position, float _radius, int _maxSteps = 50)
            {
                DrawWireArc(_position, new Vector3(1, 0, 0), 360, _radius, false, 0, _maxSteps);
            }
            
            public static void DrawSemiCircle(Vector3 _position, Vector3 _direction, float _anglesRange, float _radius, int _maxSteps = 20)
            {
                DrawWireArc(_position, _direction, _anglesRange, _radius, true, 0, _maxSteps);
            }
            
            public static void DrawHollowSemiCircle(Vector3 _position, Vector3 _direction, float _anglesRange, float _innerRadius, float _outerRadius, int _maxSteps = 20)
            {
                DrawWireArc(_position, _direction, _anglesRange, _innerRadius, false, 0, _maxSteps);
                DrawWireArc(_position, _direction, _anglesRange, _outerRadius, true, _innerRadius, _maxSteps);
            }
            
            private static void DrawWireArc(Vector3 _position, Vector3 _direction, float _anglesRange, float _radius, bool _startLines, float _startLinesOffset, int _maxSteps = 20)
            {
                // Calculate angle and angle step
                float _angleFormDirection = GetAngleFromDirection(_position, _direction);
                float _angle = _angleFormDirection - _anglesRange / 2;
                float _angleStep = _anglesRange / _maxSteps;
                
                // First start line and offset
                float _angleInRadians = Mathf.Deg2Rad * _angle;
                Vector3 posA;
                if (_startLines)
                {
                    posA = _position + new Vector3(_startLinesOffset * Mathf.Cos(_angleInRadians), 0, _startLinesOffset * Mathf.Sin(_angleInRadians));
                }
                else
                {
                    posA = _position + new Vector3(_radius * Mathf.Cos(_angleInRadians), 0, _radius * Mathf.Sin(_angleInRadians));
                }
                
                // Every arc step
                for (int i = 0; i <= _maxSteps; i++)
                {
                    Vector3 posB = _position;
                    posB += new Vector3(_radius * Mathf.Cos(_angleInRadians), 0, _radius * Mathf.Sin(_angleInRadians));
                    UnityEngine.Gizmos.DrawLine(posA, posB);
                    
                    _angle += _angleStep;
                    _angleInRadians = Mathf.Deg2Rad * _angle;
                    posA = posB;
                }
                
                // Second start line and offset
                if (_startLines)
                {
                    _angle -= _angleStep;
                    _angleInRadians = Mathf.Deg2Rad * _angle;
                    UnityEngine.Gizmos.DrawLine(posA, _position + new Vector3(_startLinesOffset * Mathf.Cos(_angleInRadians), 0, _startLinesOffset * Mathf.Sin(_angleInRadians)));
                }
            }
            
            private static float GetAngleFromDirection(Vector3 _position, Vector3 _direction)
            {
                Vector3 _forwardPosition = _position + _direction;
                float _angle = Mathf.Rad2Deg * Mathf.Atan2(_forwardPosition.z - _position.z, _forwardPosition.x - _position.x);
                return _angle;
            }
        }
        
        
        public static class GizmoSemiCircle
        {
            public static void DrawWireArc(Vector3 position, Vector3 dir, float anglesRange, float radius, float maxSteps = 20)
            {
                GizmosExtra.DrawSemiCircle(position, dir, anglesRange, radius);
                Debug.LogWarning("[Programmer Note] GizmoSemiCircle class has been deprecated, please use GizmosExtra class instead");
            }
        }
    }
}
