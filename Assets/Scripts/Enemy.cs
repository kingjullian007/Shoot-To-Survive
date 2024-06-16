using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    protected float currentHealth;
    [SerializeField] private Slider healthBar;
    protected Transform playerTransform;
    private Transform healthBarCanvas;
    public bool isDead = false; // Changed access to protected

    protected virtual void Start ()
    {
        currentHealth = maxHealth;
        playerTransform = Singleton.Instance.PlayerControllerInstance.transform;

        // Find the HealthBarCanvas child and cache its transform
        healthBarCanvas = healthBar.transform.parent;
        UpdateHealthBar(); // Initialize health bar
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
        if (!isDead) // Only take damage if not already dead
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            UpdateHealthBar();
            //Debug.Log("I am Enemy & my currentHealth: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void UpdateHealthBar ()
    {
        if (healthBar != null)
        {
            var healthPercentage = currentHealth / maxHealth * 100f;
            healthBar.value = Mathf.Clamp(healthPercentage, 0f, maxHealth);
        }
    }

    protected virtual void Die ()
    {
        isDead = true; // Mark as dead
        // Drop coins
        Singleton.Instance.PoolManagerInstance.Spawn(SpawnObjectKey.Coins, transform.position, Quaternion.identity);

        // Notify the SpawnManager to remove the enemy
        SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager != null)
        {
            spawnManager.RemoveEnemy(gameObject);
        }
        
        // DeSpawn the enemy
        Singleton.Instance.PoolManagerInstance.DeSpawn(gameObject);
    }

    // Method for specific attack behavior to be overridden by subclasses
    protected abstract void Attack ();
}
