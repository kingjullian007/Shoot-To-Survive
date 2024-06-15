using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    protected float currentHealth;
    [SerializeField] private Slider healthBar;
    protected Transform playerTransform;
    private Transform healthBarCanvas;
    private DefenseZone defenseZone;

    protected virtual void Start ()
    {
        currentHealth = maxHealth;
        playerTransform = Singleton.Instance.PlayerControllerInstance.transform;

        // Find the HealthBarCanvas child and cache its transform
        healthBarCanvas = healthBar.transform.parent;

        // Find the DefenseZone component in the player's hierarchy
        defenseZone = Singleton.Instance.PlayerControllerInstance.GetComponentInChildren<DefenseZone>();
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
        Debug.Log("I am Enemy & my currentHealth: " + currentHealth);
    }

    private void UpdateHealthBar ()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
            Mathf.Clamp(healthBar.value, 0f, 1f);
        }
    }

    protected virtual void Die ()
    {
        // Drop coins
        Singleton.Instance.PoolManagerInstance.Spawn(SpawnObjectKey.Coins, transform.position, Quaternion.identity);

        // Remove from the defense zone list
        if (defenseZone != null)
        {
            defenseZone.RemoveEnemyFromZone(gameObject);
        }

        Singleton.Instance.PoolManagerInstance.DeSpawn(gameObject);
    }

    // Method for specific attack behavior to be overridden by subclasses
    protected abstract void Attack ();
}
