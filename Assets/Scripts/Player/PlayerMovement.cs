using UnityEngine;

public enum PlayerState
{
    Idle,
    Moving,
    Jumping,
    Attacking,
    Blocking,
    Dashing,
    Hit,
    Dead
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    public bool isPlayer2;
    public Transform opponent;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Dash")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.18f;
    public float doubleTapTime = 0.25f;

    [Header("Attack Durations")]
    public float punchDuration = 0.25f;
    public float kickDuration = 0.40f;

    public GameObject punchHitbox;
    public GameObject kickHitbox;

    private Rigidbody2D rb;
    private Animator animator;

    private PlayerControls controlsP1;
    private PlayerControls1 controlsP2;

    private PlayerState state = PlayerState.Idle;
    private bool isGrounded;

    // timers
    private float attackTimer;
    private float dashTimer;
    private float hitTimer;

    // input
    private float moveInput;
    private bool punchHeld;
    private bool kickHeld;
    private bool blockHeld;

    // dash detection
    private float lastTapTime;
    private float lastTapDirection;
    private bool airDashUsed;

    private bool initialized = false;


    // private void Awake()
    // {
    //     rb = GetComponent<Rigidbody2D>();
    //     animator = GetComponent<Animator>();

    //     if (!isPlayer2)
    //     {
    //         controlsP1 = new PlayerControls();

    //         controlsP1.Player.Move.performed += ctx => OnMove(ctx.ReadValue<float>());
    //         controlsP1.Player.Move.canceled += _ => moveInput = 0f;

    //         controlsP1.Player.Jump.performed += _ => Jump();

    //         controlsP1.Player.Punch.performed += _ =>
    //         {
    //             if (punchHeld) return;
    //             punchHeld = true;
    //             Punch();
    //         };
    //         controlsP1.Player.Punch.canceled += _ => punchHeld = false;

    //         controlsP1.Player.Kick.performed += _ =>
    //         {
    //             if (kickHeld) return;
    //             kickHeld = true;
    //             Kick();
    //         };
    //         controlsP1.Player.Kick.canceled += _ => kickHeld = false;

    //         controlsP1.Player.Blocking.performed += _ => blockHeld = true;
    //         controlsP1.Player.Blocking.canceled += _ => blockHeld = false;
    //     }
    //     else
    //     {
    //         controlsP2 = new PlayerControls1();

    //         controlsP2.Player.Move.performed += ctx => OnMove(ctx.ReadValue<float>());
    //         controlsP2.Player.Move.canceled += _ => moveInput = 0f;

    //         controlsP2.Player.Jump.performed += _ => Jump();

    //         controlsP2.Player.Punch.performed += _ =>
    //         {
    //             if (punchHeld) return;
    //             punchHeld = true;
    //             Punch();
    //         };
    //         controlsP2.Player.Punch.canceled += _ => punchHeld = false;

    //         controlsP2.Player.Kick.performed += _ =>
    //         {
    //             if (kickHeld) return;
    //             kickHeld = true;
    //             Kick();
    //         };
    //         controlsP2.Player.Kick.canceled += _ => kickHeld = false;

    //         controlsP2.Player.Blocking.performed += _ => blockHeld = true;
    //         controlsP2.Player.Blocking.canceled += _ => blockHeld = false;
    //     }
    // }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Initialize(bool player2)
    {
        isPlayer2 = player2;

        if (!isPlayer2)
        {
            controlsP1 = new PlayerControls();

            controlsP1.Player.Move.performed += ctx => OnMove(ctx.ReadValue<float>());
            controlsP1.Player.Move.canceled += _ => moveInput = 0f;

            controlsP1.Player.Jump.performed += _ => Jump();

            controlsP1.Player.Punch.performed += _ =>
            {
                if (punchHeld) return;
                punchHeld = true;
                Punch();
            };
            controlsP1.Player.Punch.canceled += _ => punchHeld = false;

            controlsP1.Player.Kick.performed += _ =>
            {
                if (kickHeld) return;
                kickHeld = true;
                Kick();
            };
            controlsP1.Player.Kick.canceled += _ => kickHeld = false;

            controlsP1.Player.Blocking.performed += _ => blockHeld = true;
            controlsP1.Player.Blocking.canceled += _ => blockHeld = false;

            controlsP1.Enable();
        }
        else
        {
            controlsP2 = new PlayerControls1();

            controlsP2.Player.Move.performed += ctx => OnMove(ctx.ReadValue<float>());
            controlsP2.Player.Move.canceled += _ => moveInput = 0f;

            controlsP2.Player.Jump.performed += _ => Jump();

            controlsP2.Player.Punch.performed += _ =>
            {
                if (punchHeld) return;
                punchHeld = true;
                Punch();
            };
            controlsP2.Player.Punch.canceled += _ => punchHeld = false;

            controlsP2.Player.Kick.performed += _ =>
            {
                if (kickHeld) return;
                kickHeld = true;
                Kick();
            };
            controlsP2.Player.Kick.canceled += _ => kickHeld = false;

            controlsP2.Player.Blocking.performed += _ => blockHeld = true;
            controlsP2.Player.Blocking.canceled += _ => blockHeld = false;

            controlsP2.Enable();
        }
        initialized = true;

    }


    // private void OnEnable()
    // {
    //     if (!isPlayer2) controlsP1.Enable();
    //     else controlsP2.Enable();
    // }
    private void OnEnable()
{
    if (!initialized) return;

    if (!isPlayer2 && controlsP1 != null)
        controlsP1.Enable();
    else if (isPlayer2 && controlsP2 != null)
        controlsP2.Enable();
}


    // private void OnDisable()
    // {
    //     if (!isPlayer2) controlsP1.Disable();
    //     else controlsP2.Disable();
    // }

private void OnDisable()
{
    if (!initialized) return;

    if (!isPlayer2 && controlsP1 != null)
        controlsP1.Disable();
    else if (isPlayer2 && controlsP2 != null)
        controlsP2.Disable();
}
    private void Update()
    {
        FaceOpponent();

        if (state == PlayerState.Dead)
            return;

        // HIT STUN
        if (state == PlayerState.Hit)
        {
            hitTimer -= Time.unscaledDeltaTime;
            if (hitTimer <= 0f)
                state = PlayerState.Idle;
            return;
        }

        // DASH
        if (state == PlayerState.Dashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
                state = isGrounded ? PlayerState.Idle : PlayerState.Jumping;
            return;
        }

        // BLOCK
        if (blockHeld && isGrounded && state != PlayerState.Attacking)
        {
            if (state != PlayerState.Blocking)
            {
                state = PlayerState.Blocking;
                animator.Play("PlayerBlock", 0, 0f);
            }

            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        // ATTACK
        if (state == PlayerState.Attacking)
        {
            attackTimer -= Time.unscaledDeltaTime;

            // FIX: lock movement ONLY on ground, keep air momentum
            if (isGrounded)
                rb.velocity = new Vector2(0, rb.velocity.y);

            if (attackTimer <= 0f)
            {
                DisableHitboxes();
                state = isGrounded ? PlayerState.Idle : PlayerState.Jumping;
            }
            return;
        }

        // MOVE
        if (isGrounded)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            if (Mathf.Abs(moveInput) > 0.01f)
            {
                if (state != PlayerState.Moving)
                {
                    state = PlayerState.Moving;
                    animator.Play("PlayerWalk", 0, 0f);
                }
            }
            else
            {
                if (state != PlayerState.Idle)
                {
                    state = PlayerState.Idle;
                    animator.Play("PlayerIdle", 0, 0f);
                }
            }
        }
    }

    // ================= INPUT HANDLING =================

    private void OnMove(float input)
    {
        moveInput = input;

        if (Mathf.Abs(input) < 0.1f)
            return;

        float dir = Mathf.Sign(input);

        if (dir == lastTapDirection && Time.time - lastTapTime <= doubleTapTime)
        {
            TryDash(dir);
            lastTapTime = 0f;
        }
        else
        {
            lastTapDirection = dir;
            lastTapTime = Time.time;
        }
    }

    // ================= ACTIONS =================

    private void TryDash(float dir)
    {
        if (state == PlayerState.Attacking ||
            state == PlayerState.Blocking ||
            state == PlayerState.Hit ||
            state == PlayerState.Dead)
            return;

        if (!isGrounded && airDashUsed)
            return;

        if (!isGrounded)
            airDashUsed = true;

        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(dir * dashSpeed, 0f);

        dashTimer = dashDuration;
        state = PlayerState.Dashing;

        animator.Play("PlayerDash", 0, 0f);
    }

    private void Punch()
    {
        if (state == PlayerState.Attacking ||
            state == PlayerState.Blocking ||
            state == PlayerState.Dashing ||
            state == PlayerState.Hit ||
            state == PlayerState.Dead)
            return;

        DisableHitboxes();

        attackTimer = punchDuration;
        punchHitbox.SetActive(true);

        state = PlayerState.Attacking;

        animator.Play(isGrounded ? "PlayerPunch" : "PlayerJumpPunch", 0, 0f);
    }

    private void Kick()
    {
        if (state == PlayerState.Attacking ||
            state == PlayerState.Blocking ||
            state == PlayerState.Dashing ||
            state == PlayerState.Hit ||
            state == PlayerState.Dead)
            return;

        DisableHitboxes();

        attackTimer = kickDuration;
        kickHitbox.SetActive(true);

        state = PlayerState.Attacking;

        animator.Play(isGrounded ? "PlayerKick" : "PlayerJumpKick", 0, 0f);
    }

    private void Jump()
    {
        if (!isGrounded ||
            state == PlayerState.Attacking ||
            state == PlayerState.Blocking ||
            state == PlayerState.Dashing ||
            state == PlayerState.Hit ||
            state == PlayerState.Dead)
            return;

        rb.velocity = new Vector2(moveInput * moveSpeed, jumpForce);
        isGrounded = false;

        state = PlayerState.Jumping;
        animator.Play("PlayerJump", 0, 0f);
    }

    public void EnterHitState(float duration)
    {
        DisableHitboxes();
        hitTimer = duration;
        state = PlayerState.Hit;
        animator.Play("PlayerDamaged", 0, 0f);
    }

    public void EnterDeadState()
    {
        state = PlayerState.Dead;
        rb.velocity = Vector2.zero;
        animator.Play("PlayerDeath", 0, 0f);
    }

    // ================= HELPERS =================

    public bool IsBlocking()
    {
        return state == PlayerState.Blocking;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("Ground")) return;

        isGrounded = true;
        airDashUsed = false;

        if (state == PlayerState.Jumping || state == PlayerState.Dashing)
        {
            state = PlayerState.Idle;
            animator.Play("PlayerIdle", 0, 0f);
        }
    }

    private void DisableHitboxes()
    {
        punchHitbox.SetActive(false);
        kickHitbox.SetActive(false);
    }

    private void FaceOpponent()
    {
        if (!opponent) return;

        transform.localScale =
            transform.position.x < opponent.position.x
                ? Vector3.one
                : new Vector3(-1, 1, 1);
    }
}

