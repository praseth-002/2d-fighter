using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Slider healthSlider;

    void Start()
    {
        healthSlider.maxValue = playerHealth.maxHealth;
        healthSlider.value = playerHealth.currentHealth;
    }

    void Update()
    {
        healthSlider.value = playerHealth.currentHealth;
    }
}
