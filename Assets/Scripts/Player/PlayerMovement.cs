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

    private bool isAttacking = false;
    private float attackTime;

    [Header("Attack Durations")]
    public float punchDuration = 0.25f;
    public float kickDuration = 0.40f;
    public float specialDuration = 0.50f;

    public GameObject punchHitbox;
    public GameObject kickHitbox;
    // public GameObject specialHitbox;

    public void EnablePunchHitbox()
    {
        punchHitbox.SetActive(true);
        Debug.Log("Punch Hitbox ENABLED");
    }

    public void EnableKickHitbox()
    {
        kickHitbox.SetActive(true);
        Debug.Log("Kick Hitbox ENABLED");
    }

    // public void EnableSpecialHitbox()
    // {
    //     specialHitbox.SetActive(true);
    //     Debug.Log("Special Hitbox ENABLED");
    // }

    public void DisableHitboxes()
    {
        punchHitbox.SetActive(false);
        kickHitbox.SetActive(false);
        // specialHitbox.SetActive(false);

        Debug.Log("All Hitboxes DISABLED");
    }




    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();

        // Jump input
        controls.Player.Jump.performed += ctx => Jump();

        // Move input (for double-tap dash detection)
        controls.Player.Move.performed += ctx => OnMoveInput(ctx.ReadValue<float>());

        controls.Player.Punch.performed += ctx => Punch();
        controls.Player.Kick.performed += ctx => Kick();
        controls.Player.Special.performed += ctx => Special();

    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        // If attacking, freeze movement
        if (isAttacking)
        {
            animator.SetBool("IsRunning", false);

            attackTime -= Time.deltaTime;

            if (attackTime <= 0f)
                isAttacking = false;

            return;
        }

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
        animator.SetBool("IsRunning", false);
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

    private void Punch()
    {
        if (isAttacking || !isGrounded) return;

        isAttacking = true;
        attackTime = punchDuration;

        animator.Play("PlayerPunch", 0, 0f);
    }

    private void Kick()
    {
        if (isAttacking || !isGrounded) return;

        isAttacking = true;
        attackTime = kickDuration;

        animator.Play("PlayerKick", 0, 0f);
    }

    private void Special()
    {
        if (isAttacking || !isGrounded) return;

        isAttacking = true;
        attackTime = specialDuration;

        animator.Play("PlayerSpecial", 0, 0f);
    }


}
