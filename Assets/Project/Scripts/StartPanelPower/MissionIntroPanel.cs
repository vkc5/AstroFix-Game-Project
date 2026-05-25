using UnityEngine;

public class MissionIntroPanel : MonoBehaviour
{
    public GameObject missionPanel;
    public GameObject batteryText;

    public PlayerMovement playerMovement;

    private bool closed = false;

    void Start()
    {
        if (missionPanel != null)
            missionPanel.SetActive(true);

        if (batteryText != null)
            batteryText.SetActive(false);

        // STOP PLAYER MOVEMENT
        if (playerMovement != null)
            playerMovement.canMove = false;
    }

    void Update()
    {
        if (!closed && Input.GetKeyDown(KeyCode.E))
        {
            closed = true;

            if (missionPanel != null)
                missionPanel.SetActive(false);

            if (batteryText != null)
                batteryText.SetActive(true);

            // ENABLE PLAYER MOVEMENT
            if (playerMovement != null)
                playerMovement.canMove = true;
        }
    }
}