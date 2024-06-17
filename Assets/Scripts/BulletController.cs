using UnityEngine;

public class BulletController : MonoBehaviour, IPoolable
{
    [SerializeField] private float speed = 25f;
    [SerializeField]  private float damage = 25f;
    private Transform bulletTransform;

    public float Damage => damage;

    private void Start ()
    {
        bulletTransform = transform;
    }

    private void Update ()
    {
        MoveBullet();
    }

    private void MoveBullet ()
    {
        // Move the bullet along its own forward direction
        bulletTransform.Translate(speed * Time.deltaTime * bulletTransform.forward, Space.World);
    }

    private void OnTriggerEnter (Collider other)
    {
        HandleCollision(other);
    }

    private void HandleCollision (Collider other)
    {
        if (other.CompareTag("EnemyAggressive") || other.CompareTag("EnemyFixed") || other.CompareTag("Player"))
        {
            var damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                DeSpawn();
            }
        }

        if (other.CompareTag("DeadEnd"))
        {
            DeSpawn();
        }
    }

    public void DeSpawn ()
    {
        var pool = Singleton.Instance.PoolManagerInstance.poolInstance;
        pool.DeSpawn(gameObject);
    }
}

