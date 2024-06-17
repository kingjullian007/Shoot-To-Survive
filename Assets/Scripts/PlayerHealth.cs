using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private Slider healthSlider;
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; private set => currentHealth = value; }

    private float currentHealth;

    private void Start ()
    {
        InitializeHealth();
        UpdateHealthUI();
    }

    private void InitializeHealth ()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage (float damage)
    {
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        UpdateHealthUI();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI ()
    {
        if (healthSlider != null)
        {
            var healthPercentage = currentHealth / maxHealth * 100f;
            healthSlider.value = Mathf.Clamp(healthPercentage, 0f, maxHealth);
        }
    }

    private void Die ()
    {
        GameEvents.GameOver?.Invoke();
    }

    public void SetHealthSlider (Slider slider)
    {
        healthSlider = slider;
    }
}
