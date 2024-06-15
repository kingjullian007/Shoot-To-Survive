using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update ()
    {
        // Make the object face the camera
        transform.LookAt(Camera.main.transform);
        // Optionally, reverse the direction to make it face away from the camera
        transform.Rotate(0, 180, 0);
    }
}
