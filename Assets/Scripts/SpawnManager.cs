using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnTransforms = new();
    [SerializeField] private float spawnInterval = 5f; // Time interval between spawns
    private float lastSpawnTime;
    private int maxEnemies = 20;
    private int aggressiveEnemyCount = 0;
    private int fixedEnemyCount = 0;
    private List<GameObject> activeEnemies = new();

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
        // Calculate the current total enemy count
        int totalEnemies = aggressiveEnemyCount + fixedEnemyCount;

        // Check if we can spawn more enemies
        if (totalEnemies >= maxEnemies)
        {
            return;
        }

        // Determine the number of enemies to spawn
        int enemiesToSpawn = Mathf.Min(5, maxEnemies - totalEnemies);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Randomly select a spawn point
            int spawnIndex = Random.Range(0, spawnTransforms.Count);
            Transform spawnPoint = spawnTransforms[spawnIndex];

            // Decide whether to spawn an aggressive or fixed enemy
            SpawnObjectKey enemyType = ( aggressiveEnemyCount <= fixedEnemyCount ) ? SpawnObjectKey.Enemy_Aggressive : SpawnObjectKey.Enemy_Fixed;

            // Spawn the enemy
            GameObject enemy = PoolManager.Instance.Spawn(enemyType, spawnPoint.position, spawnPoint.rotation);
            if (enemy != null)
            {
                activeEnemies.Add(enemy);
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
            PoolManager.Instance.DeSpawn(enemy);
            if (enemy.CompareTag("EnemyAggressive"))
            {
                aggressiveEnemyCount--;
            }
            else if (enemy.CompareTag("EnemyFixed"))
            {
                fixedEnemyCount--;
            }
        }
    }
}
