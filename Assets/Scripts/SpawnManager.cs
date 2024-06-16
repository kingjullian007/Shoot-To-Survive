using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnTransforms = new List<Transform>();
    [SerializeField] private float spawnInterval = 5f;
    private float lastSpawnTime;
    private int maxEnemies = 30; // Maximum number of enemies on the ground at any time
   
    private Transform playerTransform;
    private PoolManager poolManager;
    private Spawn spawnInstance;
    public Spawn SpawnInstance => spawnInstance;

    private void Start ()
    {
        playerTransform = Singleton.Instance.PlayerControllerInstance.transform;
        poolManager = Singleton.Instance.PoolManagerInstance;
        spawnInstance = new(maxEnemies ,poolManager, spawnTransforms, playerTransform);
    }

    private void Update ()
    {
        if (Singleton.Instance.GameManagerInstance.CurrentState != GameState.GamePlay)
        {
            return;
        }

        if (Time.time > lastSpawnTime + spawnInterval)
        {
            lastSpawnTime = Time.time;
            spawnInstance.SpawnEnemies();
        }
    }
}
