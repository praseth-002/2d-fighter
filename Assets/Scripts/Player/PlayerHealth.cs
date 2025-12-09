using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Vector2 hitKnockback = new Vector2(5f, 2f); // x = horizontal, y = vertical
    public float hitStunDuration = 0.3f;

    private Animator animator;
    private Rigidbody2D rb;
    private PlayerMovement movement; // reference to disable temporarily

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(int amount, Vector2 attackDirection)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        Debug.Log(gameObject.name + " took " + amount + " damage! HP: " + currentHealth);

        // Play hit animation
        if (animator != null)
            animator.Play("PlayerDamaged", 0, 0f);

        // Apply knockback
        if (rb != null)
            rb.velocity = new Vector2(attackDirection.x * hitKnockback.x, hitKnockback.y);

        // Temporarily disable movement
        if (movement != null)
            StartCoroutine(HitStunCoroutine());

        if (currentHealth <= 0)
            Die();
    }

    private System.Collections.IEnumerator HitStunCoroutine()
    {
        movement.enabled = false;
        yield return new WaitForSeconds(hitStunDuration);
        movement.enabled = true;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " DIED!");
        // TODO: play death animation / end match
        movement.enabled = false;
        rb.velocity = Vector2.zero;
    }
}
