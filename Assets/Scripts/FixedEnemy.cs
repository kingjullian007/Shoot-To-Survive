using UnityEngine;

public class FixedEnemy : Enemy
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float shootingInterval = 0.2f;
    private float shootStartTime;
    [SerializeField] private float rotationSpeed = 2f; // Speed at which the enemy rotates towards the player
    [SerializeField] private float attackRange = 15f; // Maximum range for attacking


    protected override void Start ()
    {
        base.Start();
    }
    protected override void Update ()
    {
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
        var bullet = Singleton.Instance.PoolManagerInstance.Spawn(SpawnObjectKey.Bullet_Enemy, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        // Implement bullet movement logic here
    }
}
