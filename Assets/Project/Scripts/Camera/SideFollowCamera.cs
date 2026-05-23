using UnityEngine;

public class SideFollowCamera : MonoBehaviour
{
    public Transform target;

    [Header("Fixed Side Position")]
    public float fixedX = 13.5f;     // wall side position
    public float height = 8f;        // camera height
    public float sideDistanceZ = 0f; // extra offset

    [Header("Follow")]
    public bool followZ = true;
    public float followSpeed = 5f;

    [Header("Look")]
    public float lookHeight = 1.5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = transform.position;

        desiredPosition.x = fixedX;
        desiredPosition.y = height;

        if (followZ)
            desiredPosition.z = target.position.z + sideDistanceZ;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        Vector3 lookTarget = target.position + Vector3.up * lookHeight;
        transform.LookAt(lookTarget);
    }
}