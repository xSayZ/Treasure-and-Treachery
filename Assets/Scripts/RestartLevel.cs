using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    // restart temporary
    [Header("Press R For Restart")]
    public PlayerInput restartInput;
    
    public void OnRestartLevel(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
