using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FuelPuzzleLevel : MonoBehaviour
{
    public FuelSystemManager fuelManager;
    public PlayerMovement playerMovement;
    public AudioSource victorySound;

    public GameObject winPopup;
    public GameObject fuelHUD;

    public string nextSceneName = "Hallway";

    private bool levelCompleted = false;

    void Start()
    {
        if (fuelHUD != null)
            fuelHUD.SetActive(true);
    }

    public void PuzzleSolved()
    {
        if (levelCompleted) return;
        StartCoroutine(LevelCompleteRoutine());
    }

    IEnumerator LevelCompleteRoutine()
    {
        levelCompleted = true;

        if (playerMovement != null)
            playerMovement.canMove = false;

        //--------------------------------
        // HIDE HUD
        //--------------------------------
        if (fuelHUD != null)
            fuelHUD.SetActive(false);

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

        Debug.Log("FUEL LEVEL COMPLETE");

        //--------------------------------
        // WAIT 3 SECONDS
        //--------------------------------
        yield return new WaitForSeconds(3f);

        //--------------------------------
        // RESET INVENTORY + LOAD HALLWAY
        //--------------------------------
        if (FuelInventory.Instance != null)
            FuelInventory.Instance.ResetInventory();

        SceneManager.LoadScene(nextSceneName);
    }
}
