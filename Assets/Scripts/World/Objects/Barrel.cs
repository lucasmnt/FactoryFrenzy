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

    // Méthode pour attribuer l'autorité au client local
    public void AssignAuthorityToClient()
    {
        /*if (IsServer) // Assurez-vous que le script est exécuté sur le serveur
        {
            NetworkObject networkObject = GetComponent<NetworkObject>();

            if (networkObject!=null)
            {
                // Attribution de l'autorité au serveur
                networkObject.AssignClientAuthority(NetworkManager.Singleton.LocalClientId);
            }
        }*/
    }
}
