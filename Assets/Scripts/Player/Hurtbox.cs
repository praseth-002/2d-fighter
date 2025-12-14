using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    private PlayerHealth health;

    private void Awake()
    {
        health = GetComponentInParent<PlayerHealth>();
    }

    // Called ONLY by Hitbox
    public void ReceiveHit(int damage, Vector2 direction)
    {
        health.TakeDamage(damage, direction);
    }
}
