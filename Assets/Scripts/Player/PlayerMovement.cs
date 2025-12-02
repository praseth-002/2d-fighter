// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerMovement : MonoBehaviour
// {
//     [Header("Movement Settings")]
//     public float moveSpeed = 5f;
//     public float jumpForce = 12f;

//     private Rigidbody2D rb;
//     private PlayerControls controls;

//     private float moveInput;   // 1D axis (A = -1, D = 1)
//     private bool isGrounded;


//     // private float jumpDirection = 0f;

//     [SerializeField] private Animator animator;

//     [Header("Dash Settings")]
//     public float dashSpeed = 15f;
//     public float dashDuration = 0.2f;
//     public float doubleTapTime = 0.25f;

//     private float lastTapTime = 0f;
//     private float lastDirection = 0f;
//     private bool isDashing = false;
//     private float dashTimeLeft = 0f;


//     private void Awake()
//     {

//         rb = GetComponent<Rigidbody2D>();
//         controls = new PlayerControls();

//         controls.Player.Jump.performed += ctx => Jump();
//     }

//     private void OnEnable() => controls.Enable();
//     private void OnDisable() => controls.Disable();

//     private void Update()
//     {
//         float move = controls.Player.Move.ReadValue<float>();

//         if (isGrounded)
//         {
//             // Free ground movement
//             rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

//             // Only run animation if on the ground
//             if (move != 0)
//                 animator.SetBool("IsRunning", true);
//             else
//                 animator.SetBool("IsRunning", false);
//         }
//         else
//         {
//             // In the air, make sure running animation is off
//             animator.SetBool("IsRunning", false);
//         }
//     }

//     // private void Update()
//     // {
//     //     float move = controls.Player.Move.ReadValue<float>();

//     //     if (!isDashing)
//     //     {
//     //         // Double-tap detection
//     //         if (move != 0)
//     //         {
//     //             if (move == lastDirection && (Time.time - lastTapTime) < doubleTapTime)
//     //             {
//     //                 StartDash(move);
//     //             }
//     //             else
//     //             {
//     //                 lastDirection = move;
//     //                 lastTapTime = Time.time;
//     //             }
//     //         }

//     //         // Normal movement
//     //         if (isGrounded)
//     //         {
//     //             rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
//     //             animator.SetBool("IsRunning", move != 0);
//     //         }
//     //         else
//     //         {
//     //             animator.SetBool("IsRunning", false);
//     //         }
//     //     }
//     //     else
//     //     {
//     //         // Dash movement stays until dash ends
//     //         dashTimeLeft -= Time.deltaTime;
//     //         if (dashTimeLeft <= 0f)
//     //             isDashing = false;
//     //     }
//     // }


//     private void TryJump()
//     {
//         if (isGrounded)
//         {
//             rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//             isGrounded = false;
//         }
//     }

//     private void Jump()
//     {
//         if (!isGrounded) return;

//         float move = controls.Player.Move.ReadValue<float>();

//         // Apply horizontal + vertical jump velocity
//         float jumpX = move * moveSpeed;
//         rb.velocity = new Vector2(jumpX, jumpForce);

//         // Trigger jump animation
//         animator.SetBool("IsJumping", true);

//         isGrounded = false;
//     }


//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.collider.CompareTag("Ground"))
//         {
//             isGrounded = true;

//             // Return to idle/running animation
//             animator.SetBool("IsJumping", false);
//         }
//     }

//     // private void StartDash(float direction)
//     // {
//     //     isDashing = true;
//     //     dashTimeLeft = dashDuration;

//     //     // Apply dash velocity instantly
//     //     rb.velocity = new Vector2(direction * dashSpeed, rb.velocity.y);

//     //     // Optional: dash animation
//     //     // animator.Play("Dash", -1, 0f);
//     // }

// }


using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float doubleTapTime = 0.25f;

    private Rigidbody2D rb;
    private PlayerControls controls;

    private bool isGrounded;
    private bool isDashing = false;
    private float dashTimeLeft = 0f;

    private float lastTapTime = 0f;
    private float lastDirection = 0f;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();

        // Jump input
        controls.Player.Jump.performed += ctx => Jump();

        // Move input (for double-tap dash detection)
        controls.Player.Move.performed += ctx => OnMoveInput(ctx.ReadValue<float>());
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        float move = controls.Player.Move.ReadValue<float>();

        // Dash logic
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;

            if (dashTimeLeft <= 0f)
                isDashing = false;

            // Lock horizontal velocity while dashing
            rb.velocity = new Vector2(lastDirection * dashSpeed, rb.velocity.y);

            // Skip normal movement/animation while dashing
            animator.SetBool("IsRunning", false);
            return;
        }

        // Normal horizontal movement
        if (isGrounded)
        {
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

            // Running animation
            animator.SetBool("IsRunning", move != 0);
        }
        else
        {
            // In air: no running animation
            animator.SetBool("IsRunning", false);
        }
    }


    private void OnMoveInput(float move)
    {
        if (move != 0 && isGrounded) // Only allow dash if grounded
        {
            // Double-tap detection
            if (move == lastDirection && (Time.time - lastTapTime) < doubleTapTime)
            {
                StartDash(move);
            }
            else
            {
                lastDirection = move;
                lastTapTime = Time.time;
            }
        }
    }

    private void Jump()
    {
        if (!isGrounded) return;

        float move = controls.Player.Move.ReadValue<float>();
        float jumpX = move * moveSpeed;

        rb.velocity = new Vector2(jumpX, jumpForce);

        animator.SetBool("IsJumping", true);
        animator.Play("PlayerJump", 0, 0f);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
        }
    }

    private void StartDash(float direction)
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        lastDirection = direction;

        // Apply dash velocity instantly
        rb.velocity = new Vector2(direction * dashSpeed, rb.velocity.y);

        // Optional: dash animation
        // animator.Play("Dash", -1, 0f);
    }
}
