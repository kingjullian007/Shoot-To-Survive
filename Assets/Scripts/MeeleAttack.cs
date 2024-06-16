
using UnityEngine;

public class MeeleAttack : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
