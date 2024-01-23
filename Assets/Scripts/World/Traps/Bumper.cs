using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : EditorObjects
{
    [SerializeField] string playerTag;
    [SerializeField] float baseBounceForce = 5f;
    [SerializeField] float velocityMultiplier = 0.2f;
    [SerializeField] int minFramesToApplyForce = 5;
    [SerializeField] int maxFramesToApplyForce = 30;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(playerTag))
        {
            Rigidbody otherRB = collision.rigidbody;

            // Obtenir la direction opposée à la normale de la collision
            Vector3 forceDirection = -collision.contacts[0].normal;
            Vector3 exemple = new Vector3(forceDirection.x, forceDirection.y/10, forceDirection.z);

            // Ajuster la magnitude de la force en fonction de la vélocité actuelle du joueur
            float finalBounceForce = baseBounceForce+(otherRB.velocity.magnitude*velocityMultiplier);

            // Calculer le nombre de frames en fonction de la vélocité relative
            int framesToApplyForce = Mathf.Clamp((int)(otherRB.velocity.magnitude*0.1f), minFramesToApplyForce, maxFramesToApplyForce);

            // Appliquer la force en direction opposée avec la force spécifiée sur plusieurs frames
            StartCoroutine(ApplyForceOverFrames(otherRB, exemple*finalBounceForce, framesToApplyForce));
        }
    }

    IEnumerator ApplyForceOverFrames(Rigidbody rb, Vector3 force, int frames)
    {
        for (int i = 0;i<frames;i++)
        {
            // Appliquer une fraction de la force à chaque frame
            rb.AddForce(force/frames, ForceMode.Impulse);
            yield return null;
        }
    }
}