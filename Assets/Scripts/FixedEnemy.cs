using UnityEngine;

public class FixedEnemy : Enemy
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float shootingInterval = 0.2f;
    [SerializeField] private float rotationSpeed = 2f; 
    [SerializeField] private float attackRange = 15f;
    private float shootStartTime;

    protected override void Start ()
    {
        base.Start();
    }
    protected override void Update ()
    {
        if (Singleton.Instance.GameManagerInstance.CurrentState != GameState.GamePlay)
        {
            return;
        }

        base.Update();
        if (!IsPlayerInRange())
        {
            return;
        }
        RotateTowardsPlayer();
        if (Time.time > shootStartTime + shootingInterval)
        {
            shootStartTime = Time.time;
            Attack();
        }
    }

    private bool IsPlayerInRange ()
    {
        if (playerTransform != null)
        {
            var distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            return distanceToPlayer <= attackRange;
        }
        return false;
    }

    private void RotateTowardsPlayer ()
    {
        var direction = ( playerTransform.position - transform.position ).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    protected override void Attack ()
    {
        Singleton.Instance.PoolManagerInstance.poolInstance.Spawn(SpawnObjectKey.Bullet_Enemy, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }
}
