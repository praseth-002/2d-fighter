using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    private PlayerHealth health;

    private void Awake()
    {
        health = GetComponentInParent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    Hitbox hitbox = other.GetComponent<Hitbox>();

    if (hitbox != null)
    {
        Vector2 direction = (transform.position - other.transform.position).normalized;
        health.TakeDamage(hitbox.damage, direction);
        Debug.Log("HURTBOX: Hit by " + other.name + " for " + hitbox.damage);
    }
}
}


