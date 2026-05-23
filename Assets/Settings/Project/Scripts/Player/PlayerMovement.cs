using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 180f;

    private Animator animator;
    private Rigidbody rb;

    private float moveInput;
    private float turnInput;
    private bool isRunning;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (animator != null)
        {
            float animValue = 0f;

            if (isRunning && moveInput > 0)
                animValue = 2f;
            else if (moveInput > 0)
                animValue = 1f;
            else if (moveInput < 0)
                animValue = -1f;
            else
                animValue = 0f;

            animator.SetFloat("Speed", animValue);
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.forward * moveInput * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        Quaternion turnRotation = Quaternion.Euler(0f, turnInput * rotationSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}