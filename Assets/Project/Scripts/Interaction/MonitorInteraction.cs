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
    private Rigidbody playerRb;

    private bool playerNear = false;
    private bool puzzleOpened = false;

    void Start()
    {
        if (playerMovement != null)
            playerRb = playerMovement.GetComponent<Rigidbody>();

        if (puzzleManager != null && puzzleManager.puzzlePanel != null)
            puzzleManager.puzzlePanel.SetActive(false);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (!playerNear || !Input.GetKeyDown(KeyCode.E))
            return;

        if (puzzleOpened)
            return;

        if (batterySystem == null || !batterySystem.HasAllBatteries())
        {
            Debug.Log("Need all batteries first!");
            return;
        }

        puzzleOpened = true;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        // FULLY STOP PLAYER PHYSICS
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        // STOP RUNNING ANIMATION
        if (playerMovement != null)
        {
            Animator anim = playerMovement.GetComponent<Animator>();

            if (anim != null)
            {
                anim.SetFloat("Speed", 0f);
                anim.Play("Idle", 0, 0f);
            }

            playerMovement.canMove = false;
            playerMovement.enabled = false;
        }

        if (thirdPersonCamera != null)
            thirdPersonCamera.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (puzzleManager != null)
        {
            puzzleManager.StartPuzzle();
            Debug.Log("Puzzle opened by E");
        }
        else
        {
            Debug.LogError("Puzzle Manager is not assigned!");
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
            {
                bool hasAllBatteries = batterySystem != null && batterySystem.HasAllBatteries();

                interactPrompt.SetActive(hasAllBatteries);
            }

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