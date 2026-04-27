using UnityEngine;

public class LaserReceiver : MonoBehaviour
{
    public bool isActivated;

    public void Activate()
    {
        if (isActivated) return;

        isActivated = true;
        Debug.Log("Puzzle target activated");
    }
}