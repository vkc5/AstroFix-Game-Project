using UnityEngine;
using UnityEngine.AI;

public class LevelStateManager : MonoBehaviour
{
    public static bool LevelCompleted = false;

    [Header("Player")]
    public PlayerMovement playerMovement;

    [Header("Monster")]
    public NavMeshAgent monsterAgent;
    public MonoBehaviour monsterScript;
    public Animator monsterAnimator;

    public void CompleteLevel()
    {
        LevelCompleted = true;

        if (playerMovement != null)
            playerMovement.canMove = false;

        if (monsterAgent != null)
        {
            monsterAgent.isStopped = true;
            monsterAgent.ResetPath();
        }

        if (monsterScript != null)
            monsterScript.enabled = false;

        if (monsterAnimator != null)
        {
            monsterAnimator.SetFloat("Speed", 0);
            monsterAnimator.Play("idle1");
        }
    }
}