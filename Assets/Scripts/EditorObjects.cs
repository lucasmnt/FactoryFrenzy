using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EditorObjects : NetworkBehaviour
{
    public Transform objTransform;
    public float xPos, yPos, zPos;
    public float xRot, yRot, ZRot;
    public float xSca, ySca, zSca;

    private void Start()
    {
        // Assurez-vous que l'objet est autoritaire côté client au démarrage
        if (objTransform==null)
        {
            objTransform=this.transform;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("collided !");
            NetworkObject playerNO = collision.collider.GetComponent<NetworkObject>();

            if (playerNO == null)
            {
                playerNO = collision.collider.GetComponentInParent<NetworkObject>();
            }

            if (playerNO!=null)
            {
                // On collision, give authority to the client
                //this.NetworkObject.ChangeOwnership(playerNO.OwnerClientId);

                ulong playerClientId = playerNO.OwnerClientId;
                RequestChangeOwnershipServerRpc(playerClientId);

                // Get the client ID of the player
                
                Debug.Log("Player Client ID: "+ playerClientId);
            }
        }
    }

    [ServerRpc]
    private void RequestChangeOwnershipServerRpc(ulong newOwnerClientId)
    {
        // Get the NetworkObject of this object
        NetworkObject networkObject = GetComponent<NetworkObject>();

        if (networkObject!=null)
        {
            // Change ownership on the server
            networkObject.ChangeOwnership(newOwnerClientId);
        }
    }
}
