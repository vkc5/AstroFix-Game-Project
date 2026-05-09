using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSceneLoader : MonoBehaviour
{
    public static PauseSceneLoader Instance;

    private bool isPaused = false;
    private bool pauseSceneLoaded = false;

    public bool canPause = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // NEW
        if (!canPause)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!isPaused)
            OpenPauseMenu();
        else
            ResumeGame();
    }

    public void OpenPauseMenu()
    {
        isPaused = true;
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (!pauseSceneLoaded)
        {
            SceneManager.LoadScene("Pause Menu", LoadSceneMode.Additive);
            pauseSceneLoaded = true;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (pauseSceneLoaded)
        {
            SceneManager.UnloadSceneAsync("Pause Menu");
            pauseSceneLoaded = false;
        }
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}