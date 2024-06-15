using UnityEngine;

public class EnemyHealth : Health
{
    // Add any enemy-specific properties or methods here

    protected override void Die ()
    {
        base.Die();
        Debug.Log("Enemy died!");
        // Add drop coins logic or other enemy-specific death behavior here
    }
}
