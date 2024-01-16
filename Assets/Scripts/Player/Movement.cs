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
    public GameObject camHolder1st;
    public GameObject camHolder3rd;
    public Vector3 camOffset;
    public float cameraSwitchSpeed = 5f;
    public PlayerPOV currentPOV = PlayerPOV.ThirdPerson;

    private Rigidbody rb;
    private bool isGrounded;

    [SerializeField]
    public LayerMask interactLayerMask;
    public float interactRange = 10f;

    public enum PlayerPOV
    {
        FirstPerson,
        ThirdPerson,
        External,
        Cinematic
    }

    public override void OnNetworkSpawn()
    {
        camHolder1st.SetActive(false);
        camHolder3rd.SetActive(IsOwner);
        base.OnNetworkSpawn(); 
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;

        if (IsLocalPlayer)
        {
            // Activer les caméras seulement pour le joueur local
            playerCam.gameObject.SetActive(true);
            camHolder1st.SetActive(true);
            camHolder3rd.SetActive(true);
        }
        else
        {
            // Désactiver les caméras pour les joueurs distants
            playerCam.gameObject.SetActive(false);
            camHolder1st.SetActive(false);
            camHolder3rd.SetActive(false);
        }
    }

    private void Update()
    {
        CheckGrounded();
        HandlePlayerMovement();
        HandlePlayerJump();
        HandlePlayerLook();
        HandleTryInteract();
        HandleCameraSwitching();
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

        // Si votre Rigidbody est cinématique, utilisez directement la fonction Translate
        if (rb.isKinematic)
        {
            transform.Translate(movement*speed*Time.deltaTime);
        }
        else
        {
            // Sinon, utilisez la vélocité seulement pour les corps non cinématiques
            rb.velocity=new Vector3(movement.x*speed, rb.velocity.y, movement.z*speed);
        }
    }

    private void HandleCameraSwitching()
    {
        if (!IsOwner) return;  // Seul le propriétaire devrait gérer le changement de caméra

        if (Input.GetKeyDown(KeyCode.P))
        {
            // Inversez l'état entre première personne et troisième personne
            currentPOV=(currentPOV==PlayerPOV.FirstPerson) ? PlayerPOV.ThirdPerson : PlayerPOV.FirstPerson;

            // Définir la position cible en fonction de l'état actuel
            Vector3 targetPosition = (currentPOV==PlayerPOV.FirstPerson) ? camHolder1st.transform.position : camHolder3rd.transform.position;

            // Commencer la transition de caméra en ajustant directement la position
            playerCam.transform.position=targetPosition;
        }
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