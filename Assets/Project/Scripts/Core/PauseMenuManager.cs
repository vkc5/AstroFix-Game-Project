using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private bool isPaused = false;
    private bool pauseSceneLoaded = false;
    public string pauseSceneName = "Pause Menu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                OpenPauseMenu();
            else
                ClosePauseMenu();
        }
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

    public void ClosePauseMenu()
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
}