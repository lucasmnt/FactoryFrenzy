using UnityEngine;

public class ShootAction : MonoBehaviour
{
    public float limiteRotationVerticale = 15f;
    public float limiteRotationHorizontale = 35f;

    private float rotationHorizontale = 0f;
    private float rotationVerticale = 0f;

    public Transform head;
    public Transform barrel;
    public GameObject _projectile;

    [SerializeField]
    private Direction direction = Direction.Forward;

    public enum Direction 
    {
       Left,
       Right,
       Forward,
       Backward
    }

    // Ajout de la variable pour la rotation en Y de la tourelle
    private float rotationY;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        switch (direction)
        {
            case Direction.Left:
                rotationHorizontale += mouseX;
                rotationHorizontale = Mathf.Clamp(rotationHorizontale, 235, 305);
                break; 
            case Direction.Right:
                rotationHorizontale += mouseX;
                rotationHorizontale = Mathf.Clamp(rotationHorizontale, 55, 125);
                break; 
            case Direction.Forward:
                rotationHorizontale += mouseX;
                rotationHorizontale = Mathf.Clamp(rotationHorizontale, -35, 35);
                break;
            case Direction.Backward:
                rotationHorizontale += mouseX;
                rotationHorizontale = Mathf.Clamp(rotationHorizontale, 145, 215);
                break;
            /*case default:
                Debug.Log("Error");
                break;*/
        }

        rotationVerticale -= mouseY;
        rotationVerticale = Mathf.Clamp(rotationVerticale, -limiteRotationVerticale, limiteRotationVerticale);

        // Ajout de la récupération de la rotation en Y de la tourelle
        rotationY = barrel.rotation.eulerAngles.y;

        head.localRotation = Quaternion.Euler(0f, 0f, -rotationVerticale);
        transform.localRotation = Quaternion.Euler(0f, rotationHorizontale, 0f);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Obtenir la rotation actuelle du canon
        Quaternion currentBarrelRotation = barrel.rotation;

        // Utiliser la rotation actuelle pour calculer la direction de tir
        Vector3 shootingDirection = currentBarrelRotation * Vector3.right;

        Debug.Log("Canon Rotation: " + barrel.rotation.eulerAngles);
        Debug.Log("Projectile Rotation: " + barrel.rotation.eulerAngles);

        // Utiliser la rotation en Y de la tourelle pour ajuster les limites de rotation horizontale
        float adjustedRotationHorizontale = Mathf.Clamp(rotationY, -limiteRotationHorizontale, limiteRotationHorizontale);

        GameObject clone = Instantiate(_projectile, barrel.position, Quaternion.LookRotation(shootingDirection));
        clone.GetComponent<Rigidbody>().AddForce(shootingDirection * 1500);
        Destroy(clone, 10);
    }
}
