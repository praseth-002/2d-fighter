using UnityEngine;
using System.Collections.Generic;

public class Hitbox : MonoBehaviour
{
    public int damage = 10;

    // Keeps track of which hurtboxes were already hit this activation
    private HashSet<Hurtbox> hitTargets = new HashSet<Hurtbox>();

    private void OnEnable()
    {
        // Reset every time the hitbox is activated
        hitTargets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Hurtbox hurtbox = other.GetComponent<Hurtbox>();
        if (!hurtbox) return;

        // Already hit this target during this activation
        if (hitTargets.Contains(hurtbox))
            return;

        hitTargets.Add(hurtbox);

        Vector2 direction = (hurtbox.transform.position - transform.position).normalized;
        hurtbox.ReceiveHit(damage, direction);
    }
}
