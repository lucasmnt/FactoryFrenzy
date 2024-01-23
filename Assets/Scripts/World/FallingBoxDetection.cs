using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBoxDetection : MonoBehaviour
{
    [SerializeField]
    private float minHeight = -20f; // Hauteur minimale permise

    private Transform playerTransform;

    public void SetPlayer(Transform player)
    {
        playerTransform = player;
    }

    private void Update()
    {
        if (playerTransform!=null)
        {
            transform.position=new Vector3(playerTransform.position.x, minHeight, playerTransform.position.z);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider)
        {
            // Check if the hit object implements the IInteractable interface
            IPlayable player = collision.collider.GetComponent<IPlayable>();
            if (player!=null)
            {
                player.Fell();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other)
        {
            // Check if the hit object implements the IInteractable interface
            IPlayable player = other.GetComponent<IPlayable>();
            if (player!=null)
            {
                player.Fell();
            }
        }
    }
}
