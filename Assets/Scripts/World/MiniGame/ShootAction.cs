using System.Threading;
using UnityEngine;

public class ShootAction : MonoBehaviour
{
    public float limiteRotationHorizontale = 35f; // Ajustez selon vos besoins
    public float limiteRotationVerticale = 15f;   // Ajustez selon vos besoins

    private float rotationHorizontale = 0f;
    private float rotationVerticale = 0f;

    public Transform head;
    public Transform barrel;
    public GameObject _projectile;

    void Update()
    {
        // Obtenez les mouvements de la souris ou des axes de manette
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Appliquez la rotation horizontale et limitez-la
        rotationHorizontale += mouseX;
        rotationHorizontale = Mathf.Clamp(rotationHorizontale, -limiteRotationHorizontale, limiteRotationHorizontale);

        // Appliquez la rotation verticale et limitez-la
        rotationVerticale -= mouseY; // Inversez la rotation verticale pour correspondre aux conventions Unity
        rotationVerticale = Mathf.Clamp(rotationVerticale, -limiteRotationVerticale, limiteRotationVerticale);

        // Appliquez les rotations à la caméra ou au personnage
        transform.localRotation = Quaternion.Euler(rotationVerticale, rotationHorizontale, 0f);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject clone = Instantiate(_projectile, barrel.position, head.rotation);
        clone.GetComponent<Rigidbody>().AddForce(head.forward * 1500);
        Destroy(clone, 10);
    }
}