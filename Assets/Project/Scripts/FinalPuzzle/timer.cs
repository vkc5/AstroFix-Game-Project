using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float startTime = 300f;
    public TextMeshProUGUI timerText;

    [Header("Wrong Answer Penalty")]
    public Color normalColor = Color.white;
    public Color penaltyColor = Color.red;
    public float penaltyFlashDuration = 1.2f;

    private float time;
    private bool ended = false;
    private Vector2 originalAnchoredPosition;

    private float penaltyFlashTimer = 0f;

    void Start()
    {
        time = startTime;

        if (timerText != null)
        {
            originalAnchoredPosition = timerText.rectTransform.anchoredPosition;
            normalColor = timerText.color;
        }

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
            UpdateTimerUI();

            if (timerText != null)
                timerText.color = Color.red;

            return;
        }

        UpdateTimerUI();

        if (penaltyFlashTimer > 0)
        {
            penaltyFlashTimer -= Time.deltaTime;

            if (timerText != null)
                timerText.color = penaltyColor;

            return;
        }

        DangerEffect();
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void DangerEffect()
    {
        if (timerText == null) return;

        timerText.rectTransform.anchoredPosition = originalAnchoredPosition;

        if (time <= 60)
            timerText.color = Color.red;
        else
            timerText.color = normalColor;

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
        if (ended) return;

        time -= amount;

        if (time <= 0)
        {
            time = 0;
            ended = true;
            UpdateTimerUI();

            if (timerText != null)
                timerText.color = Color.red;

            return;
        }

        UpdateTimerUI();

        penaltyFlashTimer = penaltyFlashDuration;

        if (timerText != null)
            timerText.color = penaltyColor;

        Debug.Log("Visible timer reduced by: " + amount);
    }
}