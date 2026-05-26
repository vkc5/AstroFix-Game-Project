using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class EndingTextFade : MonoBehaviour
{
    [Header("Text Reference")]
    public TextMeshProUGUI missionText;

    [Header("End Popup")]
    public GameObject endPopup;
    public string mainMenuSceneName = "Start Menu";

    [Header("Ending Lines")]
    [TextArea(2, 5)]
    public string[] lines =
    {
        "MISSION COMPLETE",
        "Earth route restored.",
        "Navigation systems online.",
        "AstroFix core stabilized.",
        "Returning home..."
    };

    [Header("Timing")]
    public float startDelay = 1f;
    public float fadeDuration = 0.8f;
    public float lineStayDuration = 1.4f;
    public float finalStayDuration = 2f;

    private void Start()
    {
        if (endPopup != null)
            endPopup.SetActive(false);

        if (missionText == null)
        {
            Debug.LogError("Mission Text is not assigned.");
            return;
        }

        StartCoroutine(PlayEndingText());
    }

    private IEnumerator PlayEndingText()
    {
        missionText.text = "";
        SetTextAlpha(0f);

        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i < lines.Length; i++)
        {
            missionText.text = BuildTextUpToLine(i);
            yield return StartCoroutine(FadeText(0f, 1f));

            if (i == lines.Length - 1)
                yield return new WaitForSeconds(finalStayDuration);
            else
                yield return new WaitForSeconds(lineStayDuration);
        }

        yield return StartCoroutine(FadeText(1f, 0f));

        GameProgressManager.Instance.CompleteStep(5);

        if (endPopup != null)
            endPopup.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private string BuildTextUpToLine(int currentLine)
    {
        string result = "";

        for (int i = 0; i <= currentLine; i++)
        {
            result += lines[i];

            if (i == 0)
                result += "\n\n";
            else
                result += "\n";
        }

        return result;
    }

    private IEnumerator FadeText(float from, float to)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            SetTextAlpha(Mathf.Lerp(from, to, t));
            yield return null;
        }

        SetTextAlpha(to);
    }

    private void SetTextAlpha(float alpha)
    {
        Color c = missionText.color;
        c.a = alpha;
        missionText.color = c;
    }
    public void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
		        Application.Quit();
        #endif
    }
}