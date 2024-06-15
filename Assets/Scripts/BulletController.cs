using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 25f;
    [SerializeField] private float damage = 25f;
    public float Damage => damage;

    private Transform bulletTransform;

    private void Start ()
    {
        bulletTransform = GetComponent<Transform>();
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
            Singleton.Instance.PoolManagerInstance.DeSpawn(gameObject);
        }
        else if (other.CompareTag("DeadEnd"))
        {
            Singleton.Instance.PoolManagerInstance.DeSpawn(gameObject);
        }
    }
}
