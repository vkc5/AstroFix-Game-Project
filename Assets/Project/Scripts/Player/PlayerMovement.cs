using UnityEngine;
using Unity.VisualScripting;

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
    public bool canMove = true;

    [Header("Flashlight System Settings")]
    [SerializeField] private GameObject flashlightFloorPrefab;
    [SerializeField] private GameObject thirdPersonCameraRef;

    private bool isFlashlightMode = false;
    private GameObject fpvCameraRef;
    private float fpvVerticalRotation = 0f;
    private float mouseSensitivity = 2f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        FirstPersonCameraBackupInit();
    }

    private void FirstPersonCameraBackupInit()
    {
        if (fpvCameraRef == null)
        {
            Transform fpvCamTransform = transform.Find("FirstPersonCamera");
            if (fpvCamTransform != null)
            {
                fpvCamTransform.gameObject.SetActive(false);
            }
        }
    }

    public void SetFlashlightMode(bool active, GameObject fpvCamera)
    {
        isFlashlightMode = active;
        fpvCameraRef = fpvCamera;
        fpvVerticalRotation = 0f;
        canMove = true;
    }

    void Update()
    {
        if (isFlashlightMode && Input.GetKeyDown(KeyCode.R))
        {
            DropFlashlightAndReturnToThirdPerson();
            return;
        }

        if (!canMove)
        {
            moveInput = 0;
            turnInput = 0;

            if (animator != null)
                animator.SetFloat("Speed", 0);

            return;
        }

        moveInput = Input.GetAxisRaw("Vertical");

        if (!isFlashlightMode)
        {
            turnInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            turnInput = 0f;
        }

        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (isFlashlightMode && fpvCameraRef != null)
        {
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            fpvVerticalRotation -= mouseY;
            fpvVerticalRotation = Mathf.Clamp(fpvVerticalRotation, -80f, 80f);
            fpvCameraRef.transform.localRotation = Quaternion.Euler(fpvVerticalRotation, 0f, 0f);
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

        if (isFlashlightMode)
        {
            rb.MovePosition(rb.position + move);

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            Quaternion mouseTurnRotation = Quaternion.Euler(0f, mouseX, 0f);
            rb.MoveRotation(rb.rotation * mouseTurnRotation);
        }
        else
        {
            rb.MovePosition(rb.position + move);
            Quaternion turnRotation = Quaternion.Euler(0f, turnInput * rotationSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

    public void SetMovementState(bool state)
    {
        canMove = state;
    }

    private void DropFlashlightAndReturnToThirdPerson()
    {
        isFlashlightMode = false;

        if (thirdPersonCameraRef != null)
        {
            thirdPersonCameraRef.SetActive(true);
        }

        if (fpvCameraRef != null)
        {
            fpvCameraRef.SetActive(false);
        }

        if (flashlightFloorPrefab != null)
        {
            Vector3 spawnPosition = transform.position + transform.forward * 1.2f + Vector3.up * 0.2f;
            GameObject droppedFlashlight = Instantiate(flashlightFloorPrefab, spawnPosition, transform.rotation);

            Rigidbody objRb = droppedFlashlight.GetComponent<Rigidbody>();
            if (objRb != null)
            {
                objRb.isKinematic = false;
            }
        }

        fpvCameraRef = null;
        canMove = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Flashlight dropped via R. Main Camera active.");
    }
}