using UnityEngine;

public class PuzzleLaserRaycast : MonoBehaviour
{
    public Transform firePoint;
    public LineRenderer lineRenderer;

    public float maxDistance = 100f;
    public int maxBounces = 7;
    public bool laserOn = false;

    public LayerMask laserHitLayers;

    void Start()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!laserOn)
        {
            if (lineRenderer != null)
                lineRenderer.enabled = false;
            return;
        }

        DrawLaser();
    }

    public void TurnOn()
    {
        laserOn = true;

        if (lineRenderer != null)
            lineRenderer.enabled = true;
    }

    void DrawLaser()
    {
        Vector3 start = firePoint.position;
        Vector3 direction = firePoint.forward;

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, start);

        for (int i = 0; i < maxBounces; i++)
        {
            if (Physics.Raycast(start, direction, out RaycastHit hit, maxDistance, laserHitLayers))
            {
                AddPoint(hit.point);

                if (hit.collider.CompareTag("Mirror"))
                {
                    direction = Vector3.Reflect(direction, hit.normal).normalized;
                    start = hit.point + direction * 0.05f;
                    continue;
                }

                if (hit.collider.CompareTag("Generator"))
                {
                    Debug.Log("Generator activated!");
                    hit.collider.GetComponent<LaserReceiver>()?.Activate();
                }

                break;
            }
            else
            {
                AddPoint(start + direction * maxDistance);
                break;
            }
        }
    }

    void AddPoint(Vector3 point)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
    }
}