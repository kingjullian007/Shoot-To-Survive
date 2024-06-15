using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    protected float currentHealth;
    [SerializeField] private Slider healthBar;
    protected Transform playerTransform;
    private Transform healthBarCanvas;

    protected virtual void Start ()
    {
        currentHealth = maxHealth;
        playerTransform = Singleton.Instance.PlayerControllerInstance.transform;

        // Find the HealthBarCanvas child and cache its transform
        healthBarCanvas = healthBar.transform.parent;
        //UpdateHealthBar();
        //if (healthBar != null)
        //{
        //    healthBar.value = healthBar.maxValue;
        //}
    }

    protected virtual void Update ()
    {
        // Make the health bar face the camera
        if (healthBarCanvas != null)
        {
            healthBarCanvas.LookAt(Camera.main.transform);
        }

        // Common enemy logic
    }

    public void TakeDamage (float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar ()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    protected virtual void Die ()
    {
        // Drop coins
        Singleton.Instance.PoolManagerInstance.Spawn(SpawnObjectKey.Coins, transform.position, Quaternion.identity);
        Singleton.Instance.PoolManagerInstance.DeSpawn(gameObject);
    }

    // Method for specific attack behavior to be overridden by subclasses
    protected abstract void Attack ();
}
