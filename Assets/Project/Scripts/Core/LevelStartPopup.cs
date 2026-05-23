using UnityEngine;

public class LevelStartPopup : MonoBehaviour
{
    public GameObject startPopup;
    public MonoBehaviour playerMovement;
    public MonsterPatrolChase monster;

    private bool levelStarted = false;

    void Start()
    {
        startPopup.SetActive(true);
        PauseSceneLoader.Instance.canPause = false;
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (monster != null)
            monster.enabled = false;

        Time.timeScale = 0f;
    }

    void Update()
    {
        if (!levelStarted && Input.GetKeyDown(KeyCode.E))
        {
            StartLevel();
        }
    }

    void StartLevel()
    {
        levelStarted = true;

        startPopup.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (monster != null)
            monster.enabled = true;
        
        PauseSceneLoader.Instance.canPause = true;

        Time.timeScale = 1f;
    }
}