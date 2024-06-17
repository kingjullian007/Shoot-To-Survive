using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private List<ObjectPoolItem> objectPoolItems;
    private Dictionary<SpawnObjectKey, Queue<GameObject>> objectPools;
    public Pool(List<ObjectPoolItem> objectPoolItems, Dictionary<SpawnObjectKey, Queue<GameObject>> objectPools)
    { 
        this.objectPoolItems = objectPoolItems;
        this.objectPools = objectPools;
        InitializeObjectPools();
    }
    private void InitializeObjectPools ()
    {
        objectPools = new Dictionary<SpawnObjectKey, Queue<GameObject>>();

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

            for (var i = 0; i < poolSize; i++)
            {
                var newObj = Object.Instantiate(prefab);
                newObj.SetActive(false);
                objectPools[key].Enqueue(newObj);
            }
        }
    }

    public GameObject Spawn (SpawnObjectKey key, Vector3 position, Quaternion rotation)
    {
        if (objectPools.ContainsKey(key))
        {
            GameObject objToSpawn;

            if (objectPools[key].Count == 0)
            {
                objToSpawn = Object.Instantiate(objectPoolItems.Find(item => item.key == key).prefab, position, rotation);
            }
            else
            {
                objToSpawn = objectPools[key].Dequeue();
                objToSpawn.transform.position = position;
                objToSpawn.transform.rotation = rotation;
                objToSpawn.SetActive(true);
            }

            if (objToSpawn.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.InitializeEnemy();
            }

            return objToSpawn;
        }
        else
        {
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
    }
}
