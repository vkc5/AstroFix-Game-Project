using System.Collections;
using UnityEngine;

public class HallwaySpawnManager : MonoBehaviour
{
    public Transform player;

    public Transform defaultSpawn;
    public Transform afterPrelevelSpawn;
    public Transform afterLevel1Spawn;
    public Transform afterLevel2Spawn;
    public Transform afterLevel3Spawn;
    public Transform afterLevel4Spawn;
    public Transform afterLevel5Spawn;

    IEnumerator Start()
    {
        yield return null;
        yield return null;

        GameProgressManager.Instance.LoadProgress();

        int completed = GameProgressManager.Instance.latestCompletedStep;

        Transform spawn = defaultSpawn;

        if (completed == 0) spawn = afterPrelevelSpawn;
        else if (completed == 1) spawn = afterLevel1Spawn;
        else if (completed == 2) spawn = afterLevel2Spawn;
        else if (completed == 3) spawn = afterLevel3Spawn;
        else if (completed == 4) spawn = afterLevel4Spawn;
        else if (completed >= 5) spawn = afterLevel5Spawn;

        if (player == null || spawn == null)
        {
            Debug.LogError("Player or spawn point missing!");
            yield break;
        }

        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (movement != null)
            movement.canMove = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        player.position = spawn.position;
        player.rotation = spawn.rotation;

        if (rb != null)
        {
            rb.position = spawn.position;
            rb.rotation = spawn.rotation;
            rb.isKinematic = false;
        }

        yield return null;

        if (movement != null)
            movement.canMove = true;

        Debug.Log("SPAWNED AT CHECKPOINT: " + spawn.name + " | Completed Step: " + completed);
    }
}