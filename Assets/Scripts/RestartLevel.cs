using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    // restart temporary
    [Header("Press R For Restart")]
<<<<<<< Updated upstream
    public PlayerInput restartInput;
    
    public void OnRestartLevel(InputAction.CallbackContext context)
    {
        if (context.started)
=======
    public KeyCode restartInput;
    

    public void Update()
    {
        if (Input.GetKeyDown(restartInput))
>>>>>>> Stashed changes
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
