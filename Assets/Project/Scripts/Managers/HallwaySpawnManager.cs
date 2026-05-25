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

    void Start()
    {
        int completed = GameProgressManager.Instance.latestCompletedStep;

        Transform spawn = defaultSpawn;

        if (completed == 0) spawn = afterPrelevelSpawn;
        else if (completed == 1) spawn = afterLevel1Spawn;
        else if (completed == 2) spawn = afterLevel2Spawn;
        else if (completed == 3) spawn = afterLevel3Spawn;
        else if (completed == 4) spawn = afterLevel4Spawn;
        else if (completed >= 5) spawn = afterLevel5Spawn;

        player.position = spawn.position;
        player.rotation = spawn.rotation;
    }
}