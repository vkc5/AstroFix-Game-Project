using UnityEngine;

public class RandomSparkManager : MonoBehaviour
{
    [Header("Spark Danger Zone")]
    public GameObject sparkDangerZone;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Timing")]
    public float moveEverySeconds = 3f;

    private float timer;
    private int lastIndex = -1;

    private void Start()
    {
        Debug.Log("RandomSparkManager started");
        MoveSparkToRandomPoint();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= moveEverySeconds)
        {
            timer = 0f;
            MoveSparkToRandomPoint();
        }
    }

    private void MoveSparkToRandomPoint()
    {
        if (sparkDangerZone == null)
        {
            Debug.LogWarning("Spark Danger Zone is missing!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No Spark Spawn Points assigned!");
            return;
        }

        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        }
        while (randomIndex == lastIndex && spawnPoints.Length > 1);

        lastIndex = randomIndex;

        sparkDangerZone.transform.position = spawnPoints[randomIndex].position;
        sparkDangerZone.transform.rotation = spawnPoints[randomIndex].rotation;

        Debug.Log("Spark moved to: " + spawnPoints[randomIndex].name);
    }
}