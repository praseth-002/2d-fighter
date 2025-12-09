// using UnityEngine;

// public class Hitbox : MonoBehaviour
// {
//     public int damage = 10; // default value

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         // Hurtbox logic is handled on the other script now
//         // So this can be empty unless debugging
//         Debug.Log("Hitbox " + name + " collided with " + other.name);
//     }
// }

using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hitbox " + name + " collided with " + other.name);
    }
}
