using UnityEngine;

public class LaserReceiver : MonoBehaviour
{
    public bool isActivated;
    public LevelLightController levelLightController;

    public void Activate()
    {
        if (isActivated) return;

        isActivated = true;

        if (levelLightController != null)
            levelLightController.TurnOnLevel();
    }
}