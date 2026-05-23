using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MonsterPatrolChase : MonoBehaviour
{
    public Transform player;
    public Transform[] patrolPoints;

    public float detectDistance = 5f;
    public float stopDistance = 7f;
    public float killDistance = 1f;

    public AudioSource farSound;
    public AudioSource detectSound;
    public AudioSource stepSound;

    private NavMeshAgent agent;
    private Animator animator;
    private int currentPoint = 0;
    private bool chasing = false;
    private bool playerKilled = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        PlayIdle();

        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[currentPoint].position);

        if (farSound != null)
            farSound.Play();

        if (stepSound != null)
            stepSound.loop = true;
    }

    void Update()
    {
        if (LevelStateManager.LevelCompleted || playerKilled)
        {
            StopMonster();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (!chasing)
        {
            Patrol();
            PlayWalk();

            if (distance <= detectDistance)
            {
                chasing = true;

                if (detectSound != null)
                    detectSound.Play();

                if (stepSound != null && !stepSound.isPlaying)
                    stepSound.Play();

                PlayRun();
            }
        }
        else
        {
            agent.SetDestination(player.position);
            PlayRun();

            if (distance >= stopDistance)
            {
                chasing = false;

                if (stepSound != null)
                    stepSound.Stop();

                if (patrolPoints.Length > 0)
                    agent.SetDestination(patrolPoints[currentPoint].position);

                PlayWalk();
            }
        }

        if (distance <= killDistance)
        {
            StartCoroutine(KillPlayerDelay());
        }
    }

    IEnumerator KillPlayerDelay()
    {
        playerKilled = true;

        StopMonster();

        if (agent != null)
            agent.enabled = false;

        PlayAttack();

        // stop player movement
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.enabled = false;

        // wait for attack animation
        yield return new WaitForSeconds(0.7f);

        FindFirstObjectByType<GameOverManager>().GameOver();
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0)
        {
            PlayIdle();
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint++;

            if (currentPoint >= patrolPoints.Length)
                currentPoint = 0;

            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    void StopMonster()
    {
        if (agent != null && agent.enabled)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (stepSound != null)
            stepSound.Stop();
    }

    void PlayIdle()
    {
        if (animator != null)
            animator.Play("idle1");
    }

    void PlayWalk()
    {
        if (animator != null)
            animator.Play("walk2");
    }

    void PlayRun()
    {
        if (animator != null)
            animator.Play("run1");
    }

    void PlayAttack()
    {
        if (animator != null)
            animator.Play("attack1");
    }
}