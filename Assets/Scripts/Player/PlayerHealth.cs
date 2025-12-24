using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Damage")]
    public float blockDamageMultiplier = 0.2f;

    [Header("Pushback")]
    public float hitPushbackForce = 6f;
    public float blockPushbackForce = 3f;

    [Header("Stun")]
    public float hitStunDuration = 0.4f;
    public float blockStunDuration = 0.2f;

    [Header("Hitstop")]
    public float hitStopDuration = 0.06f;
    public float blockHitStopDuration = 0.04f;

    private PlayerMovement movement;
    private Animator animator;
    private Rigidbody2D rb;

    private bool isDead = false;
    private PlayerSound playerSound;


    private void Awake()
    {
        currentHealth = maxHealth;
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerSound = GetComponent<PlayerSound>();

    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if (isDead) return;

        bool blocked = movement.IsBlocking();

        if (blocked)
        {
            int reduced = Mathf.RoundToInt(damage * blockDamageMultiplier);
            currentHealth -= reduced;

            animator.Play("PlayerBlock", 0, 0f);
            movement.EnterHitState(blockStunDuration);

            ApplyPushback(hitDirection, blockPushbackForce);
            HitStopManager.Instance.DoHitStop(blockHitStopDuration);
        }
        else
        {
            currentHealth -= damage;

            animator.Play("PlayerDamaged", 0, 0f);
            movement.EnterHitState(hitStunDuration);

            ApplyPushback(hitDirection, hitPushbackForce);
            HitStopManager.Instance.DoHitStop(hitStopDuration);
            playerSound.PlaySound(PlayerSound.SoundType.Hurt);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void ApplyPushback(Vector2 direction, float force)
    {
        direction.y = 0;
        direction.Normalize();

        rb.velocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        playerSound.PlaySound(PlayerSound.SoundType.Death);
        animator.Play("PlayerDeath", 0, 0f);
        movement.EnterDeadState();

        RoundManager.Instance.OnPlayerDeath(this);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    public void ResetState()
    {
        // Let movement recover cleanly
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
            movement.EnterHitState(0f);
    }

}
