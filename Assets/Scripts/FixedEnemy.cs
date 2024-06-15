using UnityEngine;

public class FixedEnemy : Enemy
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float shootingInterval = 0.2f;
    private float shootStartTime;
    [SerializeField] private float rotationSpeed = 2f; // Speed at which the enemy rotates towards the player

    protected override void Update ()
    {
        base.Update();
        RotateTowardsPlayer();
        if (Time.time > shootStartTime + shootingInterval)
        {
            shootStartTime = Time.time;
            Attack();
        }
    }

    private void RotateTowardsPlayer ()
    {
        if (playerTransform != null)
        {
            var direction = ( playerTransform.position - transform.position ).normalized;
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    protected override void Attack ()
    {
        var bullet = Singleton.Instance.PoolManagerInstance.Spawn(SpawnObjectKey.Bullets, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        // Implement bullet movement logic here
    }
}
