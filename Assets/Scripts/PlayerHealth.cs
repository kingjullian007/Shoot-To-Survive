using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private Slider healthSlider;

    public int MaxHealth { get => maxHealth; private set => maxHealth = value; }
    public int CurrentHealth { get => currentHealth; private set => currentHealth = value; }

    private void Start ()
    {
        InitializeHealth();
        UpdateHealthUI();
    }

    private void InitializeHealth ()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage (int damage)
    {
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        UpdateHealthUI();

        if (CurrentHealth <= 0)
        {
            Die();
        }
        Debug.Log("I am Player & my currentHealth: " + currentHealth);

    }

    private void UpdateHealthUI ()
    {
        if (healthSlider != null)
        {
            // Calculate normalized value for the slider
            float normalizedHealth = (float)CurrentHealth / MaxHealth;
            healthSlider.value = normalizedHealth;
        }
    }

    private void Die ()
    {
        // Handle death logic
        Debug.Log("Player died!");
        // Add game over logic or respawn logic here
    }

    public void SetHealthSlider (Slider slider)
    {
        healthSlider = slider;
    }
}
