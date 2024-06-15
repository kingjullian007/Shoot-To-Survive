
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 25f;
    private Transform bulletTransform;

    private void Start ()
    {
        bulletTransform = GetComponent<Transform>();
    }
    private void Update ()
    {
        bulletTransform.Translate(speed * Time.deltaTime * bulletTransform.forward, Space.World);
    }

    private void OnTriggerEnter (Collider other)
    {
        if(other.CompareTag("DeadEnd"))
        {
            Singleton.Instance.PoolManagerInstance.DeSpawn(gameObject);
            Debug.Log("Whoop");
        }
    }
}
