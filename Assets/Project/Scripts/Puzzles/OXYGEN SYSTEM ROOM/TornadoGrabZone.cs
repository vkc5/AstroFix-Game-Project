using UnityEngine;

public class TornadoGrabZone : MonoBehaviour
{
    public Transform pullPoint;

    public float pullDistance = 22f;
    public float killDistance = 2f;

    public float pullForce = 5.9f;

    public Rigidbody playerRb;
    public Transform player;

    public bool canPull = true;

    void FixedUpdate()
    {
        if (!canPull) return;

        float distance =
            Vector3.Distance(player.position, pullPoint.position);

        if (distance <= pullDistance)
        {
            Vector3 pullDirection =
                (pullPoint.position - player.position).normalized;

            // Add pulling force
            playerRb.AddForce(
                pullDirection * pullForce,
                ForceMode.Force
            );
        }

        if (distance <= killDistance)
        {
            canPull = false;

            FindFirstObjectByType<GameOverManager>().GameOver();
        }
    }
}