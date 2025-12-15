// // using UnityEngine;

// // public class DamageTester : MonoBehaviour
// // {
// //     public PlayerHealth playerHealth;

// //     void Update()
// //     {
// //         if (Input.GetKeyDown(KeyCode.H)) // press H to simulate hit
// //         {
// //             playerHealth.TakeDamage(10, Vector2.left);
// //         }
// //     }
// // }

using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public PlayerHealth playerHealth;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // press H to simulate hit
        {
            Vector2 testDirection = Vector2.left; // push player left
            playerHealth.TakeDamage(10, testDirection);
        }
    }
}
