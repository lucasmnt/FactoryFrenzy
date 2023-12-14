using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPlatform : EditorObjects
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other)
        {
            // Check if the hit object implements the IInteractable interface
            IPlayable player = other.GetComponent<IPlayable>();
            if (player!=null)
            {
                player.UpdateCurrentCheckpoint(this.gameObject);
            }
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
                player.UpdateCurrentCheckpoint(this.gameObject);
            }
        }
    }
}
