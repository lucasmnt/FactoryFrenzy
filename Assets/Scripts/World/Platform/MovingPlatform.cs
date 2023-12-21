using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MovementType
    {
        Loop,
        PingPong
    }

    [SerializeField]
    public MovementType movementType = MovementType.Loop;

    [SerializeField]
    public List<Transform> waypoints = new List<Transform>();

    [SerializeField]
    public float moveSpeed = 5f;

    [SerializeField]
    public BoxCollider detectionBox;

    Vector3 previousPosition;
    private int currentWaypointIndex = 0;
    private int direction = 1; // 1: Forward, -1: Backward

    void Update()
    {
        previousPosition=transform.position;
        MovePlatform();
        DetectPlayableObjects();
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
        if (waypoints.Count == 0)
            return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

        if (transform.position == targetWaypoint.position)
        {
            // Change the current waypoint index based on movement type
            if (movementType == MovementType.Loop)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            }
            else if (movementType == MovementType.PingPong)
            {
                currentWaypointIndex += direction;

                if (currentWaypointIndex == waypoints.Count - 1 || currentWaypointIndex == 0)
                {
                    direction *= -1; // Reverse the direction at endpoints
                }
            }
        }
    }
}