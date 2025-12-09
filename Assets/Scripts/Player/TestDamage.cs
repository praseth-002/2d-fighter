using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public PlayerHealth playerHealth;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // press H to simulate hit
        {
            playerHealth.TakeDamage(10, Vector2.left);
        }
    }
}
