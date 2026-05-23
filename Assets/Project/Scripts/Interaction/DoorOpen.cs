using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    public Vector3 leftOpenOffset = new Vector3(-2f, 0f, 0f);
    public Vector3 rightOpenOffset = new Vector3(2f, 0f, 0f);

    public float speed = 2f;

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;

    private Vector3 leftTargetPos;
    private Vector3 rightTargetPos;

    private bool open = false;

    void Start()
    {
        leftClosedPos = leftDoor.localPosition;
        rightClosedPos = rightDoor.localPosition;

        leftTargetPos = leftClosedPos;
        rightTargetPos = rightClosedPos;
    }

    void Update()
    {
        if (open)
        {
            leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftTargetPos, Time.deltaTime * speed);
            rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightTargetPos, Time.deltaTime * speed);
        }
    }

    public void OpenDoor()
    {
        leftTargetPos = leftClosedPos + leftOpenOffset;
        rightTargetPos = rightClosedPos + rightOpenOffset;
        open = true;
    }
}