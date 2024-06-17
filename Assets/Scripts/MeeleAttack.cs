
using UnityEngine;

public class MeeleAttack : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<IDamageable>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
