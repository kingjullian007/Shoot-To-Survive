using System.Collections.Generic;
using UnityEngine;

public enum SpawnObjectKey
{
    Bullets,
    Enemy_Aggressive,
    Enemy_Fixed,
    Coins
}

[System.Serializable]
public class ObjectPoolItem
{
    public SpawnObjectKey key;
    public GameObject prefab;
    public int poolSize;
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance; // Singleton instance

    [SerializeField] private List<ObjectPoolItem> objectPoolItems;
    private Dictionary<SpawnObjectKey, Queue<GameObject>> objectPools;

    private void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeObjectPools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeObjectPools ()
    {
        objectPools = new Dictionary<SpawnObjectKey, Queue<GameObject>>();

        // Create object pools for each item in the list
        foreach (var item in objectPoolItems)
        {
            CreateObjectPool(item.key, item.prefab, item.poolSize);
        }
    }

    private void CreateObjectPool (SpawnObjectKey key, GameObject prefab, int poolSize)
    {
        if (!objectPools.ContainsKey(key))
        {
            objectPools[key] = new Queue<GameObject>();

            // Populate the object pool with instances of the prefab
            for (var i = 0; i < poolSize; i++)
            {
                var newObj = Instantiate(prefab);
                newObj.SetActive(false);
                objectPools[key].Enqueue(newObj);
            }
        }
    }

    public GameObject Spawn (SpawnObjectKey key, Vector3 position, Quaternion rotation)
    {
        if (objectPools.ContainsKey(key))
        {
            if (objectPools[key].Count == 0)
            {
                // If the pool is empty, create a new instance
                Debug.LogWarning("Object pool for " + key + " is empty. Creating a new instance.");
                var newObj = Instantiate(objectPoolItems.Find(item => item.key == key).prefab, position, rotation);
                return newObj;
            }
            else
            {
                // Reuse an object from the pool
                var objToSpawn = objectPools[key].Dequeue();
                objToSpawn.transform.position = position;
                objToSpawn.transform.rotation = rotation;
                objToSpawn.SetActive(true);
                return objToSpawn;
            }
        }
        else
        {
            Debug.LogWarning("Object pool for " + key + " not found!");
            return null;
        }
    }

    public void DeSpawn (GameObject obj)
    {
        obj.SetActive(false);
        var poolKey = obj.name.Replace("(Clone)", "");
        foreach (var item in objectPoolItems)
        {
            if (item.prefab.name == poolKey)
            {
                objectPools[item.key].Enqueue(obj);
                return;
            }
        }
        Debug.LogWarning("Object pool for " + poolKey + " not found!");
    }
}
