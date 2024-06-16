using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Joystick joystick;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float shootingInterval = 0.5f;
    private float shootStartTime;
    private Transform playerTransform;
    private DefenseZone defenseZone;

    private void Start ()
    {
        playerTransform = transform;
        defenseZone = GetComponentInChildren<DefenseZone>();
    }

    private void Update ()
    {
        if (Singleton.Instance.GameManagerInstance.CurrentState!= GameState.GamePlay)
        {
            return;
        }
        var horizontalInput = joystick.Horizontal();
        var verticalInput = joystick.Vertical();

        var moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (moveDirection != Vector3.zero)
        {
            playerTransform.rotation = Quaternion.LookRotation(moveDirection);
        }

        playerTransform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);

        ScanAndShoot();
    }

    private void ScanAndShoot ()
    {
        if (defenseZone == null)
        {
            Debug.Log("Defense zone not detected! Try adding one first to the player");
            return;
        }

        var enemies = defenseZone.GetEnemiesInZone();

        // Remove dead enemies from the list
        enemies.RemoveAll(enemy => enemy == null || enemy.GetComponent<Enemy>().isDead); // Updated here

        if (enemies.Count > 0)
        {
            // Find the closest enemy
            var closestEnemy = enemies.OrderBy(e => Vector3.Distance(playerTransform.position, e.transform.position)).FirstOrDefault();

            if (closestEnemy != null)
            {
                // Calculate the direction to the closest enemy
                var directionToEnemy = ( closestEnemy.transform.position - bulletSpawnPoint.position ).normalized;

                // Smoothly rotate the player towards the closest enemy
                var targetRotation = Quaternion.LookRotation(directionToEnemy);
                playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * 5f);

                if (Time.time > shootStartTime + shootingInterval)
                {
                    // Spawn the bullet and set its direction
                    var bullet = Singleton.Instance.PoolManagerInstance.Spawn(SpawnObjectKey.Bullet_Player, bulletSpawnPoint.position, Quaternion.LookRotation(directionToEnemy));

                    // Optionally, you can adjust the bullet's forward direction
                    bullet.transform.forward = directionToEnemy;

                    shootStartTime = Time.time;
                }
            }
        }
    }
}
