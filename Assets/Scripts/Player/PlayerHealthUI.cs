using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Slider player1HealthBar;
    public Slider player2HealthBar;

    private PlayerHealth player1Health;
    private PlayerHealth player2Health;

    void Update()
    {
        if (player1Health == null || player2Health == null)
            return;

        player1HealthBar.value = player1Health.currentHealth;
        player2HealthBar.value = player2Health.currentHealth;
    }

    public void BindPlayers(GameObject p1, GameObject p2)
    {
        player1Health = p1.GetComponent<PlayerHealth>();
        player2Health = p2.GetComponent<PlayerHealth>();

        player1HealthBar.maxValue = player1Health.maxHealth;
        player2HealthBar.maxValue = player2Health.maxHealth;
    }
}
