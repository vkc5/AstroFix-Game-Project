using UnityEngine;
using UnityEngine.AI;

public class MonsterChase : MonoBehaviour
{
    public Transform player;
    public float detectDistance = 10f;
    public float stopDistance = 15f;
    public float killDistance = 1.5f;

    public AudioSource farSound;
    public AudioSource detectSound;
    public AudioSource stepSound;

    private NavMeshAgent agent;
    private bool chasing = false;

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
        float distance = Vector3.Distance(transform.position, player.position);

        if (!chasing && distance <= detectDistance)
        {
            chasing = true;

            if (detectSound != null)
                detectSound.Play();

            if (stepSound != null && !stepSound.isPlaying)
                stepSound.Play();
        }

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

        if (distance <= killDistance)
        {
            FindFirstObjectByType<GameOverManager>().GameOver();
        }
    }
}