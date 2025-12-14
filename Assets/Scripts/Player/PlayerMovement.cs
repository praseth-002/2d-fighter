using UnityEngine;

public enum PlayerState
{
    Idle,
    Moving,
    Jumping,
    Attacking,
    Blocking,
    Hit,
    Dead
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    public bool isPlayer2 = false;
    public Transform opponent;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float doubleTapTime = 0.25f;

    [Header("Attack Durations")]
    public float punchDuration = 0.25f;
    public float kickDuration = 0.40f;

    [Header("Block Settings")]
    public float blockRange = 1.5f;

    public GameObject punchHitbox;
    public GameObject kickHitbox;

    private Rigidbody2D rb;
    private Animator animator;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (!isPlayer2)
        {
            controlsP1 = new PlayerControls();

            controlsP1.Player.Jump.performed += _ => Jump();
            controlsP1.Player.Move.performed += ctx => OnMoveInput(ctx.ReadValue<float>());
            controlsP1.Player.Punch.performed += _ => Punch();
            controlsP1.Player.Kick.performed += _ => Kick();
        }
        else
        {
            controlsP2 = new PlayerControls1();

            controlsP2.Player.Jump.performed += _ => Jump();
            controlsP2.Player.Move.performed += ctx => OnMoveInput(ctx.ReadValue<float>());
            controlsP2.Player.Punch.performed += _ => Punch();
            controlsP2.Player.Kick.performed += _ => Kick();
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
        EvaluateBlocking();

        if (currentState == PlayerState.Blocking)
        {
            animator.Play("PlayerBlock", 0, 0f);
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        if (isAttacking)
        {
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

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            rb.velocity = new Vector2(lastDirection * dashSpeed, rb.velocity.y);
            if (dashTimeLeft <= 0f) isDashing = false;
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

    // ================= BLOCK =================

    // private void EvaluateBlocking()
    // {
    //     if (!isGrounded || isAttacking || !opponent)
    //     {
    //         if (currentState == PlayerState.Blocking)
    //             currentState = PlayerState.Idle;
    //         return;
    //     }

    //     float distance = Mathf.Abs(opponent.position.x - transform.position.x);
    //     if (distance > blockRange)
    //     {
    //         if (currentState == PlayerState.Blocking)
    //             currentState = PlayerState.Idle;
    //         return;
    //     }

    //     float move = GetMoveInput();
    //     bool opponentOnRight = opponent.position.x > transform.position.x;

    //     bool holdingBack =
    //         (opponentOnRight && move < 0) ||
    //         (!opponentOnRight && move > 0);

    //     currentState = holdingBack ? PlayerState.Blocking : PlayerState.Idle;
    // }
    private void EvaluateBlocking()
    {
        if (!isGrounded || isAttacking || !opponent)
        {
            ExitBlockIfNeeded();
            return;
        }

        PlayerMovement opponentMovement = opponent.GetComponent<PlayerMovement>();
        if (opponentMovement == null || !opponentMovement.IsAttacking())
        {
            ExitBlockIfNeeded();
            return;
        }

        float distance = Mathf.Abs(opponent.position.x - transform.position.x);
        if (distance > blockRange)
        {
            ExitBlockIfNeeded();
            return;
        }

        float move = GetMoveInput();
        bool opponentOnRight = opponent.position.x > transform.position.x;

        bool holdingBack =
            (opponentOnRight && move < 0) ||
            (!opponentOnRight && move > 0);

        if (holdingBack)
            currentState = PlayerState.Blocking;
        else
            ExitBlockIfNeeded();
    }


    public bool IsBlocking()
    {
        return currentState == PlayerState.Blocking;
    }

    // ================= INPUT =================

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
            StartDash(move);
        else
        {
            lastDirection = move;
            lastTapTime = Time.time;
        }
    }

    // ================= ACTIONS =================

    private void Jump()
    {
        if (!isGrounded || currentState == PlayerState.Blocking) return;

        rb.velocity = new Vector2(GetMoveInput() * moveSpeed, jumpForce);
        animator.Play("PlayerJump", 0, 0f);
        isGrounded = false;
        currentState = PlayerState.Jumping;
    }

    private void Punch()
    {
        if (isAttacking || !isGrounded || currentState == PlayerState.Blocking) return;

        isAttacking = true;
        attackTime = punchDuration;
        currentState = PlayerState.Attacking;

        animator.Play("PlayerPunch", 0, 0f);
        punchHitbox.SetActive(true);
    }

    private void Kick()
    {
        if (isAttacking || !isGrounded || currentState == PlayerState.Blocking) return;

        isAttacking = true;
        attackTime = kickDuration;
        currentState = PlayerState.Attacking;

        animator.Play("PlayerKick", 0, 0f);
        kickHitbox.SetActive(true);
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
        if (currentState != PlayerState.Hit)
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
        Invoke(nameof(ExitHit), duration);
    }

    private void ExitHit()
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

        transform.localScale =
            transform.position.x < opponent.position.x
            ? Vector3.one
            : new Vector3(-1, 1, 1);
    }

    public bool IsAttacking()
    {
        return currentState == PlayerState.Attacking;
    }
    private void ExitBlockIfNeeded()
    {
        if (currentState == PlayerState.Blocking)
            currentState = PlayerState.Idle;
    }

}
