using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    public int stepNumber;
    public string levelSceneName;

    public Transform player;
    public GameObject objectivePopup;
    public GameObject interactHUD;

    public float interactDistance = 2f;

    void Start()
    {
        RefreshState();
    }

    void Update()
    {
        RefreshState();

        if (!GameProgressManager.Instance.CanPlayStep(stepNumber))
            return;

        if (player == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(levelSceneName);
        }
    }

    void RefreshState()
    {
        bool canPlay = GameProgressManager.Instance.CanPlayStep(stepNumber);

        if (objectivePopup != null)
            objectivePopup.SetActive(canPlay);

        if (interactHUD != null && player != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            interactHUD.SetActive(canPlay && distance <= interactDistance);
        }
    }
}