using UnityEngine;
using TMPro;

public class HallwayKeyCounter : MonoBehaviour
{
    public GameObject keyHUD;
    public TextMeshProUGUI keyCountText;

    public int totalKeys = 5;

    void Start()
    {
        UpdateKeyCounter();
    }

    public void UpdateKeyCounter()
    {
        int completedStep = GameProgressManager.Instance.latestCompletedStep;

        // Prelevel not completed yet
        if (completedStep < 0)
        {
            if (keyHUD != null)
                keyHUD.SetActive(false);

            return;
        }

        // After prelevel completed, show HUD
        if (keyHUD != null)
            keyHUD.SetActive(true);

        // Keys only from completed normal levels
        // Prelevel = 0, Level 1 = 1, Level 2 = 2, etc.
        int keyCount = Mathf.Clamp(completedStep, 0, totalKeys);

        if (keyCountText != null)
            keyCountText.text = keyCount.ToString();
    }
}