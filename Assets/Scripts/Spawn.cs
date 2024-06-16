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

    public Spawn(int maxEnemies, PoolManager poolManager, List<Transform> spawnTransforms, Transform playerTransform)
    {
        this.maxEnemies = maxEnemies;
        this.poolManager = poolManager;
        this.spawnTransforms = spawnTransforms;
        this.playerTransform = playerTransform;
        activeEnemies = new();
        occupiedPositions = new();
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

            // Determine the ratio of aggressive to fixed enemies
            var aggressiveEnemyCount = activeEnemies.FindAll(e => e.CompareTag("EnemyAggressive")).Count;
            var fixedEnemyCount = activeEnemies.FindAll(e => e.CompareTag("EnemyFixed")).Count;
            var aggressiveEnemyRatio = (float)aggressiveEnemyCount / maxEnemies;
            var fixedEnemyRatio = (float)fixedEnemyCount / maxEnemies;

            // Decide the type of enemy to spawn based on the ratios
            SpawnObjectKey enemyType;
            if (aggressiveEnemyRatio < 0.7f && fixedEnemyRatio < 0.3f)
            {
                enemyType = ( Random.value < 0.7f ) ? SpawnObjectKey.Enemy_Aggressive : SpawnObjectKey.Enemy_Fixed;
            }
            else if (aggressiveEnemyRatio >= 0.7f)
            {
                enemyType = SpawnObjectKey.Enemy_Fixed;
            }
            else
            {
                enemyType = SpawnObjectKey.Enemy_Aggressive;
            }

            var enemy = poolManager.Spawn(enemyType, spawnPoint.position, spawnPoint.rotation);
            if (enemy != null)
            {
                activeEnemies.Add(enemy);
                occupiedPositions.Add(spawnPoint.position);
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
            poolManager.DeSpawn(enemy);
            occupiedPositions.Remove(enemy.transform.position);

            // Debug log to track enemy removal
            Debug.Log($"Removed {enemy.name}, Active Enemies: {activeEnemies.Count}");
        }
    }
}
