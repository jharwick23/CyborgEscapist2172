using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // References
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private InputHandler inputHandler;

    // Variables
    private bool isPaused = false;

    void Start()
    {
        if(!inputHandler)
        {
            inputHandler = FindFirstObjectByType<InputHandler>();
        }
        if(pausePanel)
        {
            pausePanel.SetActive(false);
        }
        if(controlsPanel)
        {
            controlsPanel.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called by pressing escape
    public void PerformPause()
    {
        if(isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    // Logic for pausing
    void Pause()
    {
        if(pausePanel)
        {
            pausePanel.SetActive(true);
        }
        else
        {
            Debug.Log("Pause Panel not found");
        }

        Time.timeScale = 0f;     
        isPaused = true;
        inputHandler.DisableInputs();
    }

    void Resume()
    {
        if(pausePanel)
        {
            pausePanel.SetActive(false);
        }
        else
        {
            Debug.Log("Pause Panel not found");
        }

        Time.timeScale = 1f;     
        isPaused = false;
        inputHandler.EnableInputs();
    }

    public void OnResumeButton()
    {
        Resume();
    }

    public void OnQuitButton()
    {
        Time.timeScale = 1f;
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    public void OpenControls()
    {
        if(pausePanel)
        {
            pausePanel.SetActive(false);
        }
        if(controlsPanel)
        {
            controlsPanel.SetActive(true);
        }
    }

    public void CloseControls()
    {
        if(pausePanel)
        {
            pausePanel.SetActive(true);
        }
        if(controlsPanel)
        {
            controlsPanel.SetActive(false);
        }
    }
}
