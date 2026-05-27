using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public Button continueButton;

    private void Start()
    {
        if (continueButton != null)
            continueButton.interactable = GameProgressManager.Instance.HasSave() &&
                                          !GameProgressManager.Instance.gameCompleted;
    }

    public void NewGame()
    {
        GameProgressManager.Instance.NewGame();
    }

    public void ContinueGame()
    {
        GameProgressManager.Instance.ContinueGame();
    }
}