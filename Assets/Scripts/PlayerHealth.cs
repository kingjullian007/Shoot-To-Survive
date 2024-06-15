using UnityEngine;

public class PlayerHealth : Health
{
    protected override void Start ()
    {
        base.Start();

        // Set the Slider's value to its maximum
        if (healthSlider != null)
        {
            healthSlider.value = healthSlider.maxValue;
        }
    }
    protected override void Die ()
    {
        base.Die();
        Debug.Log("Player died!");
        // Add game over logic or respawn logic here
    }
}
