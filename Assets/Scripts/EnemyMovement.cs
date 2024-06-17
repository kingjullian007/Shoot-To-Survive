using UnityEngine;
using System;

public class EnemyMovement : MonoBehaviour
{
    public event Action<GameObject, Vector3, Vector3> OnPositionChanged;
    private Vector3 lastPosition;

    private void Start ()
    {
        lastPosition = transform.position;
    }

    private void Update ()
    {
        if (Singleton.Instance.GameManagerInstance.CurrentState != GameState.GamePlay)
        {
            return;
        }

        var currentPosition = transform.position;
        if (currentPosition != lastPosition)
        {
            OnPositionChanged?.Invoke(gameObject, lastPosition, currentPosition);
            lastPosition = currentPosition;
        }
    }
}
