using System.Collections.Generic;
using UnityEngine;

public enum SpawnObjectKey
{
    Bullet_Player,
    Bullet_Enemy,
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
    [SerializeField] private List<ObjectPoolItem> objectPoolItems;
    private Dictionary<SpawnObjectKey, Queue<GameObject>> objectPools;

    private void Awake ()
    {
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
            GameObject objToSpawn;

            if (objectPools[key].Count == 0)
            {
                objToSpawn = Instantiate(objectPoolItems.Find(item => item.key == key).prefab, position, rotation);
            }
            else
            {
                objToSpawn = objectPools[key].Dequeue();
                objToSpawn.transform.position = position;
                objToSpawn.transform.rotation = rotation;
                objToSpawn.SetActive(true);
            }

            var enemy = objToSpawn.GetComponent<Enemy>();
            if (enemy != null)
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
