using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour, IDamageable, IPoolable
{
    [SerializeField] protected float maxHealth = 100f;
    protected float currentHealth;
    [SerializeField] private Slider healthBar;
    protected Transform playerTransform;
    private Transform healthBarCanvas;
    public bool isDead = false;
    private Pool pool;

    protected virtual void Start ()
    {
        playerTransform = Singleton.Instance.PlayerControllerInstance.transform;
        healthBarCanvas = healthBar.transform.parent;
        pool = Singleton.Instance.PoolManagerInstance.poolInstance;
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
        pool.Spawn(SpawnObjectKey.Coins, transform.position, Quaternion.identity);

        var spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager != null)
        {
            spawnManager.SpawnInstance.RemoveEnemy(gameObject);
        }
        GameEvents.EnemyDied?.Invoke();
        //pool.DeSpawn(gameObject);
        DeSpawn();
    }

    protected virtual void Attack () { }

    public void DeSpawn ()
    {
        pool.DeSpawn(gameObject);
    }
}
