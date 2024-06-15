using System.Collections.Generic;
using UnityEngine;

public class DefenseZone : MonoBehaviour
{
    private List<GameObject> enemiesInZone = new List<GameObject>();

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("EnemyAggressive") || other.CompareTag("EnemyFixed"))
        {
            enemiesInZone.Add(other.gameObject);
            // Handle shooting bullets or other defense mechanisms
            Debug.Log("One enemy detected!");
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.CompareTag("EnemyAggressive") || other.CompareTag("EnemyFixed"))
        {
            enemiesInZone.Remove(other.gameObject);
            // Handle stopping shooting or other defense mechanisms
            Debug.Log("One enemy exited!");

        }
    }

    public List<GameObject> GetEnemiesInZone ()
    {
        return enemiesInZone;
    }
}
