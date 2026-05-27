using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Position")]
    public float distance = 2f;
    public float height = 0f;
    public float smoothSpeed = 10f;

    [Header("Rotation")]
    public float mouseSensitivity = 3f;
    public float minPitch = -25f;
    public float maxPitch = 45f;

    [Header("Collision")]
    public LayerMask collisionMask = ~0;
    public float collisionRadius = 0.25f;
    public float minDistance = 1.2f;
    public float collisionOffset = 0.2f;

    private float yaw;
    private float pitch = 10f;
    private Vector3 velocity;
    void Start()
    {
        if (target != null)
        {
            yaw = target.eulerAngles.y;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPoint = target.position + Vector3.up * height;
        Vector3 desiredCameraPosition = targetPoint - rotation * Vector3.forward * distance;

        Vector3 direction = desiredCameraPosition - targetPoint;
        float finalDistance = distance;

        if (Physics.SphereCast(
            targetPoint,
            collisionRadius,
            direction.normalized,
            out RaycastHit hit,
            distance,
            collisionMask,
            QueryTriggerInteraction.Ignore))
        {
            finalDistance = Mathf.Clamp(hit.distance - collisionOffset, minDistance, distance);
        }

        Vector3 finalCameraPosition = targetPoint - rotation * Vector3.forward * finalDistance;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            finalCameraPosition,
            ref velocity,
            0.08f
        );
        transform.LookAt(targetPoint);
    }
}