using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.Netcode;

public class SceneLoaderFloor : NetworkBehaviour
{
    public UnitySceneManager unitySceneManager;
    public string sceneToLoadName;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        IPlayable player = other.GetComponent<IPlayable>();
        if (player!=null)
        {
            NetworkManager.SceneManager.LoadScene(sceneToLoadName, LoadSceneMode.Single);
            //unitySceneManager.LoadSceneWithName(sceneToLoadName);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IPlayable player = collision.collider.GetComponent<IPlayable>();
        if (player!=null)
        {
            NetworkManager.SceneManager.LoadScene(sceneToLoadName, LoadSceneMode.Single);
            //unitySceneManager.LoadSceneWithName(sceneToLoadName);
        }
    }
}
