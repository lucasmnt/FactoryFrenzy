using UnityEngine;

public class Trampoline : EditorObjects
{
    public float jumpForce = 10f; // Force de saut du trampoline

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.collider.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                // Appliquer une force vers le haut au Rigidbody du joueur
                playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
