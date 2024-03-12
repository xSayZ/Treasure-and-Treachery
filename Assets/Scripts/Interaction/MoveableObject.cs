// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-11
// Author: Felix
// Description: Treasure and Treachery
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;


namespace Game {
    namespace Interactable {
        public class MoveableObject : MonoBehaviour
        {
            [Header("References")]
            public Collider collider;
            
            [Header("Settings")]
            [SerializeField] private float speed = 5f;
            [SerializeField] private bool allowRotation = false;
            
            private bool isMoving = false;
            private Transform targetPosition;
            private Transform startTransform;
            
            private void Start()
            {
                startTransform = transform;
                if(collider!= null)
                    collider.gameObject.SetActive(false);
            }
            private void Update()
            {
                InterpolatePosition();
            }

            public void MoveToPosition(Transform _targetPosition)
            {
                targetPosition = _targetPosition;
            }
            
            public void IsMoving(bool _isMoving)
            {
                isMoving = _isMoving;
            }
            
            private void InterpolatePosition()
            {
                Transform _transform;
                if (isMoving) {
                    (_transform = transform).position = Vector3.Lerp(transform.position, targetPosition.position, speed * Time.deltaTime);
                    if(allowRotation)
                        transform.rotation = Quaternion.Lerp(_transform.rotation, targetPosition.rotation, speed * Time.deltaTime);
                }
                else {
                    (_transform = transform).position = Vector3.Lerp(transform.position, startTransform.position, speed * Time.deltaTime);
                    if (allowRotation)
                        transform.rotation = Quaternion.Lerp(_transform.rotation, startTransform.rotation, speed * Time.deltaTime);
                    if(collider!= null)
                        collider.gameObject.SetActive(true);
                }
            }
            
        }
    }
}
