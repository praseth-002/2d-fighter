using UnityEngine;

public enum PlayerState
{
    Idle,
    Moving,
    Jumping,
    Attacking,
    Hit,
    Dead
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    public bool isPlayer2 = false;
    public Transform opponent;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float doubleTapTime = 0.25f;

    [Header("Attack Durations")]
    public float punchDuration = 0.25f;
    public float kickDuration = 0.40f;
    public float specialDuration = 0.50f;

    public GameObject punchHitbox;
    public GameObject kickHitbox;

    private Rigidbody2D rb;
    private Animator animator;

    // Input
    private PlayerControls controlsP1;
    private PlayerControls1 controlsP2;

    private PlayerState currentState = PlayerState.Idle;

    private bool isGrounded;
    private bool isAttacking;
    private float attackTime;

    private bool isDashing;
    private float dashTimeLeft;
    private float lastTapTime;
    private float lastDirection;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!isPlayer2)
        {
            controlsP1 = new PlayerControls();

            controlsP1.Player.Jump.performed += _ => Jump();
            controlsP1.Player.Move.performed += ctx => OnMoveInput(ctx.ReadValue<float>());
            controlsP1.Player.Punch.performed += _ => Punch();
            controlsP1.Player.Kick.performed += _ => Kick();
            controlsP1.Player.Special.performed += _ => Special();
        }
        else
        {
            controlsP2 = new PlayerControls1();

            controlsP2.Player.Jump.performed += _ => Jump();
            controlsP2.Player.Move.performed += ctx => OnMoveInput(ctx.ReadValue<float>());
            controlsP2.Player.Punch.performed += _ => Punch();
            controlsP2.Player.Kick.performed += _ => Kick();
            controlsP2.Player.Special.performed += _ => Special();
        }
    }


    private void OnEnable()
    {
        if (!isPlayer2) controlsP1.Enable();
        else controlsP2.Enable();
    }

    private void OnDisable()
    {
        if (!isPlayer2) controlsP1.Disable();
        else controlsP2.Disable();
    }

    private void Update()
    {
        if (currentState == PlayerState.Hit || currentState == PlayerState.Dead)
            return;

        FaceOpponent();

        if (isAttacking)
        {
            animator.SetBool("IsRunning", false);
            attackTime -= Time.deltaTime;

            if (attackTime <= 0f)
            {
                isAttacking = false;
                DisableHitboxes();
                currentState = PlayerState.Idle;
            }
            return;
        }

        float move = GetMoveInput();

        // Dash handling
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            rb.velocity = new Vector2(lastDirection * dashSpeed, rb.velocity.y);

            if (dashTimeLeft <= 0f)
                isDashing = false;

            return;
        }

        if (isGrounded)
        {
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
            animator.SetBool("IsRunning", move != 0);
            currentState = move != 0 ? PlayerState.Moving : PlayerState.Idle;
        }
        else
        {
            animator.SetBool("IsRunning", false);
            currentState = PlayerState.Jumping;
        }
    }

    private float GetMoveInput()
    {
        return !isPlayer2
       ? controlsP1.Player.Move.ReadValue<float>()
       : controlsP2.Player.Move.ReadValue<float>();
    }

    private void OnMoveInput(float move)
    {
        if (!isGrounded || move == 0) return;

        if (move == lastDirection && Time.time - lastTapTime < doubleTapTime)
        {
            StartDash(move);
        }
        else
        {
            lastDirection = move;
            lastTapTime = Time.time;
        }
    }

    private void Jump()
    {
        if (!isGrounded) return;

        float move = GetMoveInput();
        rb.velocity = new Vector2(move * moveSpeed, jumpForce);

        animator.SetBool("IsJumping", true);
        animator.Play("PlayerJump", 0, 0f);

        isGrounded = false;
        currentState = PlayerState.Jumping;
    }

    private void Punch()
    {
        if (isAttacking || !isGrounded) return;

        isAttacking = true;
        attackTime = punchDuration;
        currentState = PlayerState.Attacking;

        animator.Play("PlayerPunch", 0, 0f);
        punchHitbox.SetActive(true);
    }

    private void Kick()
    {
        if (isAttacking || !isGrounded) return;

        isAttacking = true;
        attackTime = kickDuration;
        currentState = PlayerState.Attacking;

        animator.Play("PlayerKick", 0, 0f);
        kickHitbox.SetActive(true);
    }

    private void Special()
    {
        // Reserved for later
    }

    private void StartDash(float direction)
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        lastDirection = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Ground")) return;

        isGrounded = true;
        animator.SetBool("IsJumping", false);

        if (currentState != PlayerState.Hit && currentState != PlayerState.Dead)
            currentState = PlayerState.Idle;
    }

    public void DisableHitboxes()
    {
        punchHitbox.SetActive(false);
        kickHitbox.SetActive(false);
    }

    public void EnterHitState(float duration)
    {
        currentState = PlayerState.Hit;
        Invoke(nameof(ExitHitState), duration);
    }

    private void ExitHitState()
    {
        if (currentState != PlayerState.Dead)
            currentState = PlayerState.Idle;
    }

    public void EnterDeadState()
    {
        currentState = PlayerState.Dead;
        rb.velocity = Vector2.zero;
    }

    private void FaceOpponent()
    {
        if (!opponent) return;

        if (transform.position.x < opponent.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }
    // private void FaceOpponent()
    // {
    //     if (!opponent || !spriteRenderer) return;

    //     // Face right if opponent is on the right
    //     bool opponentOnRight = opponent.position.x > transform.position.x;

    //     // flipX = true means sprite faces LEFT
    //     spriteRenderer.flipX = !opponentOnRight;
    // }
}
