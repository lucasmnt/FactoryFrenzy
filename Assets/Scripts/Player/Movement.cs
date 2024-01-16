using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    public float jumpForce = 5f;
    public LayerMask groundMask;

    public Camera playerCam;
    public GameObject camHolder;
    public Vector3 camOffset;

    private Rigidbody rb;
    private bool isGrounded;

    [SerializeField]
    public LayerMask interactLayerMask;
    public float interactRange = 5f;




    public override void OnNetworkSpawn()
    {
        camHolder.SetActive(IsOwner);
        base.OnNetworkSpawn(); 
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true; // Freeze rotation to prevent camera tilting
    }

    private void Update()
    {
        CheckGrounded();
        HandlePlayerMovement();
        HandlePlayerJump();
        HandlePlayerLook();
        HandleTryInteract();
    }

    private void CheckGrounded()
    {
        // Check if the player is grounded
        isGrounded=Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);
    }

    private void HandlePlayerMovement()
    {
        // Player Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward*vertical+transform.right*horizontal;
        rb.velocity=new Vector3(movement.x*speed, rb.velocity.y, movement.z*speed);
    }

    private void HandlePlayerJump()
    {
        // Player Jump
        if (isGrounded&&Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
        }
    }

    private void HandlePlayerLook()
    {
        // Player Look
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up*mouseX*sensitivity);
        playerCam.transform.Rotate(Vector3.left*mouseY*sensitivity);
        //camHolder.transform.position = camHolder.transform.position + camOffset;

        // Clamp vertical camera rotation to prevent flipping
        Quaternion currentRotation = playerCam.transform.localRotation;
        float clampedXRotation = Mathf.Clamp(currentRotation.x, -0.59f, 0.59f);
        currentRotation=new Quaternion(clampedXRotation, currentRotation.y, currentRotation.z, currentRotation.w);
        playerCam.transform.localRotation=currentRotation;
    }

    private void HandleTryInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Créez un rayon depuis la caméra vers l'avant
            Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
            RaycastHit hit;

            // Vérifiez s'il y a une collision avec un objet portant l'interface IInteractable
            if (Physics.Raycast(ray, out hit, interactRange, interactLayerMask))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable!=null)
                {
                    // Appel à la méthode Interact de l'objet
                    interactable.Interact();
                }
            }
        }
    }
}