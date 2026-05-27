using UnityEngine;

public class SparkHazard : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        CheckPlayer(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckPlayer(other);
    }

    private void CheckPlayer(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            triggered = true;

            Debug.Log("Player hit by spark!");

            GameOverManager gameOver = FindFirstObjectByType<GameOverManager>();

            if (gameOver != null)
            {
                gameOver.GameOver();
            }
        }
    }
}