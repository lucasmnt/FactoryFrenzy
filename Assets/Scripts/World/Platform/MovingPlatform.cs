using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MovementType
    {
        Vertical,
        Horizontal,
        Diagonal
    }

    [SerializeField]
    public MovementType movementType = MovementType.Horizontal;

    [SerializeField]
    public float maxDistanceMoved = 15;

    [SerializeField]
    public float moveSpeed = 5;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition=transform.position;
    }

    void Update()
    {
        switch (movementType)
        {
            case MovementType.Horizontal:
                float horizontalOffset = Mathf.Sin(Time.time*moveSpeed)*maxDistanceMoved;
                transform.position=originalPosition+new Vector3(horizontalOffset, 0, 0);
                break;
            case MovementType.Vertical:
                float verticalOffset = Mathf.Sin(Time.time*moveSpeed)*maxDistanceMoved;
                transform.position=originalPosition+new Vector3(0, verticalOffset, 0);
                break;
            case MovementType.Diagonal:
                float diagonalOffset = Mathf.Sin(Time.time*moveSpeed)*maxDistanceMoved;
                transform.position=originalPosition+new Vector3(diagonalOffset, diagonalOffset, 0);
                break;
        }
    }
}