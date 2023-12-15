using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField]
    public Animation anim;

    private bool isFalling = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        originalPosition=transform.position;
        originalRotation=transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isFalling)
        {
            IPlayable player = other.GetComponent<IPlayable>();
            if (player!=null)
            {
                StartCoroutine(FallAndRespawn());
                isFalling = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isFalling)
        {
            IPlayable player = collision.collider.GetComponent<IPlayable>();
            if (player!=null)
            {
                StartCoroutine(FallAndRespawn());
                isFalling=true;
            }
        }
    }

    private IEnumerator FallAndRespawn()
    {
        yield return new WaitForSeconds(5f); // Attendre 5 secondes avant de faire tomber la plateforme

        Fall();

        yield return new WaitForSeconds(5f); // Attendre 5 secondes avant de réapparaître la plateforme
        Respawn();
    }

    private void Fall()
    {
        // Ajouter le code pour faire tomber la plateforme (par exemple, désactiver le collider ou jouer une animation)
        Debug.Log("Platform is falling!");
    }

    private void Respawn()
    {
        // Ajouter le code pour réapparaître la plateforme à sa position d'origine
        transform.position=originalPosition;
        transform.rotation=originalRotation;
        Debug.Log("Platform respawned!");
        isFalling=false;
    }
}