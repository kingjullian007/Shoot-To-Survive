
using UnityEngine;

public class CoinController : MonoBehaviour, IPoolable
{
    public void DeSpawn ()
    {
        Singleton.Instance.PoolManagerInstance.poolInstance.DeSpawn(gameObject);
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameEvents.CoinCollected?.Invoke(1);
            //Singleton.Instance.PoolManagerInstance.poolInstance.DeSpawn(gameObject);
            DeSpawn();
        }
    }
}
