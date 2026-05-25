using System.Collections;
using System.Collections.Generic;
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
    public Text titleLabel;
    public TextMeshProUGUI timerText;

    [Header("Buttons")]
    public Button[] cells;
    public Button submitButton;

    [Header("Door")]
    public Animator[] doorAnimators;
    public string openTriggerName = "open";
    public string closeTriggerName = "close";

    [Header("Player Control")]
    public PlayerMovement playerMovement;
    public ThirdPersonCamera thirdPersonCamera;

    [Header("Levels")]
    public int totalLevels = 3;
    public int currentLevel = 1;

    // Harder difficulty for 6x4 grid
    public int[] darkCellsPerLevel = { 6, 9, 12 };

    [Header("Settings")]
    public float[] showDurationsPerLevel = { 5f, 4f, 3f };

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

        currentLevel = 1;

        StartLevel();
    }

    void StartLevel()
    {
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
        return puzzlePanel != null &&
               timerText != null &&
               submitButton != null &&
               cells != null &&
               cells.Length > 0;
    }

    void SetMessage(string text)
    {
        if (messageText != null)
            messageText.text = text;

        if (titleLabel != null)
            titleLabel.text = text;
    }

    int GetDarkCellsForCurrentLevel()
    {
        int index = currentLevel - 1;

        if (darkCellsPerLevel != null &&
            index >= 0 &&
            index < darkCellsPerLevel.Length)
        {
            return darkCellsPerLevel[index];
        }

        return 6;
    }

    float GetShowDurationForCurrentLevel()
    {
        int index = currentLevel - 1;

        if (showDurationsPerLevel != null &&
            index >= 0 &&
            index < showDurationsPerLevel.Length)
        {
            return showDurationsPerLevel[index];
        }

        return 5f;
    }

    void GeneratePattern()
    {
        int darkCells = GetDarkCellsForCurrentLevel();

        if (darkCells > cells.Length)
            darkCells = cells.Length;

        // Reset all cells
        for (int i = 0; i < pattern.Length; i++)
            pattern[i] = false;

        // Better randomization
        List<int> available = new List<int>();

        for (int i = 0; i < cells.Length; i++)
            available.Add(i);

        for (int i = 0; i < darkCells; i++)
        {
            int randomListIndex = Random.Range(0, available.Count);

            int chosenCell = available[randomListIndex];

            pattern[chosenCell] = true;

            available.RemoveAt(randomListIndex);
        }
    }

    IEnumerator ShowThenHide()
    {
        playerCanPress = false;

        SetMessage("Level " + currentLevel + " / " + totalLevels + " - Remember!");
        timerText.text = "";

        // Show pattern
        for (int i = 0; i < cells.Length; i++)
        {
            SetColor(i, pattern[i] ? shownColor : hiddenColor);
        }

        float timeLeft = GetShowDurationForCurrentLevel();

        while (timeLeft > 0)
        {
            timerText.text = Mathf.CeilToInt(timeLeft) + "s";

            yield return new WaitForSeconds(0.1f);

            timeLeft -= 0.1f;
        }

        // Hide pattern
        for (int i = 0; i < cells.Length; i++)
        {
            SetColor(i, hiddenColor);
        }

        SetMessage("Level " + currentLevel + " / " + totalLevels + " - Click the cells!");
        timerText.text = "";

        playerCanPress = true;
    }

    void OnCellClick(int index)
    {
        if (!playerCanPress)
            return;

        playerAnswer[index] = !playerAnswer[index];

        SetColor(index,
            playerAnswer[index] ? shownColor : hiddenColor);
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
                SetColor(i,
                    pattern[i] ? correctColor : hiddenColor);
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
            if (currentLevel < totalLevels)
            {
                SetMessage("Level " + currentLevel + " complete!");
                timerText.text = "";

                yield return new WaitForSeconds(1f);

                currentLevel++;

                StartLevel();
            }
            else
            {
                SetMessage("All levels complete!");
                timerText.text = "";

                yield return new WaitForSeconds(0.5f);

                puzzlePanel.SetActive(false);

                if (winPanel != null)
                    winPanel.SetActive(true);

                OpenDoor();

                if (playerMovement != null)
                    playerMovement.enabled = true;

                if (thirdPersonCamera != null)
                    thirdPersonCamera.enabled = true;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            SetMessage("Wrong! Restarting Level " + currentLevel);
            timerText.text = "";

            yield return new WaitForSeconds(1f);

            StartLevel();
        }
    }

    void OpenDoor()
    {
        foreach (Animator anim in doorAnimators)
        {
            if (anim != null)
            {
                anim.ResetTrigger(closeTriggerName);
                anim.SetTrigger(openTriggerName);
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