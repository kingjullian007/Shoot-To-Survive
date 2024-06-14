using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Joystick joystick;
    private Vector3 movement;

    private void Update ()
    {
        movement.x = joystick.Horizontal();
        movement.z = joystick.Vertical();

        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }
}
