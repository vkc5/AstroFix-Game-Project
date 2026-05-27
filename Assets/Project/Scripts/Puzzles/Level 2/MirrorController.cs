using UnityEngine;

public class MirrorController : MonoBehaviour
{
    public float rotateSpeed = 50f;
    public bool isControlled = false;

    void Update()
    {
        if (!isControlled) return;

        float rotate = Input.GetAxisRaw("Horizontal");
        transform.Rotate(0f, rotate * rotateSpeed * Time.deltaTime, 0f);
    }

    public void ToggleControl()
    {
        isControlled = !isControlled;
    }
}