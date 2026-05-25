using UnityEngine;

public class HallwayStartPopup : MonoBehaviour
{
    public GameObject startPopup;

    public MonoBehaviour playerMovement;

    private bool missionStarted = false;

    void Start()
    {
        bool preLevelCompleted =
            GameProgressManager.Instance.IsStepCompleted(0);

        // IF PRELEVEL ALREADY COMPLETED
        if (preLevelCompleted)
        {
            startPopup.SetActive(false);

            if (playerMovement != null)
                playerMovement.enabled = true;

            if (PauseSceneLoader.Instance != null)
                PauseSceneLoader.Instance.canPause = true;

            Time.timeScale = 1f;

            return;
        }

        // SHOW POPUP
        startPopup.SetActive(true);

        // DISABLE PAUSE
        if (PauseSceneLoader.Instance != null)
            PauseSceneLoader.Instance.canPause = false;

        // DISABLE MOVEMENT
        if (playerMovement != null)
            playerMovement.enabled = false;

        // SHOW CURSOR
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // FREEZE GAME
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (!missionStarted && Input.GetKeyDown(KeyCode.E))
        {
            StartMission();
        }
    }

    void StartMission()
    {
        missionStarted = true;

        startPopup.SetActive(false);

        // ENABLE MOVEMENT
        if (playerMovement != null)
            playerMovement.enabled = true;

        // ENABLE PAUSE
        if (PauseSceneLoader.Instance != null)
            PauseSceneLoader.Instance.canPause = true;

        // HIDE CURSOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // RESUME GAME
        Time.timeScale = 1f;
    }
}