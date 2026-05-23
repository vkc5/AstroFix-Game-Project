using UnityEngine;

public class KillPlayerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (LevelStateManager.LevelCompleted) return;

        if (other.CompareTag("Player"))
        {
            FindFirstObjectByType<GameOverManager>().GameOver();
        }
    }
}