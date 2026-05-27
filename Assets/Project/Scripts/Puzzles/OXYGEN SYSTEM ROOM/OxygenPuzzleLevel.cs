using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OxygenPuzzleLevel : MonoBehaviour
{
    public static bool IsPuzzlePanelOpen = false;

    public CircuitPuzzleManager puzzleManager;

    public Transform player;
    public PlayerMovement playerMovement;

    public Transform firstComputer;
    public Transform secondComputer;

    public GameObject firstHUD;
    public GameObject secondHUD;

    public AudioSource victorySound;

    public float interactDistance = 2f;
    public string nextSceneName = "Hallway";

    private int currentPuzzleNumber = 1;
    private bool puzzleOpen = false;
    private bool levelCompleted = false;

    public GameObject winPopup;
    void Start()
    {
        IsPuzzlePanelOpen = false;

        if (firstHUD != null) firstHUD.SetActive(true);
        if (secondHUD != null) secondHUD.SetActive(false);

        if (puzzleManager != null)
            puzzleManager.gameObject.SetActive(true);
    }

    void Update()
    {
        if (puzzleOpen || levelCompleted) return;

        Transform target = currentPuzzleNumber == 1 ? firstComputer : secondComputer;

        if (target == null || player == null) return;

        float distance = Vector3.Distance(player.position, target.position);

        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            OpenPuzzle();
        }
    }

    void OpenPuzzle()
    {
        puzzleOpen = true;

        if (PauseSceneLoader.Instance != null)
            PauseSceneLoader.Instance.SetCanPause(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (playerMovement != null)
            playerMovement.canMove = false;

        puzzleManager.OpenPuzzle(7, currentPuzzleNumber);
    }

    public void PuzzleClosedWithoutSolve()
    {
        puzzleOpen = false;
        IsPuzzlePanelOpen = false;

        if (PauseSceneLoader.Instance != null)
            PauseSceneLoader.Instance.SetCanPause(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (!levelCompleted && playerMovement != null)
            playerMovement.canMove = true;
    }

    public void PuzzleSolved()
    {
        puzzleOpen = false;
        IsPuzzlePanelOpen = false;

        if (currentPuzzleNumber == 1)
        {
            if (PauseSceneLoader.Instance != null)
                PauseSceneLoader.Instance.SetCanPause(true);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            currentPuzzleNumber = 2;

            if (playerMovement != null)
                playerMovement.canMove = true;

            if (firstHUD != null) firstHUD.SetActive(false);
            if (secondHUD != null) secondHUD.SetActive(true);
        }
        else
        {
            StartCoroutine(LevelCompleteRoutine());
        }
    }

    IEnumerator LevelCompleteRoutine()
    {
        levelCompleted = true;

        if (playerMovement != null)
            playerMovement.canMove = false;

        //--------------------------------
        // HIDE HUD
        //--------------------------------
        if (secondHUD != null)
            secondHUD.SetActive(false);

        //--------------------------------
        // PLAY SOUND
        //--------------------------------
        if (victorySound != null)
            victorySound.Play();

        //--------------------------------
        // SHOW WIN POPUP
        //--------------------------------
        if (winPopup != null)
            winPopup.SetActive(true);

        Debug.Log("LEVEL COMPLETE");

        //--------------------------------
        // WAIT 3 SECONDS
        //--------------------------------
        yield return new WaitForSeconds(3f);

        //--------------------------------
        // SAVE COMPLETION
        //--------------------------------
        GameProgressManager.Instance.CompleteStep(3);

        //--------------------------------
        // LOAD HALLWAY
        //--------------------------------
        //SceneManager.LoadScene(nextSceneName);
    }
}