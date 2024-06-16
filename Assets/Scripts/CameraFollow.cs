using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // Player's transform
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -10f); // Offset from the player

    private void Update ()
    {
        if (target != null)
        {
            // Calculate the target position with offset
            Vector3 targetPosition = target.position + offset;

            // Smoothly move the camera towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
        }
    }
}
