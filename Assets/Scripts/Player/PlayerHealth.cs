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

    private PlayerMovement movement;
    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        currentHealth = maxHealth;
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        // bool blocked = movement.IsBlocking();
        bool blocked = movement.IsBlocking();


        if (blocked)
        {
            int reducedDamage = Mathf.RoundToInt(damage * blockDamageMultiplier);
            currentHealth -= reducedDamage;

            animator.Play("PlayerBlock", 0, 0f);
            movement.EnterHitState(blockStunDuration);

            ApplyPushback(hitDirection, blockPushbackForce);
        }
        else
        {
            currentHealth -= damage;

            animator.Play("PlayerDamaged", 0, 0f);
            movement.EnterHitState(hitStunDuration);

            ApplyPushback(hitDirection, hitPushbackForce);
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
        animator.Play("PlayerDeath", 0, 0f);
        movement.EnterDeadState();
        RoundManager.Instance.PlayerDied(this);
    }
}
