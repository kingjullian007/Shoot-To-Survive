using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    protected int maxHealth = 100;
    protected int currentHealth;
    [SerializeField] protected Slider healthSlider;

    public int MaxHealth { get => maxHealth; protected set => maxHealth = value; }
    public int CurrentHealth { get => currentHealth; protected set => currentHealth = value; }

    protected virtual void Start ()
    {
        InitializeHealth();
        UpdateHealthUI();
    }

    protected void InitializeHealth ()
    {
        CurrentHealth = MaxHealth;
    }

    public virtual void TakeDamage (int damage)
    {
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        UpdateHealthUI();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void UpdateHealthUI ()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)CurrentHealth / MaxHealth;
        }
    }

    protected virtual void Die ()
    {
        // Handle death logic in child classes
    }

    public void SetHealthSlider (Slider slider)
    {
        healthSlider = slider;
    }
}
