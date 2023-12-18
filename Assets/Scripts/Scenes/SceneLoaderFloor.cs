using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoaderFloor : MonoBehaviour
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
            unitySceneManager.LoadSceneWithName(sceneToLoadName);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IPlayable player = collision.collider.GetComponent<IPlayable>();
        if (player!=null)
        {
            unitySceneManager.LoadSceneWithName(sceneToLoadName);
        }
    }
}
