using System.Collections;
using UnityEngine;

public class FallingPlatformV2 : MonoBehaviour
{
    [SerializeField]
    private Rigidbody[] childRigidbodies;

    [SerializeField]
    private Vector3[] childPositions;

    [SerializeField]
    private Quaternion childRotations;

    [SerializeField]
    private BoxCollider collider;

    [SerializeField]
    private MeshRenderer renderer;

    private bool isFalling = false;

    void Start()
    {
        childRigidbodies = GetComponentsInChildren<Rigidbody>();
        collider=GetComponent<BoxCollider>();
        renderer=GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isFalling)
        {
            IPlayable player = other.GetComponent<IPlayable>();
            if (player!=null)
            {
                isFalling=true;
                StartCoroutine(BreakAndRespawn());
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
                isFalling=true;
                StartCoroutine(BreakAndRespawn());
            }
        }
    }

    private IEnumerator BreakAndRespawn()
    {
        // Play animation
        yield return new WaitForSeconds(3f); // Wait 3 seconds for the platform to fall

        StartCoroutine(BreakPlatform()); // Make the platform break

        yield return new WaitForSeconds(2f);
        Respawn();
        yield return new WaitForSeconds(1f);
        isFalling=false;
    }

    private IEnumerator BreakPlatform()
    {
        // Disable the collider
        collider.enabled=false;
        renderer.enabled=false;

        // Enable the rigidbodies
        foreach (var rb in childRigidbodies)
        {
            rb.isKinematic=false;
            rb.useGravity=true;

            // Ajoute une impulsion aléatoire vers le bas
            Vector3 randomForce = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            rb.AddForce(randomForce*Random.Range(5f, 10f), ForceMode.Impulse);
        }

        yield return null;
    }

    private void Respawn()
    {
        // Reset the position and rotation


        // Enable the collider
        collider.enabled=true;
        renderer.enabled=true;

        // Disable the rigidbodies
        foreach (var rb in childRigidbodies)
        {
            rb.isKinematic=true;
            rb.useGravity=false;
        }
    }
}