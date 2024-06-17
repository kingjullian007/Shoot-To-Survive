using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    protected float currentHealth;
    [SerializeField] private Slider healthBar;
    protected Transform playerTransform;
    private Transform healthBarCanvas;
    public bool isDead = false;

    protected virtual void Start ()
    {
        playerTransform = Singleton.Instance.PlayerControllerInstance.transform;
        healthBarCanvas = healthBar.transform.parent;
        InitializeEnemy();
    }

    protected virtual void Update ()
    {
        if (healthBarCanvas != null)
        {
            healthBarCanvas.LookAt(Camera.main.transform);
        }
    }

    public void InitializeEnemy ()
    {
        currentHealth = maxHealth;
        isDead = false;
        UpdateHealthBar();
    }

    public void TakeDamage (float amount)
    {
        if (!isDead)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            UpdateHealthBar();

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
        isDead = true;
        Singleton.Instance.PoolManagerInstance.Spawn(SpawnObjectKey.Coins, transform.position, Quaternion.identity);

        var spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager != null)
        {
            spawnManager.SpawnInstance.RemoveEnemy(gameObject);
        }
        GameEvents.EnemyDied?.Invoke();
        Singleton.Instance.PoolManagerInstance.DeSpawn(gameObject);
    }

    protected virtual void Attack () { }
}
