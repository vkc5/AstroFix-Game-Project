using UnityEngine;

public class LaserReflectionSystem : MonoBehaviour
{
    public Transform firePoint;
    public LineRenderer lineRenderer;
    public float maxDistance = 60f;
    public int maxBounces = 5;
    public LayerMask hitLayers;

    [Header("HUD")]
    public Transform hudIcon;
    public Transform defaultHudTarget;

    [Header("Hit Effect")]
    public GameObject hitEffect;
    public float hitOffset = 0.05f;

    private ParticleSystem[] hitParticles;

    public bool laserOn;
    public Transform currentTarget;

    void Start()
    {
        lineRenderer.enabled = false;

        if (hitEffect != null)
        {
            hitParticles = hitEffect.GetComponentsInChildren<ParticleSystem>();
            StopHitEffect();
        }

        ShowHUD(defaultHudTarget);
    }

    void Update()
    {
        if (!laserOn)
        {
            lineRenderer.enabled = false;

            if (hitEffect != null)
                StopHitEffect();

            ShowHUD(defaultHudTarget);
            return;
        }

        lineRenderer.enabled = true;
        DrawLaser();
    }

    public void TurnOn()
    {
        laserOn = true;
    }

    void DrawLaser()
    {
        Vector3 start = firePoint.position;
        Vector3 direction = firePoint.forward;

        currentTarget = null;
        bool hitSomething = false;

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, start);

        for (int i = 0; i < maxBounces; i++)
        {
            if (Physics.Raycast(start, direction, out RaycastHit hit, maxDistance, hitLayers))
            {
                hitSomething = true;
                AddPoint(hit.point);
                MoveHitEffect(hit.point, hit.normal);

                if (hit.collider.CompareTag("Mirror"))
                {
                    currentTarget = hit.collider.transform;
                    ShowHUD(currentTarget);

                    direction = Vector3.Reflect(direction, hit.normal).normalized;
                    start = hit.point + direction * 0.05f;
                    continue;
                }

                if (hit.collider.CompareTag("Generator"))
                {
                    currentTarget = hit.collider.transform;
                    ShowHUD(currentTarget);
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

        if (!hitSomething && hitEffect != null)
            StopHitEffect();
        if (currentTarget == null)
            ShowHUD(defaultHudTarget);
    }

    void MoveHitEffect(Vector3 hitPoint, Vector3 hitNormal)
    {
        if (hitEffect == null) return;

        hitEffect.transform.position = hitPoint + hitNormal * hitOffset;
        hitEffect.transform.LookAt(hitPoint + hitNormal);

        foreach (ParticleSystem ps in hitParticles)
        {
            if (!ps.isPlaying)
                ps.Play();
        }
    }

    void AddPoint(Vector3 point)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
    }

    void ShowHUD(Transform target)
    {
        if (hudIcon == null || target == null) return;

        hudIcon.gameObject.SetActive(true);
        hudIcon.SetParent(null); // important
        hudIcon.position = target.position + Vector3.up * 2f;
        hudIcon.rotation = Quaternion.identity;
    }
    void StopHitEffect()
    {
        if (hitParticles == null) return;

        foreach (ParticleSystem ps in hitParticles)
        {
            if (ps.isPlaying)
                ps.Stop();
        }
    }
}