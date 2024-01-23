using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    UnityEvent unityEventHost;

    [SerializeField]
    UnityEvent unityEventClient;

    [SerializeField]
    UnityEvent unityEventServer;

    public void StartHost()
    {
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Host started");
        }
        else
        {
            Debug.Log("Host failed to Start");
        }
    }

    public void StartServer()
    {
        if (NetworkManager.Singleton.StartServer())
        {
            Debug.Log("Server started");
        }
        else
        {
            Debug.Log("Server failed to Start");
        }
    }

    public void StartClient()
    {
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client started");
        }
        else
        {
            Debug.Log("Client failed to Start");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartHost();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            StartClient();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            StartServer();
        }
    }
}