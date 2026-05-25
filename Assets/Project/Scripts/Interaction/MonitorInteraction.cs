using UnityEngine;

public class MonitorInteraction : MonoBehaviour
{
    [Header("Systems")]
    public BatteryPickup batterySystem;
    public MemoryGridPuzzle puzzleManager;

    [Header("UI")]
    public GameObject interactPrompt;

    [Header("Pause During Puzzle")]
    public PlayerMovement playerMovement;
    public ThirdPersonCamera thirdPersonCamera;

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

            if (batterySystem != null && !batterySystem.HasAllBatteries())
            {
                Debug.Log("Need all batteries first!");
                return;
            }

            puzzleOpened = true;

            if (interactPrompt != null)
                interactPrompt.SetActive(false);

            if (playerMovement != null)
                playerMovement.enabled = false;

            if (thirdPersonCamera != null)
                thirdPersonCamera.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (puzzleManager != null && puzzleManager.puzzlePanel != null)
            {
                puzzleManager.puzzlePanel.SetActive(true);
                puzzleManager.StartPuzzle();
                Debug.Log("Puzzle opened by E");
            }
            else
            {
                Debug.LogError("Puzzle Manager or Puzzle Panel is not assigned!");
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

            if (interactPrompt != null && !puzzleOpened)
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