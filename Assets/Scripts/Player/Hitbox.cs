// using UnityEngine;

// public class Hitbox : MonoBehaviour
// {
//     public int damage = 10; // set in inspector

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Hurtbox"))
//         {
//             Debug.Log("Hit landed!");
//             // later we call enemy.TakeDamage(damage);
//         }
//     }
// }


using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 10; // default value

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Hurtbox logic is handled on the other script now
        // So this can be empty unless debugging
        Debug.Log("Hitbox " + name + " collided with " + other.name);
    }
}
