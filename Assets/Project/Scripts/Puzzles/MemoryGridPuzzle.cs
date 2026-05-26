using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryGridPuzzle : MonoBehaviour
{
    [Header("Puzzle UI")]
    public GameObject puzzlePanel;
    public GameObject winPanel;

    [Header("Text")]
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI timerText;

    [Header("Buttons")]
    public Button[] cells;
    public Button submitButton;

    [Header("Door")]
    public Animator[] doorAnimators;
    public string openTriggerName = "open";
    public string closeTriggerName = "close";

    [Header("Settings")]
    public float showDuration = 5f;
    public int darkCells = 5;

    [Header("Colors")]
    public Color hiddenColor = new Color(0.2f, 0.2f, 0.2f);
    public Color shownColor = new Color(0.8f, 0.1f, 0.1f);
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    private bool[] pattern;
    private bool[] playerAnswer;
    private bool playerCanPress = false;
    private Coroutine puzzleRoutine;

    void Start()
    {
        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        if (submitButton != null)
        {
            submitButton.onClick.RemoveAllListeners();
            submitButton.onClick.AddListener(CheckAnswer);
        }

        Debug.Log("MemoryGridPuzzle is ready.");
        Debug.Log("Cells Count: " + (cells != null ? cells.Length : 0));
        Debug.Log("Message Text: " + (messageText != null ? "OK" : "MISSING"));
        Debug.Log("Timer Text: " + (timerText != null ? "OK" : "MISSING"));
        Debug.Log("Submit Button: " + (submitButton != null ? "OK" : "MISSING"));
        Debug.Log("Door Animators Count: " + (doorAnimators != null ? doorAnimators.Length : 0));
    }

    public void StartPuzzle()
    {
        if (!CheckReferences())
            return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        puzzlePanel.SetActive(true);

        if (winPanel != null)
            winPanel.SetActive(false);

        pattern = new bool[cells.Length];
        playerAnswer = new bool[cells.Length];
        playerCanPress = false;

        for (int i = 0; i < cells.Length; i++)
        {
            int index = i;

            cells[i].onClick.RemoveAllListeners();
            cells[i].onClick.AddListener(() => OnCellClick(index));

            SetColor(i, hiddenColor);
        }

        GeneratePattern();

        if (puzzleRoutine != null)
            StopCoroutine(puzzleRoutine);

        puzzleRoutine = StartCoroutine(ShowThenHide());
    }

    bool CheckReferences()
    {
        bool ok = true;

        if (puzzlePanel == null)
        {
            Debug.LogError("Puzzle Panel is missing.");
            ok = false;
        }

        if (messageText == null)
        {
            Debug.LogError("Message Text is missing.");
            ok = false;
        }

        if (timerText == null)
        {
            Debug.LogError("Timer Text is missing.");
            ok = false;
        }

        if (submitButton == null)
        {
            Debug.LogError("Submit Button is missing.");
            ok = false;
        }

        if (cells == null || cells.Length == 0)
        {
            Debug.LogError("Cells are missing.");
            ok = false;
        }

        return ok;
    }

    void GeneratePattern()
    {
        for (int i = 0; i < pattern.Length; i++)
            pattern[i] = false;

        int count = 0;

        while (count < darkCells)
        {
            int randomIndex = Random.Range(0, cells.Length);

            if (!pattern[randomIndex])
            {
                pattern[randomIndex] = true;
                count++;
            }
        }
    }

    IEnumerator ShowThenHide()
    {
        playerCanPress = false;

        messageText.text = "Remember the pattern!";
        timerText.text = "";

        for (int i = 0; i < cells.Length; i++)
        {
            if (pattern[i])
                SetColor(i, shownColor);
            else
                SetColor(i, hiddenColor);
        }

        float timeLeft = showDuration;

        while (timeLeft > 0)
        {
            timerText.text = "Hiding in: " + Mathf.CeilToInt(timeLeft) + "s";

            yield return new WaitForSeconds(0.1f);
            timeLeft -= 0.1f;
        }

        for (int i = 0; i < cells.Length; i++)
            SetColor(i, hiddenColor);

        messageText.text = "Now click the dark cells!";
        timerText.text = "";

        playerCanPress = true;
    }

    void OnCellClick(int index)
    {
        if (!playerCanPress)
            return;

        playerAnswer[index] = !playerAnswer[index];

        if (playerAnswer[index])
            SetColor(index, shownColor);
        else
            SetColor(index, hiddenColor);
    }

    public void CheckAnswer()
    {
        if (!playerCanPress)
            return;

        StartCoroutine(EvaluateAnswer());
    }

    IEnumerator EvaluateAnswer()
    {
        playerCanPress = false;

        bool allCorrect = true;

        for (int i = 0; i < cells.Length; i++)
        {
            if (playerAnswer[i] == pattern[i])
            {
                if (pattern[i])
                    SetColor(i, correctColor);
                else
                    SetColor(i, hiddenColor);
            }
            else
            {
                SetColor(i, wrongColor);
                allCorrect = false;
            }
        }

        yield return new WaitForSeconds(1.5f);

        if (allCorrect)
        {
            messageText.text = "Perfect!";
            timerText.text = "";

            yield return new WaitForSeconds(0.5f);

            puzzlePanel.SetActive(false);

            if (winPanel != null)
                winPanel.SetActive(true);

            OpenDoor();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            messageText.text = "Wrong! Watch again...";
            timerText.text = "";

            yield return new WaitForSeconds(1f);

            for (int i = 0; i < playerAnswer.Length; i++)
                playerAnswer[i] = false;

            puzzleRoutine = StartCoroutine(ShowThenHide());
        }
    }

    void OpenDoor()
    {
        if (doorAnimators == null || doorAnimators.Length == 0)
        {
            Debug.LogError("No door animators assigned!");
            return;
        }

        foreach (Animator anim in doorAnimators)
        {
            if (anim != null)
            {
                anim.ResetTrigger(closeTriggerName);
                anim.SetTrigger(openTriggerName);

                Debug.Log("Door open trigger sent to: " + anim.gameObject.name);
            }
        }
    }

    void SetColor(int index, Color color)
    {
        if (cells[index] == null)
            return;

        Image image = cells[index].GetComponent<Image>();

        if (image != null)
            image.color = color;
    }
}