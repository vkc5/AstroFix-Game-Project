using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance;

    public string hallwaySceneName = "Hallway";

    public int unlockedStep = 0;
    public int latestCompletedStep = -1;
    public bool gameCompleted = false;

    private const string SaveExistsKey = "SaveExists";
    private const string UnlockedStepKey = "UnlockedStep";
    private const string LatestCompletedStepKey = "LatestCompletedStep";
    private const string GameCompletedKey = "GameCompleted";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadProgress();
    }

    public bool HasSave()
    {
        return PlayerPrefs.GetInt(SaveExistsKey, 0) == 1;
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();

        unlockedStep = 0;          // prelevel first
        latestCompletedStep = -1;  // nothing completed
        gameCompleted = false;

        SaveProgress();
        SceneManager.LoadScene(hallwaySceneName);
    }

    public void ContinueGame()
    {
        LoadProgress();

        if (!HasSave() || gameCompleted)
            return;

        SceneManager.LoadScene(hallwaySceneName);
    }

    public bool CanPlayStep(int stepNumber)
    {
        if (gameCompleted)
            return false;

        if (IsStepCompleted(stepNumber))
            return false;

        return stepNumber == unlockedStep;
    }

    public bool IsStepCompleted(int stepNumber)
    {
        return stepNumber <= latestCompletedStep;
    }

    public void CompleteStep(int stepNumber)
    {
        if (stepNumber > latestCompletedStep)
            latestCompletedStep = stepNumber;

        unlockedStep = latestCompletedStep + 1;

        if (latestCompletedStep >= 5)
            gameCompleted = true;

        SaveProgress();

        // ONLY return to hallway if NOT hallway mission
        if (stepNumber != 0)
        {
            SceneManager.LoadScene(hallwaySceneName);
        }
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt(SaveExistsKey, 1);
        PlayerPrefs.SetInt(UnlockedStepKey, unlockedStep);
        PlayerPrefs.SetInt(LatestCompletedStepKey, latestCompletedStep);
        PlayerPrefs.SetInt(GameCompletedKey, gameCompleted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        unlockedStep = PlayerPrefs.GetInt(UnlockedStepKey, 0);
        latestCompletedStep = PlayerPrefs.GetInt(LatestCompletedStepKey, -1);
        gameCompleted = PlayerPrefs.GetInt(GameCompletedKey, 0) == 1;
    }
}