using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private List<ObjectPoolItem> objectPoolItems;
    private Dictionary<SpawnObjectKey, Queue<GameObject>> objectPools;
    private Pool pool;
    public Pool poolInstance => pool;

    private void Awake ()
    {
        pool = new(objectPoolItems, objectPools);
    }
}
