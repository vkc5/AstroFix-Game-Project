using UnityEngine;

public class PauseMenuButtons : MonoBehaviour
{
    public void Resume()
    {
        PauseSceneLoader loader = PauseSceneLoader.Instance;

        if (loader != null)
        {
            loader.ResumeGame();
        }
        else
        {
            Debug.LogError("PauseSceneLoader not found!");
        }
    }

    public void Retry()
    {
        PauseSceneLoader loader = PauseSceneLoader.Instance;

        if (loader != null)
        {
            loader.RetryLevel();
        }
        else
        {
            Debug.LogError("PauseSceneLoader not found!");
        }
    }
}