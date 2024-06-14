using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Joystick joystick;

    private void Update ()
    {
        // Get input from the joystick
        var horizontalInput = joystick.Horizontal();
        var verticalInput = joystick.Vertical();

        // Calculate the movement direction based on input
        var moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Check if there is any movement input
        if (moveDirection != Vector3.zero)
        {
            // Rotate the player to face the movement direction
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }

        // Apply movement
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }
}

