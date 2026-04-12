using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
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

        transform.Translate(Vector3.forward * move * speed * Time.deltaTime);
        transform.Rotate(Vector3.up * turn * rotationSpeed * Time.deltaTime);

        if (animator != null)
        {
            animator.SetFloat("Speed", move);
        }
    }
}