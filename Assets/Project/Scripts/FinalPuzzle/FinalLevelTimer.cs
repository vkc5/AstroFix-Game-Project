using UnityEngine;
using TMPro;
using System.Collections;

public class FinalLevelTimer : MonoBehaviour
{
    public float timeRemaining = 300f; // 5:00
    public TextMeshProUGUI timerText;
    public GameObject failPanel;

    [Header("Penalty Flash")]
    public Color normalColor = Color.white;
    public Color penaltyColor = Color.red;
    public float flashDuration = 0.6f;

    private bool timerStarted = false;
    private bool timerEnded = false;
    private Coroutine flashCoroutine;

    void Start()
    {
        if (timerText != null)
            normalColor = timerText.color;

        UpdateTimerUI();

        if (failPanel != null)
            failPanel.SetActive(false);
    }

    void Update()
    {
        if (!timerStarted || timerEnded) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerEnded = true;
            UpdateTimerUI();
            TimeFailed();
            return;
        }

        UpdateTimerUI();
    }

    public void StartTimer()
    {
        timerStarted = true;
        UpdateTimerUI();
    }

    public void RemoveTime(float amount)
    {
        if (timerEnded) return;

        timeRemaining -= amount;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerEnded = true;
            UpdateTimerUI();
            TimeFailed();
            return;
        }

        UpdateTimerUI();

        if (timerText != null)
        {
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashTimerRed());
        }

        Debug.Log("Timer reduced by: " + amount);
    }

    private IEnumerator FlashTimerRed()
    {
        timerText.color = penaltyColor;
        yield return new WaitForSeconds(flashDuration);
        timerText.color = normalColor;
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    void TimeFailed()
    {
        Debug.Log("Time is over! Level failed.");

        if (failPanel != null)
            failPanel.SetActive(true);
    }
}