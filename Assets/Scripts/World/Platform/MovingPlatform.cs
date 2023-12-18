using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    public MovementType movementType = MovementType.Horizontal;

    [SerializeField]
    public MovementWay movementWay = MovementWay.Normal;

    [SerializeField]
    public float maxDistanceMoved = 15;

    [SerializeField]
    public float moveSpeed = 5;

    [SerializeField]
    public BoxCollider detectionBox;


    private Vector3 originalPosition;
    private Vector3 previousPosition;

    void Start()
    {
        originalPosition=transform.position;
        previousPosition=originalPosition;
    }

    void Update()
    {
        MovePlatform();
        DetectPlayableObjects();

        // Update the previous position
        previousPosition=transform.position;
    }

    void DetectPlayableObjects()
    {
        // Check for objects with IPlayable interface in the platform's vicinity using a box collider
        Collider[] colliders = Physics.OverlapBox(detectionBox.bounds.center, detectionBox.bounds.extents, detectionBox.transform.rotation);
        foreach (Collider collider in colliders)
        {
            IPlayable iPlayable = collider.GetComponent<IPlayable>();
            if (iPlayable!=null)
            {
                // Calculate the platform movement
                Vector3 platformMovement = transform.position-previousPosition;

                // Give the player the platform's movement
                iPlayable.StickToPlatform(platformMovement);
            }
        }
    }

    void MovePlatform()
    {
        switch (movementType)
        {
            case MovementType.Horizontal:
                if(movementWay == MovementWay.Normal)
                {
                    float horizontalOffset = Mathf.Cos(Time.time*moveSpeed)*maxDistanceMoved;
                    transform.position=originalPosition+new Vector3(horizontalOffset, 0, 0);
                    break;
                }
                else
                {
                    float horizontalOffset = - Mathf.Cos(Time.time*moveSpeed)*maxDistanceMoved;
                    transform.position=originalPosition+new Vector3(horizontalOffset, 0, 0);
                    break;
                }
            case MovementType.Vertical:
                if (movementWay==MovementWay.Normal)
                {
                    float verticalOffset = Mathf.Cos(Time.time*moveSpeed)*maxDistanceMoved;
                    transform.position=originalPosition+new Vector3(0, verticalOffset, 0);
                    break;
                }
                else
                {
                    float verticalOffset = - Mathf.Cos(Time.time*moveSpeed)*maxDistanceMoved;
                    transform.position=originalPosition+new Vector3(0, verticalOffset, 0);
                    break;
                }
            case MovementType.Diagonal:
                if (movementWay==MovementWay.Normal)
                {
                    float diagonalOffset = Mathf.Cos(Time.time*moveSpeed)*maxDistanceMoved;
                    transform.position=originalPosition+new Vector3(diagonalOffset, diagonalOffset, 0);
                    break;
                }
                else
                {
                    float diagonalOffset = Mathf.Cos(Time.time*moveSpeed)*maxDistanceMoved;
                    transform.position=originalPosition+new Vector3(diagonalOffset, diagonalOffset, 0);
                    break;
                }
        }
    }
}