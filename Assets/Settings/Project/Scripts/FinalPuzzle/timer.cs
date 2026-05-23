using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float startTime = 300f;
    public TextMeshProUGUI timerText;

    private float time;
    private bool ended = false;
    private Vector2 originalAnchoredPosition;

    void Start()
    {
        time = startTime;
        originalAnchoredPosition = timerText.rectTransform.anchoredPosition;
        UpdateTimerUI();
    }

    void Update()
    {
        if (ended) return;

        time -= Time.deltaTime;

        if (time <= 0)
        {
            time = 0;
            ended = true;
            timerText.text = "SYSTEM FAILURE";
            timerText.color = Color.red;
            return;
        }

        UpdateTimerUI();
        DangerEffect();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void DangerEffect()
    {
        timerText.rectTransform.anchoredPosition = originalAnchoredPosition;

        if (time <= 60)
            timerText.color = Color.red;
        else
            timerText.color = Color.white;

        if (time <= 30)
        {
            float flash = Mathf.PingPong(Time.time * 5f, 1f);
            timerText.color = Color.Lerp(Color.red, Color.white, flash);

            float shakeX = Random.Range(-4f, 4f);
            float shakeY = Random.Range(-2f, 2f);
            timerText.rectTransform.anchoredPosition =
                originalAnchoredPosition + new Vector2(shakeX, shakeY);
        }

    }
    public void RemoveTime(float amount)
    {
        time -= amount;

        if (time < 0)
            time = 0;

        UpdateTimerUI();
    }
}