using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public SpawnObjectKey key;
    public GameObject prefab;
    public int poolSize;
}
