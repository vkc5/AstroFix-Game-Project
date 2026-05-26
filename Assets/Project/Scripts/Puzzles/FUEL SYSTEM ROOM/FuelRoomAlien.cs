using UnityEngine;
using UnityEngine.AI;

public class FuelRoomAlien : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Distances")]
    public float detectDistance = 1000f;
    public float stopDistance = 2000f;
    public float killDistance = 1.5f;

    [Header("Sounds (optional)")]
    public AudioSource farSound;
    public AudioSource detectSound;
    public AudioSource stepSound;

    private NavMeshAgent agent;
    private bool chasing = false;
    private bool dead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (farSound != null && !farSound.isPlaying)
            farSound.Play();

        if (stepSound != null)
            stepSound.loop = true;
    }

    void Update()
    {
        // Safety checks — if anything's missing, do nothing instead of crashing.
        if (dead) return;
        if (player == null) return;
        if (agent == null) return;
        if (!agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Start chasing once player is within detect range
        if (!chasing && distance <= detectDistance)
        {
            chasing = true;

            if (detectSound != null)
                detectSound.Play();

            if (stepSound != null && !stepSound.isPlaying)
                stepSound.Play();
        }

        // Chase logic
        if (chasing)
        {
            agent.SetDestination(player.position);

            if (distance >= stopDistance)
            {
                chasing = false;
                agent.ResetPath();

                if (stepSound != null)
                    stepSound.Stop();
            }
        }

        // Kill check — find game over manager even if disabled, and only fire once
      if (distance <= killDistance)
        {
            GameOverManager gom = FindFirstObjectByType<GameOverManager>(FindObjectsInactive.Include);
            if (gom != null)
            {
                dead = true;
                gom.GameOver();
            }
        }
    }
}