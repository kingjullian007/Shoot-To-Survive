using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update ()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
