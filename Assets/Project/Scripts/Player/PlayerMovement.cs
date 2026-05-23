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

    // New variable to control whether the player can move or not
    private bool canMove = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if movement is allowed before processing inputs
        if (canMove)
        {
            moveInput = Input.GetAxisRaw("Vertical");
            turnInput = Input.GetAxisRaw("Horizontal");
            isRunning = Input.GetKey(KeyCode.LeftShift);
        }
        else
        {
            // Reset inputs to stop movement and animations instantly when movement is disabled
            moveInput = 0f;
            turnInput = 0f;
            isRunning = false;
        }

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

    /// <summary>
    /// Public function to enable or disable player movement from other scripts (e.g., Interaction System)
    /// </summary>
    /// <param name="state">True to enable movement, False to disable it</param>
    public void SetMovementState(bool state)
    {
        canMove = state;
    }
}