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
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.CompareTag("EnemyAggressive") || other.CompareTag("EnemyFixed"))
        {
            enemiesInZone.Remove(other.gameObject);
        }
    }

    public List<GameObject> GetEnemiesInZone ()
    {
        return enemiesInZone;
    }

    public void RemoveEnemyFromZone (GameObject enemy)
    {
        enemiesInZone.Remove(enemy);
    }
}
