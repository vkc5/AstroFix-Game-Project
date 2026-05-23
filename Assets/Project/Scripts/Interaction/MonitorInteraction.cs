using UnityEngine;

public class MonitorInteraction : MonoBehaviour
{
    [Header("Systems")]
    public BatteryPickup batterySystem;
    public MemoryGridPuzzle puzzleManager;

    [Header("UI")]
    public GameObject interactPrompt;

    private bool playerNear = false;
    private bool puzzleOpened = false;

    void Start()
    {
        if (puzzleManager != null && puzzleManager.puzzlePanel != null)
            puzzleManager.puzzlePanel.SetActive(false);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (puzzleOpened)
                return;

            // Temporary true for testing
            if (true)
            // if (batterySystem != null && batterySystem.HasAllBatteries())
            {
                puzzleOpened = true;

                if (interactPrompt != null)
                    interactPrompt.SetActive(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (puzzleManager != null)
                {
                    puzzleManager.StartPuzzle();
                    Debug.Log("Puzzle opened and StartPuzzle called!");
                }
                else
                {
                    Debug.LogError("Puzzle Manager is not assigned in MonitorInteraction!");
                }
            }
            else
            {
                Debug.Log("Place all batteries first!");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;

            if (batterySystem != null)
                batterySystem.nearMonitor = true;

            if (interactPrompt != null)
                interactPrompt.SetActive(true);

            Debug.Log("Player near monitor");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;

            if (batterySystem != null)
                batterySystem.nearMonitor = false;

            if (interactPrompt != null)
                interactPrompt.SetActive(false);

            Debug.Log("Player left monitor");
        }
    }
}