using UnityEngine;

public class TestTargetGroupFollower : MonoBehaviour
{
    public Transform targetObject; // The object whose position we want to match

    void Update()
    {
        if (targetObject != null)
        {
            // Match the position of this object to the target object
            transform.position = targetObject.position;
        }
        else
        {
            Debug.LogWarning("Target object is not assigned!");
        }
    }
}