using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    private Rigidbody2D rb;
    private PlayerControls controls;

    private float moveInput;   // 1D axis (A = -1, D = 1)
    private bool isGrounded;

    private float jumpDirection = 0f;

    [SerializeField] private Animator animator;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();

        controls.Player.Jump.performed += ctx => Jump();
    }

     private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        float move = controls.Player.Move.ReadValue<float>();

        if (isGrounded)
        {
            // Free ground movement
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        }

        if (move != 0)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    private void TryJump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

     private void Jump()
    {
        if (!isGrounded) return;

        float move = controls.Player.Move.ReadValue<float>();

        // SIMPLE: choose horizontal force ON jump
        float jumpX = move * moveSpeed;

        // Apply velocity ONCE
        rb.velocity = new Vector2(jumpX, jumpForce);

        isGrounded = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
         if (collision.collider.CompareTag("Ground"))
            isGrounded = true;
    }
}
