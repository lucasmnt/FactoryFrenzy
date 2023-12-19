using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBoxDetection : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
