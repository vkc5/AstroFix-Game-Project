using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 150f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Vertical");
        float turn = Input.GetAxisRaw("Horizontal");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        transform.Translate(Vector3.forward * move * currentSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * turn * rotationSpeed * Time.deltaTime);

        if (animator != null)
        {
            float animValue = move;

            if (isRunning && move > 0)
                animValue = 2f;
            else if (move > 0)
                animValue = 1f;
            else if (move < 0)
                animValue = -1f;
            else
                animValue = 0f;

            animator.SetFloat("Speed", animValue);
        }
    }
}