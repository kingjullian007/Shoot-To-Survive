using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 25f;
    [SerializeField] private float damage = 25f;
    public float Damage => damage;

    private Transform bulletTransform;

    private Pool pool;

    private void Start ()
    {
        bulletTransform = GetComponent<Transform>();
        pool = Singleton.Instance.PoolManagerInstance.poolInstance;
    }

    private void Update ()
    {
        bulletTransform.Translate(speed * Time.deltaTime * bulletTransform.forward, Space.World);
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("EnemyAggressive") || other.CompareTag("EnemyFixed"))
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            pool.DeSpawn(gameObject);
        }
       
        else if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            pool.DeSpawn(gameObject);
        }

        else if (other.CompareTag("DeadEnd"))
        {
            pool.DeSpawn(gameObject);
        }

    }
}
