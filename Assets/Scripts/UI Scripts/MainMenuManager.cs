using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject menuPanel;
    
    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    public void OpenControls()
    {
        if(menuPanel)
        {
            menuPanel.SetActive(false);
        }
        if(controlsPanel)
        {
            controlsPanel.SetActive(true);
        }
    }

    public void CloseControls()
    {
        if(menuPanel)
        {
            menuPanel.SetActive(true);
        }
        if(controlsPanel)
        {
            controlsPanel.SetActive(false);
        }
    }
}
