using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnTransforms = new List<Transform>();
    [SerializeField] private float spawnInterval = 2f;
    private float lastSpawnTime;
    private int maxEnemies = 30;
    private int aggressiveEnemyCount = 0;
    private int fixedEnemyCount = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>(); // Track occupied spawn positions
    private Transform playerTransform;
    private PoolManager poolManager;

    private void Start ()
    {
        playerTransform = Singleton.Instance.PlayerControllerInstance.transform;
        poolManager = Singleton.Instance.PoolManagerInstance;
    }

    private void Update ()
    {
        if (Time.time > lastSpawnTime + spawnInterval)
        {
            lastSpawnTime = Time.time;
            SpawnEnemies();
        }
    }

    private void SpawnEnemies ()
    {
        var totalEnemies = aggressiveEnemyCount + fixedEnemyCount;

        if (totalEnemies >= maxEnemies)
        {
            return;
        }

        var enemiesToSpawn = Mathf.Min(5, maxEnemies - totalEnemies);
        for (var i = 0; i < enemiesToSpawn; i++)
        {
            var spawnIndex = Random.Range(0, spawnTransforms.Count);
            var spawnPoint = spawnTransforms[spawnIndex];

            if (occupiedPositions.Contains(spawnPoint.position) || Vector3.Distance(spawnPoint.position, playerTransform.position) < 5f)
            {
                continue; 
            }

            var enemyType = ( aggressiveEnemyCount <= fixedEnemyCount ) ? SpawnObjectKey.Enemy_Aggressive : SpawnObjectKey.Enemy_Fixed;

            // Spawn the enemy
            var enemy = poolManager.Spawn(enemyType, spawnPoint.position, spawnPoint.rotation);
            if (enemy != null)
            {
                activeEnemies.Add(enemy);
                occupiedPositions.Add(spawnPoint.position);
                if (enemyType == SpawnObjectKey.Enemy_Aggressive)
                {
                    aggressiveEnemyCount++;
                }
                else
                {
                    fixedEnemyCount++;
                }
            }
        }
    }

    public void RemoveEnemy (GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            poolManager.DeSpawn(enemy);
            if (enemy.CompareTag("EnemyAggressive"))
            {
                aggressiveEnemyCount--;
            }
            else if (enemy.CompareTag("EnemyFixed"))
            {
                fixedEnemyCount--;
            }
            occupiedPositions.Remove(enemy.transform.position);
        }
    }
}
