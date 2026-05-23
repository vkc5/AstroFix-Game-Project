using UnityEngine;
using TMPro;
using System.Collections;

public class ConsolePuzzle : MonoBehaviour
{
    [Header("UI")]
    public GameObject puzzlePanel;
    public TMP_InputField answerInput;
    public TMP_Text feedbackText;

    [Header("Puzzle")]
    public string correctWord = "REACT";

    [Header("Player")]
    public PlayerKeyInventory inventory;
    public PlayerMovement playerMovementScript;
    public ThirdPersonCamera cameraScript;

    [Header("Portal")]
    public GameObject portalObject;

    [Header("Inserted Key Visual")]
    public GameObject carriedKeyObject;
    private bool keyInserted = false;

    [Header("Timer Penalty")]
    public FinalLevelTimer timer;
    public float wrongPenalty = 10f;

    private bool playerNear = false;
    private bool puzzleSolved = false;

    private void Start()
    {
        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        if (feedbackText != null)
        {
            feedbackText.text = "WAITING FOR INPUT...";
            feedbackText.color = Color.cyan;
        }

        if (portalObject != null)
            portalObject.SetActive(false);

        // AUTO FIND TIMER if not assigned in Inspector
        if (timer == null)
        {
            timer = FindFirstObjectByType<FinalLevelTimer>();

            if (timer != null)
                Debug.Log("ConsolePuzzle found FinalLevelTimer automatically.");
            else
                Debug.LogError("NO FinalLevelTimer found in scene.");
        }
    }

    private void Update()
    {
        bool panelIsOpen = puzzlePanel != null && puzzlePanel.activeSelf;

        if (playerNear && !panelIsOpen && Input.GetKeyDown(KeyCode.E) && !puzzleSolved)
        {
            OpenPanel();
        }

        if (panelIsOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        if (inventory == null)
        {
            Debug.LogWarning("Player inventory not found.");
            return;
        }

        if (!inventory.HasAllKeys())
        {
            Debug.Log("You need all 5 keys first.");
            return;
        }

        if (puzzlePanel == null || answerInput == null || feedbackText == null)
        {
            Debug.LogWarning("Puzzle UI references are missing.");
            return;
        }

        puzzlePanel.SetActive(true);

        if (!keyInserted && carriedKeyObject != null)
        {
            carriedKeyObject.SetActive(false);
            keyInserted = true;
            Debug.Log("Final key inserted into console.");
        }

        answerInput.text = "";
        feedbackText.text = "WAITING FOR INPUT...";
        feedbackText.color = Color.cyan;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        if (cameraScript != null)
            cameraScript.enabled = false;

        answerInput.interactable = true;
        answerInput.Select();
        answerInput.ActivateInputField();
    }

    public void ClosePanel()
    {
        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        if (cameraScript != null)
            cameraScript.enabled = true;
    }

    public void CheckAnswer()
    {
        if (answerInput == null || feedbackText == null)
            return;

        string playerAnswer = answerInput.text.Trim().ToUpper();

        Debug.Log("Submitted answer: " + playerAnswer);

        if (playerAnswer == correctWord.ToUpper())
        {
            puzzleSolved = true;

            feedbackText.text = "ACCESS GRANTED. PORTAL OPENING...";
            feedbackText.color = Color.green;

            if (portalObject != null)
                portalObject.SetActive(true);
            else
                Debug.LogWarning("Portal Object is not assigned.");

            StartCoroutine(ClosePanelAfterDelay());
        }
        else
        {
            feedbackText.text = "ACCESS DENIED. TRY AGAIN.";
            feedbackText.color = Color.cyan;

            answerInput.text = "";
            answerInput.Select();
            answerInput.ActivateInputField();

            // Try again if timer somehow became null
            if (timer == null)
                timer = FindFirstObjectByType<FinalLevelTimer>();

            if (timer != null)
            {
                timer.RemoveTime(wrongPenalty);
                Debug.Log("Wrong answer. Timer reduced by " + wrongPenalty);
            }
            else
            {
                Debug.LogError("Timer still NOT found. Check if FinalLevelTimer object is active in scene.");
            }
        }
    }

    private IEnumerator ClosePanelAfterDelay()
    {
        yield return new WaitForSeconds(1.2f);
        ClosePanel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            playerNear = true;

            inventory = other.GetComponent<PlayerKeyInventory>();

            if (inventory == null)
                inventory = other.transform.root.GetComponent<PlayerKeyInventory>();

            Debug.Log("Console ready. Press E to access terminal.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            playerNear = false;
            inventory = null;

            if (puzzlePanel != null && puzzlePanel.activeSelf)
                ClosePanel();
        }
    }
}