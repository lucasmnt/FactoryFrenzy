using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Barrel : EditorObjects
{
    [SerializeField]
    MeshCollider topCollider;
    [SerializeField]
    MeshCollider barrelCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // M�thode pour attribuer l'autorit� au client local
    public void AssignAuthorityToClient()
    {
        /*if (IsServer) // Assurez-vous que le script est ex�cut� sur le serveur
        {
            NetworkObject networkObject = GetComponent<NetworkObject>();

            if (networkObject!=null)
            {
                // Attribution de l'autorit� au serveur
                networkObject.AssignClientAuthority(NetworkManager.Singleton.LocalClientId);
            }
        }*/
    }
}
