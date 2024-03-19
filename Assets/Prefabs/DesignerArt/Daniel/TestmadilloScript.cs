// /*------------------------------
// --------------------------------
// Creation Date: 2024-03-19
// Author: Daniel
// Description: Treasure and Treachery
// --------------------------------
// ------------------------------*/

using UnityEngine;
using UnityEngine.Events;

public class TestmadilloScript : MonoBehaviour
{
    public Rigidbody objectToCheck; // Reference to the object you want to check if it's moving
    public GameObject objectToActivate; // Reference to the object whose events you want to trigger
    public float speedThreshold = 1.0f; // Speed threshold above which the object is considered moving
    public UnityEvent onObjectStartMoving; // Event to trigger when the object starts moving
    public UnityEvent onObjectStopMoving; // Event to trigger when the object stops moving

    private bool isObjectMoving = false; // Flag to track if the object is moving

    void Update()
    {
        // Check if the objectToCheck is moving
        if (objectToCheck != null)
        {
            // Check if the object's velocity magnitude is greater than the speed threshold
            if (objectToCheck.velocity.magnitude > speedThreshold)
            {
                if (!isObjectMoving)
                {
                    // Object started moving
                    isObjectMoving = true;
                    // Trigger event for object start moving
                    onObjectStartMoving.Invoke();
                }
            }
            else
            {
                if (isObjectMoving)
                {
                    // Object stopped moving
                    isObjectMoving = false;
                    // Trigger event for object stop moving
                    onObjectStopMoving.Invoke();
                }
            }
        }
        else
        {
            Debug.LogWarning("objectToCheck is null.");
        }
    }
}
