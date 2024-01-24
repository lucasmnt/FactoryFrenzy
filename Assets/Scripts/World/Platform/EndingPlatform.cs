using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EndingPlatform : EditorObjects
{
    public RoundManager roundManager;

    void Start()
    {
        roundManager=GetComponent<RoundManager>();
        GetComponent<NetworkObject>().RemoveOwnership();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other!=null)
        {
            // Check if the hit object implements the IInteractable interface
            IPlayable player;
            if (other.TryGetComponent(out player))
            {
                if (!player.GetFinishedState())
                {
                    // Call the ServerRpc to notify all clients
                    roundManager.AddPlayerToArrivalList(player.HasFinished());
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider!=null)
        {
            // Check if the hit object implements the IInteractable interface
            IPlayable player;
            if (collision.collider.TryGetComponent(out player))
            {
                if (!player.GetFinishedState())
                {
                    roundManager.AddPlayerToArrivalList(player.HasFinished());
                }
            }
        }
    }
}