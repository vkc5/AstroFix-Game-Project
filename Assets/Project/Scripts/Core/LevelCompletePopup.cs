using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletePopup : MonoBehaviour
{
    public GameObject popupCanvas;

    public float popupDelay = 3f;
    public float hallwayDelay = 4f;

    public string hallwaySceneName = "Hallway";

    private bool levelCompleted = false;

    public void ShowLevelComplete()
    {
        if (levelCompleted) return;

        levelCompleted = true;
        StartCoroutine(LevelCompleteRoutine());
    }

    IEnumerator LevelCompleteRoutine()
    {
        yield return new WaitForSeconds(popupDelay);

        if (popupCanvas != null)
            popupCanvas.SetActive(true);

        yield return new WaitForSeconds(hallwayDelay);

        SceneManager.LoadScene(hallwaySceneName);
    }
}