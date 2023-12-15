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
        anim=gameObject.GetComponent<Animation>();
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isFalling)
        {
            IPlayable player = other.GetComponent<IPlayable>();
            if (player!=null)
            {
                isFalling=true;
                StartCoroutine(FallAndRespawn());
                
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

    // Plays an animation clip.
    private void PlayAnimation(string animationName)
    {
        anim.Play(animationName);
    }

    private IEnumerator FallAndRespawn()
    {
        PlayAnimation("FallingPlatform"); // Play animation
        yield return new WaitForSeconds(3f); // Wait 5 seconds making fall the platform

        StartCoroutine(FallPlatform(20f)); // make the platform fall 20 units

        yield return new WaitForSeconds(2f);
        Respawn();
        yield return new WaitForSeconds(1f);
        isFalling=false;
    }

    private IEnumerator FallPlatform(float distanceToFall)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = originalPosition-new Vector3(0f, distanceToFall, 0f);

        while (elapsedTime<1f)
        {
            transform.position=Vector3.Lerp(initialPosition, targetPosition, elapsedTime);
            elapsedTime+=Time.deltaTime*2f; // Contrôlez la vitesse de descente ici
            yield return null;
        }

        transform.position=targetPosition;
    }

    private void Respawn()
    {
        this.transform.position=originalPosition;
        this.transform.rotation=originalRotation;
        PlayAnimation("RespawningPlatform");
    }
}