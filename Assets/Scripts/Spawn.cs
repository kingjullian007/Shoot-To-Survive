using System.Collections.Generic;
using UnityEngine;

public class Spawn
{
    private int maxEnemies;
    private PoolManager poolManager;
    private List<Transform> spawnTransforms;
    private List<GameObject> activeEnemies;
    private HashSet<Vector3> occupiedPositions;
    private Transform playerTransform;

    public Spawn (int maxEnemies, PoolManager poolManager, List<Transform> spawnTransforms, Transform playerTransform)
    {
        this.maxEnemies = maxEnemies;
        this.poolManager = poolManager;
        this.spawnTransforms = spawnTransforms;
        this.playerTransform = playerTransform;
        activeEnemies = new List<GameObject>();
        occupiedPositions = new HashSet<Vector3>();
    }

    public void SpawnEnemies ()
    {
        var totalEnemies = activeEnemies.Count;

        // Limit the number of enemies on the ground at any time to maxEnemies
        if (totalEnemies >= maxEnemies)
        {
            return;
        }

        // Calculate how many enemies we can still spawn
        var enemiesToSpawn = Mathf.Min(5, maxEnemies - totalEnemies);

        for (var i = 0; i < enemiesToSpawn; i++)
        {
            var spawnPoint = GetValidSpawnPoint();
            if (spawnPoint == null)
            {
                // No valid spawn point found, stop spawning this cycle
                break;
            }

            // Randomly decide the type of enemy to spawn
            var enemyType = ( Random.value < 0.5f ) ? SpawnObjectKey.Enemy_Aggressive : SpawnObjectKey.Enemy_Fixed;

            var enemy = poolManager.poolInstance.Spawn(enemyType, spawnPoint.position, spawnPoint.rotation);
            if (enemy != null)
            {
                InitializeEnemy(enemy); // Initialize the enemy's health and state
                activeEnemies.Add(enemy);
                occupiedPositions.Add(spawnPoint.position);

                // Ensure we track the enemy movement
                var enemyMovement = enemy.GetComponent<EnemyMovement>();
                if (enemyMovement == null)
                {
                    enemyMovement = enemy.AddComponent<EnemyMovement>();
                }

                enemyMovement.OnPositionChanged += HandleEnemyPositionChanged;
            }
        }
    }

    private Transform GetValidSpawnPoint ()
    {
        foreach (var spawnPoint in spawnTransforms)
        {
            if (!occupiedPositions.Contains(spawnPoint.position) && Vector3.Distance(spawnPoint.position, playerTransform.position) >= 5f)
            {
                return spawnPoint;
            }
        }
        return null;
    }

    public void RemoveEnemy (GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            poolManager.poolInstance.DeSpawn(enemy);

            // Unsubscribe from position change events if applicable
            var enemyMovement = enemy.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.OnPositionChanged -= HandleEnemyPositionChanged;
            }

            // Ensure occupied positions are updated when enemies are removed
            if (occupiedPositions.Contains(enemy.transform.position))
            {
                occupiedPositions.Remove(enemy.transform.position);
            }

            // Debug log to track enemy removal
            Debug.Log($"Removed {enemy.name}, Active Enemies: {activeEnemies.Count}");
        }
    }

    private void HandleEnemyPositionChanged (GameObject enemy, Vector3 oldPosition, Vector3 newPosition)
    {
        if (occupiedPositions.Contains(oldPosition))
        {
            occupiedPositions.Remove(oldPosition);
        }
    }

    private void InitializeEnemy (GameObject enemy)
    {
        var enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.InitializeEnemy();
        }
    }
}
