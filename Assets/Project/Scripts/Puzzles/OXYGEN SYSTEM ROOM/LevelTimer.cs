using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    public float levelTime = 120f;
    public Text timerText;

    public Color normalColor = Color.white;
    public Color warningColor = Color.red;

    private bool timerRunning = true;

    [Header("Warning Sound")]
    public AudioSource warningAudio;

    private bool warningStarted = false;
    void Update()
    {
        if (!timerRunning) return;

        levelTime -= Time.deltaTime;

        if (levelTime <= 30 && !warningStarted)
        {
            warningStarted = true;

            if (warningAudio != null)
                warningAudio.Play();
        }

        if (levelTime <= 0)
        {
            levelTime = 0;
            timerRunning = false;

            if (warningAudio != null)
                warningAudio.Stop();

            FindFirstObjectByType<GameOverManager>().GameOver();
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(levelTime / 60);
        int seconds = Mathf.FloorToInt(levelTime % 60);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

        if (levelTime <= 30)
            timerText.color = warningColor;
        else
            timerText.color = normalColor;
    }

    public void StopTimer()
    {
        timerRunning = false;

        if (warningAudio != null)
            warningAudio.Stop();
    }
}