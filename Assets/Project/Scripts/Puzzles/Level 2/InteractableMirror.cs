using UnityEngine;

public class InteractableMirror : MonoBehaviour
{
    public float rotateAmount = 45f;

    public void Interact()
    {
        transform.Rotate(0f, rotateAmount, 0f, Space.World);
        Debug.Log(gameObject.name + " rotated");
    }
}