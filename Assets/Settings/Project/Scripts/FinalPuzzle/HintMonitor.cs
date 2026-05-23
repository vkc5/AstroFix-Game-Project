using UnityEngine;

public class HintMonitor : MonoBehaviour
{
    [Header("UI")]
    public GameObject hintPanel;

    [Header("Player Control")]
    public PlayerMovement playerMovementScript;
    public ThirdPersonCamera cameraScript;

    private bool playerNear = false;

    void Start()
    {
        if (hintPanel != null)
            hintPanel.SetActive(false);
    }

    void Update()
    {
        bool panelOpen = hintPanel != null && hintPanel.activeSelf;

        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed near hint monitor");

            if (!panelOpen)
            {
                OpenHint();
            }
        }

        if (panelOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseHint();
        }
    }

    public void OpenHint()
    {
        if (hintPanel == null) return;

        hintPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        if (cameraScript != null)
            cameraScript.enabled = false;
    }

    public void CloseHint()
    {
        if (hintPanel != null)
            hintPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        if (cameraScript != null)
            cameraScript.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            playerNear = true;
            Debug.Log("Hint monitor ready. Press E to read system log.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            playerNear = false;

            if (hintPanel != null && hintPanel.activeSelf)
                CloseHint();
        }
    }
}