using UnityEngine;

public class BulletController : MonoBehaviour, IPoolable
{
    private Transform bulletTransform;
    private float speed = 25f;
    private float damage = 25f;

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
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
            DeSpawn();
        }
        else if (other.CompareTag("DeadEnd"))
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
