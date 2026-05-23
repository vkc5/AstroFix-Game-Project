using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPopup;
    public AudioSource gameOverAudio;

    private bool isGameOver = false;

    void Start()
    {
        if (gameOverPopup != null)
            gameOverPopup.SetActive(false);
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Time.timeScale = 0f;

        if (gameOverPopup != null)
            gameOverPopup.SetActive(true);

        if (gameOverAudio != null)
            gameOverAudio.Play();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void TryAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}