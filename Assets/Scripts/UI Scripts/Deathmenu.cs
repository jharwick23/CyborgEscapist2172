using UnityEngine;
using UnityEngine.SceneManagement;

public class Deathmenu : MonoBehaviour
{
    // References
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private PlayerHealth playerHealth;

    void Start()
    {
        if(!inputHandler)
        {
            inputHandler = FindFirstObjectByType<InputHandler>();
        }
        if(deathPanel)
        {
            deathPanel.SetActive(false);
        }
        if(!playerHealth)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }

        Time.timeScale = 1f;
    }

    // Logic for pausing
    public void EnableDeathMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        if(deathPanel)
        {
            deathPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Death Panel not found");
        }

        Time.timeScale = 0f;
        inputHandler.DisableInputs();
    }

    // Logic to restart the level
    public void OnRestartLevel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        RespawnResume();
        playerHealth.Spawn();
    }

    void RespawnResume()
    {
        if(deathPanel)
        {
            deathPanel.SetActive(false);
        }
        else
        {
            Debug.Log("Death Panel not found");
        }

        Time.timeScale = 1f;
        inputHandler.EnableInputs();
    }

    public void OnQuitButton()
    {
        Time.timeScale = 1f;
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}
