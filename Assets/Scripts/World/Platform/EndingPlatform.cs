using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPlatform : EditorObjects
{
    RoundManager roundManager;
    // Start is called before the first frame update
    void Start()
    {
        roundManager = GetComponent<RoundManager>();
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
                if(player.GetFinishedState()==false)
                {
                    roundManager.AddPlayerToArrivalList(player.HasFinished());
                }
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
                if (player.GetFinishedState()==false)
                {
                    roundManager.AddPlayerToArrivalList(player.HasFinished());
                }
            }
        }
    }
}
