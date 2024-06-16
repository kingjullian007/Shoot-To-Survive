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
        // Caching the transform for optimization purpose
        playerTransform = transform;
        defenseZone = GetComponentInChildren<DefenseZone>();
    }

    private void Update ()
    {
        // Get input from the joystick
        var horizontalInput = joystick.Horizontal();
        var verticalInput = joystick.Vertical();

        // Calculate the movement direction based on input
        var moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Check if there is any movement input
        if (moveDirection != Vector3.zero)
        {
            // Rotate the player to face the movement direction
            playerTransform.rotation = Quaternion.LookRotation(moveDirection);
        }

        // Apply movement
        playerTransform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);

        // Shooting mechanism
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
        if (enemies.Count() > 0)
        {
            if (Time.time > shootStartTime + shootingInterval)
            {
                // Find the closest enemy
                var closestEnemy = enemies.OrderBy(e => Vector3.Distance(playerTransform.position, e.transform.position)).FirstOrDefault();

                if (closestEnemy != null)
                {
                    // Calculate the direction to the closest enemy
                    var directionToEnemy = ( closestEnemy.transform.position - bulletSpawnPoint.position ).normalized;

                    // Spawn the bullet and set its direction
                    var bullet = Singleton.Instance.PoolManagerInstance.Spawn(SpawnObjectKey.Bullet_Player, bulletSpawnPoint.position, Quaternion.LookRotation(directionToEnemy));
                    Debug.Log("Boom");

                    // Optionally, you can adjust the bullet's forward direction
                    bullet.transform.forward = directionToEnemy;
                }

                shootStartTime = Time.time;
            }
        }
    }
}
