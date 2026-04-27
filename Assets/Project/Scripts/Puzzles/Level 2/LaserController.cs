using UnityEngine;

public class LaserController : MonoBehaviour
{
    public LaserReflectionSystem laserReflection;
    public Transform laserPivot;
    public Transform sphereVisual; // NEW

    public float rotateSpeed = 80f;
    public bool isControlled;

    void Update()
    {
        if (!isControlled) return;

        float rotate = Input.GetAxisRaw("Horizontal");
        float rotationAmount = rotate * rotateSpeed * Time.deltaTime;

        // keep your original working rotation
        laserPivot.Rotate(0f, rotationAmount, 0f);

        // add sphere rotation WITHOUT affecting system
        if (sphereVisual != null)
        {
            sphereVisual.Rotate(0f, rotationAmount, 0f);
        }
    }

    public void ToggleControl()
    {
        isControlled = !isControlled;
        laserReflection.TurnOn(); // KEEP AS IS (since yours works)
    }
}