using Game.Backend;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.loadStoredPlayerData = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
